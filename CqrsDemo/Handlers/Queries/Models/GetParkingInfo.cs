using CqrsDemo.Models.Responses;
using MediatR;

namespace CqrsDemo.Handlers.Queries.Models
{

    public class GetParkingInfo : IRequest<ParkingInfo>
    {
        public string ParkingName { get; set; }
    }

}
