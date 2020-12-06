using CqrsDemo.Models.Responses;
using MediatR;

namespace CqrsDemo.Handlers.Queries.Models
{

    public class GetTotalAvailablePlaces : IRequest<AvailablePlaceInfo>
    {
    }

}
