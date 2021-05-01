using MediatR;

namespace CqrsDemo.Cqrs.Handlers.Commands.OpenParking
{
    public class OpenParkingCommand : IRequest<Unit>
    {
        public string ParkingName { get; set; }
    }
}
