using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CqrsDemo.Exceptions;
using CqrsDemo.Shared.Resources;
using CqrsDemo.Services.Commands;
using CqrsDemo.Infrastructure.Database;
using CqrsDemo.Infrastructure.Domain.Entities;
using MediatR;

namespace CqrsDemo.Handlers.Commands.LeaveParkingPlace
{
    public class LeaveParkingPlaceCommandHandler : IRequestHandler<LeaveParkingPlaceCommand, Unit>
    {
        private readonly MainDbContext FMainDbContext;
        private readonly ICommands FCommandStore;

        public LeaveParkingPlaceCommandHandler(MainDbContext AMainDbContext, ICommands ACommandStore) 
        {
            FMainDbContext = AMainDbContext;
            FCommandStore = ACommandStore;
        }

        public async Task<Unit> Handle(LeaveParkingPlaceCommand ARequest, CancellationToken ACancellationToken)
        {
            var LParking = (await FMainDbContext.Parking
                .ToListAsync(ACancellationToken))
                .FirstOrDefault(Parking => Parking.Name == ARequest.ParkingName);

            if (LParking == null)
                throw new BusinessException(nameof(ErrorCodes.CANNOT_FIND_PARKING), ErrorCodes.CANNOT_FIND_PARKING);

            if (!LParking.IsOpened)
                throw new BusinessException(nameof(ErrorCodes.PARKING_ALREADY_CLOSED), ErrorCodes.PARKING_ALREADY_CLOSED);

            var LParkingPlace = (await FMainDbContext.ParkingPlaces
                .ToListAsync(ACancellationToken))
                .FirstOrDefault(Parking => Parking.ParkingName == ARequest.ParkingName && Parking.Number == ARequest.PlaceNumber);

            if (LParkingPlace == null)
                throw new BusinessException(nameof(ErrorCodes.CANNOT_FIND_PARKING_PLACE), ErrorCodes.CANNOT_FIND_PARKING_PLACE);

            if (LParkingPlace.IsFree)
                throw new BusinessException(nameof(ErrorCodes.PARKING_PLACE_STILL_FREE), ErrorCodes.PARKING_PLACE_STILL_FREE);

            LParkingPlace.IsFree = true;
            LParkingPlace.UserId = null;

            await FMainDbContext.SaveChangesAsync(ACancellationToken);
            await FCommandStore.Push(ARequest, ACancellationToken);
            return await Task.FromResult(Unit.Value);
        }
    }
}
