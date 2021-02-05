using Xunit;
using Moq;
using MockQueryable.Moq;
using FluentAssertions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CqrsDemo.UnitTests.Mock;
using CqrsDemo.Infrastructure.Database;
using CqrsDemo.Handlers.Queries.GetParkingInfo;
using CqrsDemo.Handlers.Queries.GetAllParkingInfo;
using CqrsDemo.Handlers.Queries.GetRandomAvailablePlace;
using CqrsDemo.Handlers.Queries.GetTotalAvailablePlaces;

namespace CqrsDemo.UnitTests
{
    public class HandlersTest_Queries
    {
        private readonly MainDbContext FMainDbContext;

        public HandlersTest_Queries() 
        {
            // Create mock instance
            var LMockDbContext = new Mock<MainDbContext>();

            // Upload pre-fixed dummy data
            var LParkingDbSet = DummyLoad.GetDummyParkings().AsQueryable().BuildMockDbSet();
            var LParkingPlaceDbSet = DummyLoad.GetDummyParkingPlaces().AsQueryable().BuildMockDbSet();

            // Populate database tables with dummy data
            LMockDbContext.Setup(AMainDbContext => AMainDbContext.Parking).Returns(LParkingDbSet.Object);
            LMockDbContext.Setup(AMainDbContext => AMainDbContext.ParkingPlaces).Returns(LParkingPlaceDbSet.Object);

            // Create test instance with mocked dependencies
            FMainDbContext = LMockDbContext.Object;
        }

        [Fact]
        public async Task Should_GetAllParkingInfo()
        {
            // Arrange
            var LHandleAllParkingInfo = new GetAllParkingInfoQueryHandler(FMainDbContext);
            var LRequest = new GetAllParkingInfoQuery();

            // Act
            var LResult = await LHandleAllParkingInfo.Handle(LRequest, CancellationToken.None);

            // Assert
            LResult.Should().HaveCount(2);
        }

        [Fact]
        public async Task Should_GetParkingInfo()
        {
            // Arrange
            var LHandleParkingInfo = new GetParkingInfoQueryHandler(FMainDbContext);
            var LRequest = new GetParkingInfoQuery() 
            { 
                ParkingName = "Poznan Plaza"
            };

            // Act
            var LResult = await LHandleParkingInfo.Handle(LRequest, CancellationToken.None);

            // Assert
            LResult.Name.Should().Be("Poznan Plaza");
            LResult.IsOpened.Should().BeFalse();
        }

        [Fact]
        public async Task Should_GetRandomAvailablePlace()
        {
            // Arrange
            var LHandleRandomAvailablePlace = new GetRandomAvailablePlaceQueryHandler(FMainDbContext);
            var LRequest = new GetRandomAvailablePlaceQuery();

            // Act
            var LResult = await LHandleRandomAvailablePlace.Handle(LRequest, CancellationToken.None);

            // Assert
            LResult.Number.Should().BeGreaterThan(0);
            LResult.ParkingName.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Should_GetTotalAvailablePlaces()
        {
            // Arrange
            var LHandleTotalAvailablePlaces = new GetTotalAvailablePlacesQueryHandler(FMainDbContext);
            var LRequest = new GetTotalAvailablePlacesQuery();

            // Act
            var LResult = await LHandleTotalAvailablePlaces.Handle(LRequest, CancellationToken.None);

            // Assert
            LResult.Number.Should().Be(3);
        }
    }
}
