using MediatR;

namespace CqrsDemo.Cqrs.Handlers.Commands.CreateParking
{
    public class CreateParkingCommand : IRequest<Unit>
    {
        public string ParkingName { get; set; }

        public int Capacity { get; set; }
    }
}
