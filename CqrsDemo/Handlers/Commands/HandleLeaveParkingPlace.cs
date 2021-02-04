using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CqrsDemo.Database;
using CqrsDemo.Models.Responses;
using CqrsDemo.Services.Commands;
using CqrsDemo.Handlers.Commands.Models;
using MediatR;

namespace CqrsDemo.Handlers.Commands
{
    public class HandleLeaveParkingPlace : IRequestHandler<LeaveParkingPlace, CommandResponse>
    {
        private readonly MainDbContext FMainDbContext;
        private readonly ICommands FCommandStore;

        public HandleLeaveParkingPlace(MainDbContext AMainDbContext, ICommands ACommandStore) 
        {
            FMainDbContext = AMainDbContext;
            FCommandStore = ACommandStore;
        }

        public async Task<CommandResponse> Handle(LeaveParkingPlace Request, CancellationToken CancellationToken)
        {
            var LParking = (await FMainDbContext.Parking
                .ToListAsync()
                ).FirstOrDefault(Parking => Parking.Name == Request.ParkingName);

            if (LParking == null)
                return new CommandResponse 
                { 
                    IsSucceeded = false,
                    ErrorCode = "no_such_parking",
                    ErrorDesc = $"Cannot find parking '{Request.ParkingName}'."
                };

            if (!LParking.IsOpened)
                return new CommandResponse 
                { 
                    IsSucceeded = false,
                    ErrorCode = "parking_closed",
                    ErrorDesc = $"The parking '{Request.ParkingName}' is closed."
                };

            var LParkingPlace = (await FMainDbContext.ParkingPlaces
                .ToListAsync())
                .FirstOrDefault(Parking => Parking.ParkingName == Request.ParkingName && Parking.Number == Request.PlaceNumber);

            if (LParkingPlace == null)
                return new CommandResponse 
                { 
                    IsSucceeded = false,
                    ErrorCode = "no_such_place",
                    ErrorDesc = $"Cannot find place #{Request.PlaceNumber} in the parking '{Request.ParkingName}'."
                };

            if (LParkingPlace.IsFree)
                return new CommandResponse 
                { 
                    IsSucceeded = false,
                    ErrorCode = "parking_free",
                    ErrorDesc = $"Parking place #{Request.PlaceNumber} is still free."
                };

            LParkingPlace.IsFree = true;
            LParkingPlace.UserId = null;

            await FMainDbContext.SaveChangesAsync();
            await FCommandStore.Push(Request);
            return new CommandResponse { IsSucceeded = true };
        }
    }
}
