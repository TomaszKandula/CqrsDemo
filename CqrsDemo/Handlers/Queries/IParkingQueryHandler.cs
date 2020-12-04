using System.Threading.Tasks;
using System.Collections.Generic;
using CqrsDemo.Models.Responses;
using CqrsDemo.Handlers.Queries.Models;

namespace CqrsDemo.Handlers.Queries
{

    public interface IParkingQueryHandler
    {
        Task<IEnumerable<ParkingInfo>> Handle(GetAllParkingInfo _);
        ParkingInfo Handle(GetParkingInfo AQuery);
        ParkingPlaceInfo Handle(GetRandomAvailablePlace _);
        int Handle(GetTotalAvailablePlaces _);
    }

}
