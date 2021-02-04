using MediatR;

namespace CqrsDemo.Handlers.Queries.GetRandomAvailablePlace
{
    public class GetRandomAvailablePlaceQuery : IRequest<GetRandomAvailablePlaceQueryResult>
    {
    }
}
