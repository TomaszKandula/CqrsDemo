using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CqrsDemo.Infrastructure.Database;
using MediatR;

namespace CqrsDemo.Handlers.Queries.GetAllParkingInfo
{
    public class GetAllParkingInfoQueryHandler : IRequestHandler<GetAllParkingInfoQuery, IEnumerable<GetAllParkingInfoQueryResult>>
    {
        private readonly MainDbContext FMainDbContext;

        public GetAllParkingInfoQueryHandler(MainDbContext AMainDbContext) 
        {
            FMainDbContext = AMainDbContext;
        }

        public async Task<IEnumerable<GetAllParkingInfoQueryResult>> Handle(GetAllParkingInfoQuery ARequest, CancellationToken ACancellationToken) 
        {
            var LParkings = await FMainDbContext.Parking
                .Include(AParking => AParking.ParkingPlaces)
                .ToListAsync(ACancellationToken);

            var LSelection = LParkings.Select(AParking => new GetAllParkingInfoQueryResult
            {
                Name = AParking.Name,
                IsOpened = AParking.IsOpened,
                MaximumPlaces = AParking.ParkingPlaces.Count,
                AvailablePlaces = AParking.IsOpened
                    ? AParking.ParkingPlaces.Where(AParkingPlace => AParkingPlace.IsFree).Count()
                    : 0
            });

            return LSelection;
        }
    }
}
