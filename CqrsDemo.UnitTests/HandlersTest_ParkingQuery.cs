using Xunit;
using Moq;
using MockQueryable.Moq;
using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using CqrsDemo.Database;
using CqrsDemo.UnitTests.Mock;
using CqrsDemo.Handlers.Queries;
using CqrsDemo.Handlers.Queries.Models;

namespace CqrsDemo.UnitTests
{

    public class HandlersTest_ParkingQuery
    {

        private readonly IParkingQueryHandler FParkingQueryHandler;

        public HandlersTest_ParkingQuery() 
        {

            // Create instances to mocked all dependencies        
            var LMockDbContext = new Mock<MainDbContext>();

            // Upload pre-fixed dummy data
            var LParkingDbSet = DummyLoad.GetDummyParkings().AsQueryable().BuildMockDbSet();
            var LParkingPlaceDbSet = DummyLoad.GetDummyParkingPlaces().AsQueryable().BuildMockDbSet();

            // Populate database tables with dummy data
            LMockDbContext.Setup(AMainDbContext => AMainDbContext.Parking).Returns(LParkingDbSet.Object);
            LMockDbContext.Setup(AMainDbContext => AMainDbContext.ParkingPlaces).Returns(LParkingPlaceDbSet.Object);

            // Create test instance with mocked dependencies
            FParkingQueryHandler = new ParkingQueryHandler(LMockDbContext.Object);

        }

        [Fact]
        public async Task Should_GetAllParkingInfo()
        {

            // Arrange
            var LQuery = new GetAllParkingInfo();

            // Act
            var LResult = await FParkingQueryHandler.Handle(LQuery);

            // Assert
            LResult.Should().HaveCount(2);

        }

        [Fact]
        public void Should_GetParkingInfo()
        {

            // Arrange
            var LQuery = new GetParkingInfo
            {
                ParkingName = "Parking-786359"
            };

            // Act
            var LResult = FParkingQueryHandler.Handle(LQuery);

            // Assert
            LResult.IsOpened.Should().BeTrue();

        }

        [Fact]
        public void Should_GetRandomAvailablePlace()
        {

            // Arrange
            var LQuery = new GetRandomAvailablePlace();

            // Act
            var LResult = FParkingQueryHandler.Handle(LQuery);

            // Assert
            LResult.Number.Should().NotBe(0);
            LResult.ParkingName.Should().NotBeNullOrEmpty();

        }

        [Fact]
        public void Should_GetTotalAvailablePlaces()
        {

            // Arrange
            var LQuery = new GetTotalAvailablePlaces();

            // Act
            var LResult = FParkingQueryHandler.Handle(LQuery);

            // Assert
            LResult.Should().Be(3);

        }

    }

}
