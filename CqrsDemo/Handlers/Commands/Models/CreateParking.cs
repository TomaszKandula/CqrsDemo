using CqrsDemo.Models.Responses;
using MediatR;

namespace CqrsDemo.Handlers.Commands.Models
{
    public class CreateParking : IRequest<CommandResponse>
    {
        public string ParkingName { get; set; }

        public int Capacity { get; set; }
    }
}
