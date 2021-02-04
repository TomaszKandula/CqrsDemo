using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CqrsDemo.Database;
using MediatR;

namespace CqrsDemo.Handlers.Queries.GetParkingInfo
{
    public class GetParkingInfoQueryHandler : IRequestHandler<GetParkingInfoQuery, GetParkingInfoQueryResult>
    {
        private readonly MainDbContext FMainDbContext;

        public GetParkingInfoQueryHandler(MainDbContext AMainDbContext) 
        {
            FMainDbContext = AMainDbContext;
        }

        public async Task<GetParkingInfoQueryResult> Handle(GetParkingInfoQuery Request, CancellationToken CancellationToken) 
        {
            var LParking = (await FMainDbContext.Parking
                .Include(AParking => AParking.ParkingPlaces)
                .ToListAsync())
                .FirstOrDefault(p => p.Name == Request.ParkingName);

            //if (LParking == null)
            //    throw new Exception($"Cannot find parking '{AQuery.ParkingName}'.");

            return new GetParkingInfoQueryResult
            {
                Name = LParking.Name,
                IsOpened = LParking.IsOpened,
                MaximumPlaces = LParking.ParkingPlaces.Count,
                AvailablePlaces = LParking.IsOpened 
                    ? LParking.ParkingPlaces
                        .Where(AParkingPlace => AParkingPlace.IsFree)
                        .Count() 
                    : 0
            };
        }
    }
}
