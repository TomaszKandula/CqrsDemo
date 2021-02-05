using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CqrsDemo.Exceptions;
using CqrsDemo.Shared.Resources;
using CqrsDemo.Infrastructure.Database;
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

        public async Task<GetRandomAvailablePlaceQueryResult> Handle(GetRandomAvailablePlaceQuery ARequest, CancellationToken ACancellationToken) 
        {
            var LRandom = new Random();
            var LRandomNext = LRandom.Next();

            var LParkingPlace = await FMainDbContext.ParkingPlaces
                .Include(AParkingPlace => AParkingPlace.ParkingNameNavigation.ParkingPlaces)
                .Where(AParkingPlace => AParkingPlace.ParkingNameNavigation.IsOpened && AParkingPlace.IsFree)
                .ToListAsync(ACancellationToken);

            var LRandomParkingPlace = LParkingPlace.OrderBy(AParkingPlace => LRandom.Next());

            if (!LRandomParkingPlace.Any()) 
            {
                throw new BusinessException(nameof(ErrorCodes.ERROR_UNEXPECTED), ErrorCodes.ERROR_UNEXPECTED);
            }

            return new GetRandomAvailablePlaceQueryResult
            {
                ParkingName = LParkingPlace.First().ParkingName,
                Number = LParkingPlace.First().Number
            };
        }
    }
}
