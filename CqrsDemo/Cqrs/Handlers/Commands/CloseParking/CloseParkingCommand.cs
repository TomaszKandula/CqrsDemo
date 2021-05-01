using MediatR;

namespace CqrsDemo.Cqrs.Handlers.Commands.CloseParking
{
    public class CloseParkingCommand : IRequest<Unit>
    {
        public string ParkingName { get; set; }
    }
}
