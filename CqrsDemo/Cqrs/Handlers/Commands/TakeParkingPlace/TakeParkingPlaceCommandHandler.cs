using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CqrsDemo.Exceptions;
using CqrsDemo.Shared.Resources;
using CqrsDemo.Services.Commands;
using CqrsDemo.Services.Authentication;
using CqrsDemo.Infrastructure.Database;
using MediatR;

namespace CqrsDemo.Cqrs.Handlers.Commands.TakeParkingPlace
{
    public class TakeParkingPlaceCommandHandler : IRequestHandler<TakeParkingPlaceCommand, Unit>
    {
        private readonly MainDbContext FMainDbContext;
        
        private readonly ICommands FCommandStore;
        
        private readonly IAuthentication FAuthentication;

        public TakeParkingPlaceCommandHandler(MainDbContext AMainDbContext, ICommands ACommandStore, IAuthentication AAuthentication) 
        {
            FMainDbContext = AMainDbContext;
            FCommandStore = ACommandStore;
            FAuthentication = AAuthentication;
        }

        public async Task<Unit> Handle(TakeParkingPlaceCommand ARequest, CancellationToken ACancellationToken)
        {
            var LParking = (await FMainDbContext.Parking
                .ToListAsync(ACancellationToken))
                .FirstOrDefault(AParking => AParking.Name == ARequest.ParkingName);

            if (LParking == null)
                throw new BusinessException(nameof(ErrorCodes.CANNOT_FIND_PARKING), ErrorCodes.CANNOT_FIND_PARKING);

            if (!LParking.IsOpened)
                throw new BusinessException(nameof(ErrorCodes.PARKING_ALREADY_CLOSED), ErrorCodes.PARKING_ALREADY_CLOSED);

            var LParkingPlace = (await FMainDbContext.ParkingPlaces
                .ToListAsync(ACancellationToken))
                .FirstOrDefault(AParkingPlace => AParkingPlace.ParkingName == ARequest.ParkingName && AParkingPlace.Number == ARequest.PlaceNumber);

            if (LParkingPlace == null)
                throw new BusinessException(nameof(ErrorCodes.CANNOT_FIND_PARKING_PLACE), ErrorCodes.CANNOT_FIND_PARKING_PLACE);

            if (!LParkingPlace.IsFree)
                throw new BusinessException(nameof(ErrorCodes.PARKING_ALREADY_TAKEN), ErrorCodes.PARKING_ALREADY_TAKEN);

            LParkingPlace.IsFree = false;
            LParkingPlace.UserId = FAuthentication.GetUserId;

            await FMainDbContext.SaveChangesAsync(ACancellationToken);
            await FCommandStore.Push(ARequest, ACancellationToken);
            return await Task.FromResult(Unit.Value);
        }
    }
}
