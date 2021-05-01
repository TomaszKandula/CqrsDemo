using System.Collections.Generic;
using MediatR;

namespace CqrsDemo.Cqrs.Handlers.Queries.GetAllParkingInfo
{
    public class GetAllParkingInfoQuery : IRequest<IEnumerable<GetAllParkingInfoQueryResult>> { }
}
