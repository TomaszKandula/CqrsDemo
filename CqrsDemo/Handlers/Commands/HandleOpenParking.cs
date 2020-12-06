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

    public class HandleOpenParking : IRequestHandler<OpenParking, CommandResponse>
    {

        private readonly MainDbContext FMainDbContext;
        private readonly ICommands FCommandStore;

        public HandleOpenParking(MainDbContext AMainDbContext, ICommands ACommandStore) 
        {
            FMainDbContext = AMainDbContext;
            FCommandStore = ACommandStore;
        }

        public async Task<CommandResponse> Handle(OpenParking Request, CancellationToken CancellationToken)
        {

            var LParking = (await FMainDbContext.Parking
                .ToListAsync()
                ).FirstOrDefault(p => p.Name == Request.ParkingName);

            //if (LParking == null)
            //    throw new Exception($"Cannot find parking '{Request.ParkingName}'.");

            //if (LParking.IsOpened)
            //    throw new Exception($"Parking '{Request.ParkingName}' is already opened.");

            LParking.IsOpened = true;

            await FMainDbContext.SaveChangesAsync();
            await FCommandStore.Push(Request);
            return new CommandResponse { IsSucceeded = true };

        }

    }

}
