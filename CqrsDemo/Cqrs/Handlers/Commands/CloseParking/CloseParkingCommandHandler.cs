using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CqrsDemo.Database;
using CqrsDemo.Services.Commands;
using MediatR;

namespace CqrsDemo.Handlers.Commands.CloseParking
{
    public class CloseParkingCommandHandler : IRequestHandler<CloseParkingCommand, Unit>
    {
        private readonly MainDbContext FMainDbContext;
        private readonly ICommands FCommandStore;

        public CloseParkingCommandHandler(MainDbContext AMainDbContext, ICommands ACommandStore) 
        {
            FMainDbContext = AMainDbContext;
            FCommandStore = ACommandStore;
        }

        public async Task<Unit> Handle(CloseParkingCommand Request, CancellationToken CancellationToken) 
        {
            var LParking = (await FMainDbContext.Parking
                .ToListAsync())
                .FirstOrDefault(Parking => Parking.Name == Request.ParkingName);

            if (LParking == null)
                throw new Exception($"Cannot find parking '{Request.ParkingName}'.");

            if (!LParking.IsOpened)
                throw new Exception($"Parking '{Request.ParkingName}' is already closed.");

            LParking.IsOpened = false;

            await FMainDbContext.SaveChangesAsync();
            await FCommandStore.Push(Request);
            return await Task.FromResult(Unit.Value);
        }
    }
}
