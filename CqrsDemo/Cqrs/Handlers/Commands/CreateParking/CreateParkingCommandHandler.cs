using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CqrsDemo.Database;
using CqrsDemo.Database.Models;
using CqrsDemo.Services.Commands;
using MediatR;

namespace CqrsDemo.Handlers.Commands.CreateParking
{
    public class CreateParkingCommandHandler : IRequestHandler<CreateParkingCommand, Unit>
    {
        private readonly MainDbContext FMainDbContext;
        private readonly ICommands FCommandStore;

        public CreateParkingCommandHandler(MainDbContext AMainDbContext, ICommands ACommandStore) 
        {
            FMainDbContext = AMainDbContext;
            FCommandStore = ACommandStore;
        }

        public async Task<Unit> Handle(CreateParkingCommand ARequest, CancellationToken ACancellationToken)
        {
            var LPlaces = Enumerable.Range(1, ARequest.Capacity)
                .Select(ANumber =>
                {
                    return new ParkingPlace
                    {
                        ParkingName = ARequest.ParkingName,
                        Number = ANumber,
                        IsFree = true
                    };
                })
                .ToList();

            var LParking = new Parking
            {
                Name = ARequest.ParkingName,
                IsOpened = true,
                ParkingPlaces = LPlaces
            };

            FMainDbContext.Add(LParking);

            await FMainDbContext.SaveChangesAsync(ACancellationToken);
            await FCommandStore.Push(ARequest, ACancellationToken);
            return await Task.FromResult(Unit.Value);
        }
    }
}
