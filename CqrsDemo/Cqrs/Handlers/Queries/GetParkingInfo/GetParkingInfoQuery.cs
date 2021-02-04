using MediatR;

namespace CqrsDemo.Handlers.Queries.GetParkingInfo
{
    public class GetParkingInfoQuery : IRequest<GetParkingInfoQueryResult>
    {
        public string ParkingName { get; set; }
    }
}
