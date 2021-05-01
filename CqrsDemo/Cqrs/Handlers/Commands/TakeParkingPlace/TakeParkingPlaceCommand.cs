using MediatR;

namespace CqrsDemo.Cqrs.Handlers.Commands.TakeParkingPlace
{
    public class TakeParkingPlaceCommand : IRequest<Unit>
    {
        public string ParkingName { get; set; }

        public int PlaceNumber { get; set; }
    }
}
