using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CqrsDemo.Database;
using CqrsDemo.Models.Responses;
using CqrsDemo.Services.Commands;
using CqrsDemo.Services.Authentication;
using CqrsDemo.Handlers.Commands.Models;
using MediatR;

namespace CqrsDemo.Handlers.Commands
{

    public class HandleTakeParkingPlace : IRequestHandler<TakeParkingPlace, CommandResponse>
    {

        private readonly MainDbContext FMainDbContext;
        private readonly ICommands FCommandStore;
        private readonly IAuthentication FAuthentication;

        public HandleTakeParkingPlace(MainDbContext AMainDbContext, ICommands ACommandStore, IAuthentication AAuthentication) 
        {
            FMainDbContext = AMainDbContext;
            FCommandStore = ACommandStore;
            FAuthentication = AAuthentication;
        }

        public async Task<CommandResponse> Handle(TakeParkingPlace Request, CancellationToken CancellationToken)
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

            //if (!LParkingPlace.IsFree)
            //    throw new Exception($"Parking place #{ACommand.PlaceNumber} is already taken.");

            LParkingPlace.IsFree = false;
            LParkingPlace.UserId = FAuthentication.GetUserId;

            await FMainDbContext.SaveChangesAsync();
            await FCommandStore.Push(Request);
            return new CommandResponse { IsSucceeded = true };

        }

    }

}
