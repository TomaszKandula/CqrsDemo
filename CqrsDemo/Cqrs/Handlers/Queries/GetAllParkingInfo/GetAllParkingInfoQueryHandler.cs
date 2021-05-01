using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CqrsDemo.Infrastructure.Database;
using MediatR;

namespace CqrsDemo.Cqrs.Handlers.Queries.GetAllParkingInfo
{
    public class GetAllParkingInfoQueryHandler : IRequestHandler<GetAllParkingInfoQuery, IEnumerable<GetAllParkingInfoQueryResult>>
    {
        private readonly MainDbContext FMainDbContext;

        public GetAllParkingInfoQueryHandler(MainDbContext AMainDbContext) 
            => FMainDbContext = AMainDbContext;

        public async Task<IEnumerable<GetAllParkingInfoQueryResult>> Handle(GetAllParkingInfoQuery ARequest, CancellationToken ACancellationToken) 
        {
            var LParkingList = await FMainDbContext.Parking
                .Include(AParking => AParking.ParkingPlaces)
                .ToListAsync(ACancellationToken);

            var LSelection = LParkingList.Select(AParking => new GetAllParkingInfoQueryResult
            {
                Name = AParking.Name,
                IsOpened = AParking.IsOpened,
                MaximumPlaces = AParking.ParkingPlaces.Count,
                AvailablePlaces = AParking.IsOpened
                    ? AParking.ParkingPlaces.Count(AParkingPlace => AParkingPlace.IsFree)
                    : 0
            });

            return LSelection;
        }
    }
}
