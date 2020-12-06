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

    public class HandleParkingInfo : IRequestHandler<GetParkingInfo, ParkingInfo>
    {

        private readonly MainDbContext FMainDbContext;

        public HandleParkingInfo(MainDbContext AMainDbContext) 
        {
            FMainDbContext = AMainDbContext;
        }

        public async Task<ParkingInfo> Handle(GetParkingInfo Request, CancellationToken CancellationToken) 
        {

            var LParking = (await FMainDbContext.Parking
                .Include(AParking => AParking.ParkingPlaces)
                .ToListAsync()
                ).FirstOrDefault(p => p.Name == Request.ParkingName);

            //if (LParking == null)
            //    throw new Exception($"Cannot find parking '{AQuery.ParkingName}'.");

            return new ParkingInfo
            {
                Name = LParking.Name,
                IsOpened = LParking.IsOpened,
                MaximumPlaces = LParking.ParkingPlaces.Count,
                AvailablePlaces =
                    LParking.IsOpened ? LParking.ParkingPlaces
                        .Where(AParkingPlace => AParkingPlace.IsFree)
                        .Count() : 0
            };

        }

    }

}
