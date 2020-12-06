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
                ).FirstOrDefault(p => p.Name == Request.ParkingName);

            //if (LParking == null)
            //    throw new Exception($"Cannot find parking '{ACommand.ParkingName}'.");

            //if (!LParking.IsOpened)
            //    throw new Exception($"The parking '{ACommand.ParkingName}' is closed.");

            var LParkingPlace = (await FMainDbContext.ParkingPlaces
                .ToListAsync()
                ).FirstOrDefault(p => p.ParkingName == Request.ParkingName && p.Number == Request.PlaceNumber);

            //if (LParkingPlace == null)
            //    throw new Exception($"Cannot find place #{ACommand.PlaceNumber} in the parking '{ACommand.ParkingName}'.");

            //if (LParkingPlace.IsFree)
            //    throw new Exception($"Parking place #{ACommand.PlaceNumber} is still free.");

            LParkingPlace.IsFree = true;
            LParkingPlace.UserId = null;

            await FMainDbContext.SaveChangesAsync();
            await FCommandStore.Push(Request);
            return new CommandResponse { IsSucceeded = true };

        }

    }

}
