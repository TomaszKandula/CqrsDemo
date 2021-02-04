using MediatR;

namespace CqrsDemo.Handlers.Queries.GetTotalAvailablePlaces
{
    public class GetTotalAvailablePlacesQuery : IRequest<GetTotalAvailablePlacesQueryResult>
    {
    }
}
