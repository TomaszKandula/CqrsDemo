using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CqrsDemo.Database;
using CqrsDemo.Services.Commands;
using CqrsDemo.Services.Authentication;
using MediatR;

namespace CqrsDemo.Handlers.Commands.TakeParkingPlace
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

        public async Task<Unit> Handle(TakeParkingPlaceCommand Request, CancellationToken CancellationToken)
        {
            var LParking = (await FMainDbContext.Parking
                .ToListAsync())
                .FirstOrDefault(p => p.Name == Request.ParkingName);

            if (LParking == null)
                throw new Exception($"Cannot find parking '{Request.ParkingName}'.");

            if (!LParking.IsOpened)
                throw new Exception($"The parking '{Request.ParkingName}' is closed.");

            var LParkingPlace = (await FMainDbContext.ParkingPlaces
                .ToListAsync())
                .FirstOrDefault(p => p.ParkingName == Request.ParkingName && p.Number == Request.PlaceNumber);

            if (LParkingPlace == null)
                throw new Exception($"Cannot find place #{Request.PlaceNumber} in the parking '{Request.ParkingName}'.");

            if (!LParkingPlace.IsFree)
                throw new Exception($"Parking place #{Request.PlaceNumber} is already taken.");

            LParkingPlace.IsFree = false;
            LParkingPlace.UserId = FAuthentication.GetUserId;

            await FMainDbContext.SaveChangesAsync();
            await FCommandStore.Push(Request);
            return await Task.FromResult(Unit.Value);
        }
    }
}
