using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CqrsDemo.Database;
using CqrsDemo.Exceptions;
using CqrsDemo.Shared.Resources;
using CqrsDemo.Services.Commands;
using MediatR;

namespace CqrsDemo.Handlers.Commands.OpenParking
{
    public class OpenParkingCommandHandler : IRequestHandler<OpenParkingCommand, Unit>
    {
        private readonly MainDbContext FMainDbContext;
        private readonly ICommands FCommandStore;

        public OpenParkingCommandHandler(MainDbContext AMainDbContext, ICommands ACommandStore) 
        {
            FMainDbContext = AMainDbContext;
            FCommandStore = ACommandStore;
        }

        public async Task<Unit> Handle(OpenParkingCommand ARequest, CancellationToken ACancellationToken)
        {
            var LParking = (await FMainDbContext.Parking
                .ToListAsync(ACancellationToken))
                .FirstOrDefault(p => p.Name == ARequest.ParkingName);

            if (LParking == null)
                throw new BusinessException(nameof(ErrorCodes.CANNOT_FIND_PARKING), ErrorCodes.CANNOT_FIND_PARKING);

            if (LParking.IsOpened)
                throw new BusinessException(nameof(ErrorCodes.PARKING_ALREADY_OPENED), ErrorCodes.PARKING_ALREADY_OPENED);

            LParking.IsOpened = true;

            await FMainDbContext.SaveChangesAsync(ACancellationToken);
            await FCommandStore.Push(ARequest, ACancellationToken);
            return await Task.FromResult(Unit.Value);
        }
    }
}
