using System.Threading;
using System.Threading.Tasks;
using CqrsDemo.Database;
using CqrsDemo.Models.Responses;
using CqrsDemo.Services.Commands;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CqrsDemo.Handlers.Commands.Models;
using MediatR;

namespace CqrsDemo.Handlers.Commands
{

    public class HandleCloseParking : IRequestHandler<CloseParking, CommandResponse>
    {

        private readonly MainDbContext FMainDbContext;
        private readonly ICommands FCommandStore;

        public HandleCloseParking(MainDbContext AMainDbContext, ICommands ACommandStore) 
        {
            FMainDbContext = AMainDbContext;
            FCommandStore = ACommandStore;
        }

        public async Task<CommandResponse> Handle(CloseParking Request, CancellationToken CancellationToken) 
        {

            var LParking = (await FMainDbContext.Parking
                .ToListAsync()
                ).FirstOrDefault(p => p.Name == Request.ParkingName);

            //if (LParking == null)
            //    throw new Exception($"Cannot find parking '{ACommand.ParkingName}'.");

            //if (!LParking.IsOpened)
            //    throw new Exception($"Parking '{ACommand.ParkingName}' is already closed.");

            LParking.IsOpened = false;

            await FMainDbContext.SaveChangesAsync();
            await FCommandStore.Push(Request);
            return new CommandResponse { IsSucceeded = true };

        }

    }

}
