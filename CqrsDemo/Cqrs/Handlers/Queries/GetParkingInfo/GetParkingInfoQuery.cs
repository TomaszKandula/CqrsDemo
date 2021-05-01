using MediatR;

namespace CqrsDemo.Cqrs.Handlers.Queries.GetParkingInfo
{
    public class GetParkingInfoQuery : IRequest<GetParkingInfoQueryResult>
    {
        public string ParkingName { get; set; }
    }
}
