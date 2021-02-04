using MediatR;

namespace CqrsDemo.Handlers.Commands.OpenParking
{
    public class OpenParkingCommand : IRequest<Unit>
    {
        public string ParkingName { get; set; }
    }
}
