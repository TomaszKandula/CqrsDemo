using CqrsDemo.Models.Responses;
using MediatR;

namespace CqrsDemo.Handlers.Commands.Models
{
    public class CloseParking : IRequest<CommandResponse>
    {
        public string ParkingName { get; set; }
    }
}
