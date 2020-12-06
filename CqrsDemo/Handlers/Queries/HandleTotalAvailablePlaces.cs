using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CqrsDemo.Database;
using CqrsDemo.Models.Responses;
using CqrsDemo.Handlers.Queries.Models;
using MediatR;

namespace CqrsDemo.Handlers.Queries
{

    public class HandleTotalAvailablePlaces : IRequestHandler<GetTotalAvailablePlaces, AvailablePlaceInfo>
    {

        private readonly MainDbContext FMainDbContext;

        public HandleTotalAvailablePlaces(MainDbContext AMainDbContext) 
        {
            FMainDbContext = AMainDbContext;
        }

        public async Task<AvailablePlaceInfo> Handle(GetTotalAvailablePlaces Request, CancellationToken CancellationToken) 
        {

            var LData = await FMainDbContext.ParkingPlaces
                .Where(AParkingPlace => AParkingPlace.ParkingNameNavigation.IsOpened && AParkingPlace.IsFree)
                .ToListAsync();

            return new AvailablePlaceInfo
            {
                Number = LData.Count
            };

        }

    }

}
