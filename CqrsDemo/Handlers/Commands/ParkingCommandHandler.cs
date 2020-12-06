using System;
using System.Linq;
using System.Threading.Tasks;
using CqrsDemo.Database.Models;
using CqrsDemo.Database;
using CqrsDemo.Services.Commands;
using CqrsDemo.Services.Authentication;
using CqrsDemo.Handlers.Commands.Models;

namespace CqrsDemo.Handlers.Commands
{
    
    public class ParkingCommandHandler : IParkingCommandHandler
    {

        private readonly MainDbContext FMainDbContext;
        private readonly ICommands FCommandStore;
        private readonly IAuthentication FAuthentication;

        public ParkingCommandHandler(MainDbContext AMainDbContext, ICommands ACommandStore, IAuthentication AAuthentication) 
        {
            FMainDbContext = AMainDbContext;
            FCommandStore = ACommandStore;
            FAuthentication = AAuthentication;
        }

        public async Task Handle(CloseParking ACommand)
        {

            var LParking = FMainDbContext.Parking
                .FirstOrDefault(p => p.Name == ACommand.ParkingName);

            if (LParking == null)
                throw new Exception($"Cannot find parking '{ACommand.ParkingName}'.");

            if (!LParking.IsOpened)
                throw new Exception($"Parking '{ACommand.ParkingName}' is already closed.");

            LParking.IsOpened = false;

            await FMainDbContext.SaveChangesAsync();
            await FCommandStore.Push(ACommand);

        }

        public async Task Handle(CreateParking ACommand)
        {

            var LPlaces = Enumerable.Range(1, ACommand.Capacity)
                .Select(ANumber =>
                {
                    return new ParkingPlace
                    {
                        ParkingName = ACommand.ParkingName,
                        Number = ANumber,
                        IsFree = true
                    };
                })
                .ToList();

            var LParking = new Parking
            {
                Name = ACommand.ParkingName,
                IsOpened = true,
                ParkingPlaces = LPlaces
            };

            FMainDbContext.Add(LParking);

            await FMainDbContext.SaveChangesAsync();
            await FCommandStore.Push(ACommand);

        }

        public async Task Handle(LeaveParking ACommand)
        {

            var LParking = FMainDbContext.Parking
                .FirstOrDefault(p => p.Name == ACommand.ParkingName);

            if (LParking == null)
                throw new Exception($"Cannot find parking '{ACommand.ParkingName}'.");

            if (!LParking.IsOpened)
                throw new Exception($"The parking '{ACommand.ParkingName}' is closed.");

            var parkingPlace = FMainDbContext.ParkingPlaces
                .FirstOrDefault(p => p.ParkingName == ACommand.ParkingName && p.Number == ACommand.PlaceNumber);

            if (parkingPlace == null)
                throw new Exception($"Cannot find place #{ACommand.PlaceNumber} in the parking '{ACommand.ParkingName}'.");

            if (parkingPlace.IsFree)
                throw new Exception($"Parking place #{ACommand.PlaceNumber} is still free.");

            parkingPlace.IsFree = true;
            parkingPlace.UserId = null;

            await FMainDbContext.SaveChangesAsync();
            await FCommandStore.Push(ACommand);

        }

        public async Task Handle(OpenParking ACommand)
        {
            
            var LParking = FMainDbContext.Parking
                .FirstOrDefault(p => p.Name == ACommand.ParkingName);

            if (LParking == null)
                throw new Exception($"Cannot find parking '{ACommand.ParkingName}'.");

            if (LParking.IsOpened)
                throw new Exception($"Parking '{ACommand.ParkingName}' is already opened.");

            LParking.IsOpened = true;

            await FMainDbContext.SaveChangesAsync();
            await FCommandStore.Push(ACommand);

        }

        public async Task Handle(TakeParkingPlace ACommand)
        {

            var LParking = FMainDbContext.Parking
                .FirstOrDefault(p => p.Name == ACommand.ParkingName);

            if (LParking == null)
            {
                throw new Exception($"Cannot find parking '{ACommand.ParkingName}'.");
            }
            if (!LParking.IsOpened)
            {
                throw new Exception($"The parking '{ACommand.ParkingName}' is closed.");
            }

            var LParkingPlace = FMainDbContext.ParkingPlaces
                .FirstOrDefault(p => p.ParkingName == ACommand.ParkingName && p.Number == ACommand.PlaceNumber);

            if (LParkingPlace == null)
            {
                throw new Exception($"Cannot find place #{ACommand.PlaceNumber} in the parking '{ACommand.ParkingName}'.");
            }
            if (!LParkingPlace.IsFree)
            {
                throw new Exception($"Parking place #{ACommand.PlaceNumber} is already taken.");
            }

            LParkingPlace.IsFree = false;
            LParkingPlace.UserId = FAuthentication.GetUserId;
            
            await FMainDbContext.SaveChangesAsync();
            await FCommandStore.Push(ACommand);

        }

    }

}
