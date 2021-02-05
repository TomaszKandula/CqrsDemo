using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CqrsDemo.Exceptions;
using CqrsDemo.Shared.Resources;
using CqrsDemo.Infrastructure.Database;
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

        public async Task<GetParkingInfoQueryResult> Handle(GetParkingInfoQuery ARequest, CancellationToken ACancellationToken) 
        {
            var LParking = (await FMainDbContext.Parking
                .Include(AParking => AParking.ParkingPlaces)
                .ToListAsync(ACancellationToken))
                .FirstOrDefault(p => p.Name == ARequest.ParkingName);

            if (LParking == null)
                throw new BusinessException(nameof(ErrorCodes.CANNOT_FIND_PARKING), ErrorCodes.CANNOT_FIND_PARKING);

            return new GetParkingInfoQueryResult
            {
                Name = LParking.Name,
                IsOpened = LParking.IsOpened,
                MaximumPlaces = LParking.ParkingPlaces.Count,
                AvailablePlaces = LParking.IsOpened 
                    ? LParking.ParkingPlaces.Where(AParkingPlace => AParkingPlace.IsFree).Count() 
                    : 0
            };
        }
    }
}
