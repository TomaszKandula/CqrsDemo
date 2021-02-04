using System.Collections.Generic;
using MediatR;

namespace CqrsDemo.Handlers.Queries.GetAllParkingInfo
{
    public class GetAllParkingInfoQuery : IRequest<IEnumerable<GetAllParkingInfoQueryResult>>
    {
    }
}
