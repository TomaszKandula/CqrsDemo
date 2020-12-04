using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CqrsDemo.Database;
using CqrsDemo.Database.Models;
using CqrsDemo.Models.Responses;
using CqrsDemo.Handlers.Queries.Models;

namespace CqrsDemo.Handlers.Queries
{

    public class ParkingQueryHandler : IParkingQueryHandler
    {

        private readonly MainDbContext FMainDbContext;

        public ParkingQueryHandler(MainDbContext AMainDbContext) 
        {
            FMainDbContext = AMainDbContext;
        }

        public async Task<IEnumerable<ParkingInfo>> Handle(GetAllParkingInfo _)
        {

            var LParkings = await FMainDbContext.Set<Parking>()
                .Include(AParking => AParking.Places)
                .ToListAsync();

            return LParkings.Select(AParking =>
            {

                return new ParkingInfo
                {
                    Name = AParking.Name,
                    IsOpened = AParking.IsOpened,
                    MaximumPlaces = AParking.Places.Count,
                    AvailablePlaces = AParking.IsOpened ? AParking.Places
                        .Where(AParkingPlace => AParkingPlace.IsFree)
                        .Count() : 0
                };

            });

        }

        public ParkingInfo Handle(GetParkingInfo AQuery)
        {

            var LParking = FMainDbContext.Set<Parking>()
                .Include(AParking => AParking.Places)
                .FirstOrDefault(p => p.Name == AQuery.ParkingName);

            if (LParking == null)
                throw new Exception($"Cannot find parking '{AQuery.ParkingName}'.");

            return new ParkingInfo
            {
                Name = LParking.Name,
                IsOpened = LParking.IsOpened,
                MaximumPlaces = LParking.Places.Count,
                AvailablePlaces =
                    LParking.IsOpened ? LParking.Places
                        .Where(AParkingPlace => AParkingPlace.IsFree)
                        .Count() : 0
            };
        }

        public ParkingPlaceInfo Handle(GetRandomAvailablePlace _)
        {

            var LRandom = new Random();

            var LParkingPlace = FMainDbContext.Set<ParkingPlace>()
                .Include(AParkingPlace => AParkingPlace.Parking)
                .Where(AParkingPlace => AParkingPlace.Parking.IsOpened && AParkingPlace.IsFree)
                .OrderBy(AParkingPlace => LRandom.Next())
                .FirstOrDefault();

            return new ParkingPlaceInfo
            {
                ParkingName = LParkingPlace.ParkingName,
                Number = LParkingPlace.Number
            };
        }

        public int Handle(GetTotalAvailablePlaces _)
        {
            return FMainDbContext.Set<ParkingPlace>()
                .Where(AParkingPlace => AParkingPlace.Parking.IsOpened && AParkingPlace.IsFree)
                .Count();
        }


    }

}
