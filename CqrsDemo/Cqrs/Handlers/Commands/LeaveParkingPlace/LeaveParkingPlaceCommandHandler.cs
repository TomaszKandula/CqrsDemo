using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CqrsDemo.Database;
using CqrsDemo.Services.Commands;
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

        public async Task<Unit> Handle(LeaveParkingPlaceCommand Request, CancellationToken CancellationToken)
        {
            var LParking = (await FMainDbContext.Parking
                .ToListAsync()
                ).FirstOrDefault(Parking => Parking.Name == Request.ParkingName);

            if (LParking == null)
                throw new Exception($"Cannot find parking '{Request.ParkingName}'.");

            if (!LParking.IsOpened)
                throw new Exception($"The parking '{Request.ParkingName}' is closed.");

            var LParkingPlace = (await FMainDbContext.ParkingPlaces
                .ToListAsync())
                .FirstOrDefault(Parking => Parking.ParkingName == Request.ParkingName && Parking.Number == Request.PlaceNumber);

            if (LParkingPlace == null)
                throw new Exception($"Cannot find place #{Request.PlaceNumber} in the parking '{Request.ParkingName}'.");

            if (LParkingPlace.IsFree)
                throw new Exception($"Parking place #{Request.PlaceNumber} is still free.");

            LParkingPlace.IsFree = true;
            LParkingPlace.UserId = null;

            await FMainDbContext.SaveChangesAsync();
            await FCommandStore.Push(Request);
            return await Task.FromResult(Unit.Value);
        }
    }
}
