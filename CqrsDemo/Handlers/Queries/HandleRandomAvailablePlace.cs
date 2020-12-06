using System;
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

    public class HandleRandomAvailablePlace : IRequestHandler<GetRandomAvailablePlace, ParkingPlaceInfo>
    {

        private readonly MainDbContext FMainDbContext;

        public HandleRandomAvailablePlace(MainDbContext AMainDbContext) 
        {
            FMainDbContext = AMainDbContext;
        }

        public async Task<ParkingPlaceInfo> Handle(GetRandomAvailablePlace Request, CancellationToken CancellationToken) 
        {

            var LRandom = new Random();

            var LParkingPlace = (await FMainDbContext.ParkingPlaces
                .Include(AParkingPlace => AParkingPlace.ParkingNameNavigation.ParkingPlaces)
                .Where(AParkingPlace => AParkingPlace.ParkingNameNavigation.IsOpened && AParkingPlace.IsFree)
                .OrderBy(AParkingPlace => LRandom.Next())
                .ToListAsync()
                ).FirstOrDefault();

            return new ParkingPlaceInfo
            {
                ParkingName = LParkingPlace.ParkingName,
                Number = LParkingPlace.Number
            };

        }

    }

}
