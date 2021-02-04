using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CqrsDemo.Database;
using MediatR;

namespace CqrsDemo.Handlers.Queries.GetRandomAvailablePlace
{
    public class GetRandomAvailablePlaceQueryHandler : IRequestHandler<GetRandomAvailablePlaceQuery, GetRandomAvailablePlaceQueryResult>
    {
        private readonly MainDbContext FMainDbContext;

        public GetRandomAvailablePlaceQueryHandler(MainDbContext AMainDbContext) 
        {
            FMainDbContext = AMainDbContext;
        }

        public async Task<GetRandomAvailablePlaceQueryResult> Handle(GetRandomAvailablePlaceQuery Request, CancellationToken CancellationToken) 
        {
            var LRandom = new Random();

            var LParkingPlace = (await FMainDbContext.ParkingPlaces
                .Include(AParkingPlace => AParkingPlace.ParkingNameNavigation.ParkingPlaces)
                .Where(AParkingPlace => AParkingPlace.ParkingNameNavigation.IsOpened && AParkingPlace.IsFree)
                .OrderBy(AParkingPlace => LRandom.Next())
                .ToListAsync())
                .FirstOrDefault();

            return new GetRandomAvailablePlaceQueryResult
            {
                ParkingName = LParkingPlace.ParkingName,
                Number = LParkingPlace.Number
            };
        }
    }
}
