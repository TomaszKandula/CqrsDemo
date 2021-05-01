using MediatR;

namespace CqrsDemo.Cqrs.Handlers.Queries.GetTotalAvailablePlaces
{
    public class GetTotalAvailablePlacesQuery : IRequest<GetTotalAvailablePlacesQueryResult> { }
}
