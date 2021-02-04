using CqrsDemo.Models.Responses;
using MediatR;

namespace CqrsDemo.Handlers.Commands.Models
{
    public class OpenParking : IRequest<CommandResponse>
    {
        public string ParkingName { get; set; }
    }
}
