using CqrsDemo.Models.Responses;
using MediatR;

namespace CqrsDemo.Handlers.Queries.Models
{

    public class GetRandomAvailablePlace : IRequest<ParkingPlaceInfo>
    {
    }

}
