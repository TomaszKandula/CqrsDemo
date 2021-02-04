using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CqrsDemo.Database;
using CqrsDemo.Database.Models;
using CqrsDemo.Models.Responses;
using CqrsDemo.Services.Commands;
using CqrsDemo.Handlers.Commands.Models;
using MediatR;

namespace CqrsDemo.Handlers.Commands
{
    public class HandleCreateParking : IRequestHandler<CreateParking, CommandResponse>
    {
        private readonly MainDbContext FMainDbContext;
        private readonly ICommands FCommandStore;

        public HandleCreateParking(MainDbContext AMainDbContext, ICommands ACommandStore) 
        {
            FMainDbContext = AMainDbContext;
            FCommandStore = ACommandStore;
        }

        public async Task<CommandResponse> Handle(CreateParking Request, CancellationToken CancellationToken)
        {
            var LPlaces = Enumerable.Range(1, Request.Capacity)
                .Select(ANumber =>
                {
                    return new ParkingPlace
                    {
                        ParkingName = Request.ParkingName,
                        Number = ANumber,
                        IsFree = true
                    };
                })
                .ToList();

            var LParking = new Parking
            {
                Name = Request.ParkingName,
                IsOpened = true,
                ParkingPlaces = LPlaces
            };

            FMainDbContext.Add(LParking);

            await FMainDbContext.SaveChangesAsync();
            await FCommandStore.Push(Request);
            return new CommandResponse { IsSucceeded = true };
        }
    }
}
