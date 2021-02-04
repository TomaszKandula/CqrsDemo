using MediatR;

namespace CqrsDemo.Handlers.Commands.CreateParking
{
    public class CreateParkingCommand : IRequest<Unit>
    {
        public string ParkingName { get; set; }

        public int Capacity { get; set; }
    }
}
