using Xunit;
using Moq;
using MockQueryable.Moq;
using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using CqrsDemo.UnitTests.Mock;
using CqrsDemo.Handlers.Queries;
using CqrsDemo.Handlers.Queries.Models;
using CqrsDemo.UnitTests.Database;

namespace CqrsDemo.UnitTests
{

    public class HandlersTest_ParkingQuery
    {

        private readonly IParkingQueryHandler FParkingQueryHandler;

        public HandlersTest_ParkingQuery() 
        {

            // Create instances to mocked all dependencies        
            var LMockDbContext = new Mock<FakeDbContext>();

            // Upload pre-fixed dummy data
            var LCommandDbSet = DummyLoad.GetDummyCommands().AsQueryable().BuildMockDbSet();
            var LParkingDbSet = DummyLoad.GetDummyParkings().AsQueryable().BuildMockDbSet();
            var LParkingPlaceDbSet = DummyLoad.GetDummyParkingPlaces().AsQueryable().BuildMockDbSet();

            // Populate database tables with dummy data
            LMockDbContext.Setup(AMainDbContext => AMainDbContext.CommandStore).Returns(LCommandDbSet.Object);
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

            // Act

            // Assert

        }

        [Fact]
        public void Should_GetRandomAvailablePlace()
        {

            // Arrange

            // Act

            // Assert

        }

        [Fact]
        public void Should_GetTotalAvailablePlaces()
        {

            // Arrange

            // Act

            // Assert

        }

    }

}