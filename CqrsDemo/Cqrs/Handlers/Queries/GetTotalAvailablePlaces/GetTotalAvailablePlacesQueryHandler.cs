using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CqrsDemo.Database;
using MediatR;

namespace CqrsDemo.Handlers.Queries.GetTotalAvailablePlaces
{
    public class GetTotalAvailablePlacesQueryHandler : IRequestHandler<GetTotalAvailablePlacesQuery, GetTotalAvailablePlacesQueryResult>
    {
        private readonly MainDbContext FMainDbContext;

        public GetTotalAvailablePlacesQueryHandler(MainDbContext AMainDbContext) 
        {
            FMainDbContext = AMainDbContext;
        }

        public async Task<GetTotalAvailablePlacesQueryResult> Handle(GetTotalAvailablePlacesQuery Request, CancellationToken CancellationToken) 
        {
            var LData = await FMainDbContext.ParkingPlaces
                .Where(AParkingPlace => AParkingPlace.ParkingNameNavigation.IsOpened && AParkingPlace.IsFree)
                .ToListAsync();

            return new GetTotalAvailablePlacesQueryResult
            {
                Number = LData.Count
            };
        }
    }
}
