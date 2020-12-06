using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CqrsDemo.Database;
using CqrsDemo.Models.Responses;
using CqrsDemo.Handlers.Queries.Models;
using MediatR;

namespace CqrsDemo.Handlers.Queries
{

    public class HandleAllParkingInfo : IRequestHandler<GetAllParkingInfo, IEnumerable<ParkingInfo>>
    {

        private readonly MainDbContext FMainDbContext;

        public HandleAllParkingInfo(MainDbContext AMainDbContext) 
        {
            FMainDbContext = AMainDbContext;
        }

        public async Task<IEnumerable<ParkingInfo>> Handle(GetAllParkingInfo Request, CancellationToken CancellationToken) 
        {

            var LParkings = await FMainDbContext.Parking
                .Include(AParking => AParking.ParkingPlaces)
                .ToListAsync();

            return LParkings.Select(AParking =>
            {

                return new ParkingInfo
                {
                    Name = AParking.Name,
                    IsOpened = AParking.IsOpened,
                    MaximumPlaces = AParking.ParkingPlaces.Count,
                    AvailablePlaces = AParking.IsOpened ? AParking.ParkingPlaces
                        .Where(AParkingPlace => AParkingPlace.IsFree)
                        .Count() : 0
                };

            });

        }

    }

}
