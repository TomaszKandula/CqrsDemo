using MediatR;

namespace CqrsDemo.Handlers.Commands.LeaveParkingPlace
{
    public class LeaveParkingPlaceCommand : IRequest<Unit>
    {
        public string ParkingName { get; set; }

        public int PlaceNumber { get; set; }
    }
}
