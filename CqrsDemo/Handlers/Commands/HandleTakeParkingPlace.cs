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
                .ToListAsync())
                .FirstOrDefault(p => p.Name == Request.ParkingName);

            if (LParking == null)
                return new CommandResponse 
                { 
                    IsSucceeded = false,
                    ErrorCode = "",
                    ErrorDesc = $"Cannot find parking '{Request.ParkingName}'."
                };

            if (!LParking.IsOpened)
                return new CommandResponse 
                { 
                    IsSucceeded = false,
                    ErrorCode = "",
                    ErrorDesc = $"The parking '{Request.ParkingName}' is closed."
                };

            var LParkingPlace = (await FMainDbContext.ParkingPlaces
                .ToListAsync())
                .FirstOrDefault(p => p.ParkingName == Request.ParkingName && p.Number == Request.PlaceNumber);

            if (LParkingPlace == null)
                return new CommandResponse 
                { 
                    IsSucceeded = false,
                    ErrorCode = "no_such_place",
                    ErrorDesc = $"Cannot find place #{Request.PlaceNumber} in the parking '{Request.ParkingName}'."
                };

            if (!LParkingPlace.IsFree)
                return new CommandResponse
                {
                    IsSucceeded = false,
                    ErrorCode = "parking_taken",
                    ErrorDesc = $"Parking place #{Request.PlaceNumber} is already taken."
                };

            LParkingPlace.IsFree = false;
            LParkingPlace.UserId = FAuthentication.GetUserId;

            await FMainDbContext.SaveChangesAsync();
            await FCommandStore.Push(Request);
            return new CommandResponse { IsSucceeded = true };
        }
    }
}
