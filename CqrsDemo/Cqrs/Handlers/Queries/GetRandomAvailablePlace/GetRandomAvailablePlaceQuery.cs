using MediatR;

namespace CqrsDemo.Cqrs.Handlers.Queries.GetRandomAvailablePlace
{
    public class GetRandomAvailablePlaceQuery : IRequest<GetRandomAvailablePlaceQueryResult> { }
}
