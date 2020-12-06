using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CqrsDemo.Database;
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

        public ParkingInfo Handle(GetParkingInfo AQuery)
        {

            var LParking = FMainDbContext.Parking
                .Include(AParking => AParking.ParkingPlaces)
                .FirstOrDefault(p => p.Name == AQuery.ParkingName);

            if (LParking == null)
                throw new Exception($"Cannot find parking '{AQuery.ParkingName}'.");

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

        public ParkingPlaceInfo Handle(GetRandomAvailablePlace _)
        {

            var LRandom = new Random();

            var LParkingPlace = FMainDbContext.ParkingPlaces
                .Include(AParkingPlace => AParkingPlace.ParkingNameNavigation.ParkingPlaces)
                .Where(AParkingPlace => AParkingPlace.ParkingNameNavigation.IsOpened && AParkingPlace.IsFree)
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
            return FMainDbContext.ParkingPlaces
                .Where(AParkingPlace => AParkingPlace.ParkingNameNavigation.IsOpened && AParkingPlace.IsFree)
                .Count();
        }


    }

}
