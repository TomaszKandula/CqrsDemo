using System.Collections.Generic;
using CqrsDemo.Models.Responses;
using MediatR;

namespace CqrsDemo.Handlers.Queries.Models
{
    public class GetAllParkingInfo : IRequest<IEnumerable<ParkingInfo>>
    {
    }
}
