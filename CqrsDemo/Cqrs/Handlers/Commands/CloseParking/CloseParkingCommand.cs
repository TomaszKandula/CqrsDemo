using MediatR;

namespace CqrsDemo.Handlers.Commands.CloseParking
{
    public class CloseParkingCommand : IRequest<Unit>
    {
        public string ParkingName { get; set; }
    }
}
