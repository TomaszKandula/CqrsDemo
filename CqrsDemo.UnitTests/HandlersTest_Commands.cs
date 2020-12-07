using Xunit;
using Moq;
using MockQueryable.Moq;
using FluentAssertions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CqrsDemo.Database;
using CqrsDemo.UnitTests.Mock;
using CqrsDemo.Handlers.Commands;
using CqrsDemo.Services.Commands;
using CqrsDemo.Services.Authentication;
using CqrsDemo.Handlers.Commands.Models;

namespace CqrsDemo.UnitTests
{

    public class HandlersTest_Commands
    {

        private readonly Mock<MainDbContext> LMockDbContext;

        public HandlersTest_Commands() 
        {

            // Create mock instances
            LMockDbContext = new Mock<MainDbContext>();

            // Upload pre-fixed dummy data
            var LCommandDbSet = DummyLoad.GetDummyCommands().AsQueryable().BuildMockDbSet();
            var LParkingDbSet = DummyLoad.GetDummyParkings().AsQueryable().BuildMockDbSet();
            var LParkingPlaceDbSet = DummyLoad.GetDummyParkingPlaces().AsQueryable().BuildMockDbSet();

            // Populate database tables with dummy data
            LMockDbContext.Setup(AMainDbContext => AMainDbContext.CommandStore).Returns(LCommandDbSet.Object);
            LMockDbContext.Setup(AMainDbContext => AMainDbContext.Parking).Returns(LParkingDbSet.Object);
            LMockDbContext.Setup(AMainDbContext => AMainDbContext.ParkingPlaces).Returns(LParkingPlaceDbSet.Object);

        }

        [Fact]
        public async Task Should_CreateParking()
        {

            // Arrange
            var LAuthentication = new Authentication();
            var LCommands = new Commands(LAuthentication, LMockDbContext.Object);
            var LHandleCreateParking = new HandleCreateParking(LMockDbContext.Object, LCommands);
            var LCommand = new CreateParking
            {
                ParkingName = "Best Parking",
                Capacity = 1000
            };

            // Act
            var LResult = await LHandleCreateParking.Handle(LCommand, CancellationToken.None);

            // Assert
            LMockDbContext.Verify(AMockDbContext => AMockDbContext.SaveChangesAsync(CancellationToken.None), Times.Exactly(2));
            LResult.IsSucceeded.Should().BeTrue();

        }

        [Fact]
        public async Task Should_OpenParking()
        {

            // Arrange
            var LAuthentication = new Authentication();
            var LCommands = new Commands(LAuthentication, LMockDbContext.Object);
            var LHandleOpenParking = new HandleOpenParking(LMockDbContext.Object, LCommands);
            var LCommand = new OpenParking
            {
                ParkingName = "Poznan Plaza"
            };

            // Act
            var LResult = await LHandleOpenParking.Handle(LCommand, CancellationToken.None);

            // Assert
            LMockDbContext.Verify(AMockDbContext => AMockDbContext.SaveChangesAsync(CancellationToken.None), Times.Exactly(2));
            LResult.IsSucceeded.Should().BeTrue();

        }

        [Fact]
        public async Task Should_CloseParking()
        {

            // Arrange
            var LAuthentication = new Authentication();
            var LCommands = new Commands(LAuthentication, LMockDbContext.Object);
            var LHandleCloseParking = new HandleCloseParking(LMockDbContext.Object, LCommands);
            var LCommand = new CloseParking
            {
                ParkingName = "Parking-786359"
            };

            // Act
            var LResult = await LHandleCloseParking.Handle(LCommand, CancellationToken.None);

            // Assert
            LMockDbContext.Verify(AMockDbContext => AMockDbContext.SaveChangesAsync(CancellationToken.None), Times.Exactly(2));
            LResult.IsSucceeded.Should().BeTrue();

        }

        [Theory]
        [InlineData("Parking-786359", 3)]
        public async Task Should_TakeParkingPlace(string ParkingName, int PlaceNumber)
        {

            // Arrange
            var LAuthentication = new Authentication();
            var LCommands = new Commands(LAuthentication, LMockDbContext.Object);
            var LHandleTakeParkingPlace = new HandleTakeParkingPlace(LMockDbContext.Object, LCommands, LAuthentication);
            var LCommand = new TakeParkingPlace
            {
                ParkingName = ParkingName,
                PlaceNumber = PlaceNumber
            };

            // Act
            var LResult = await LHandleTakeParkingPlace.Handle(LCommand, CancellationToken.None);

            // Assert
            LMockDbContext.Verify(AMockDbContext => AMockDbContext.SaveChangesAsync(CancellationToken.None), Times.Exactly(2));
            LResult.IsSucceeded.Should().BeTrue();

        }

        [Theory]
        [InlineData("Parking-786359", 4)]
        public async Task Should_LeaveParkingPlace(string ParkingName, int PlaceNumber)
        {

            // Arrange
            var LAuthentication = new Authentication();
            var LCommands = new Commands(LAuthentication, LMockDbContext.Object);
            var LHandleLeaveParkingPlace = new HandleLeaveParkingPlace(LMockDbContext.Object, LCommands);
            var LCommand = new LeaveParkingPlace
            {
                ParkingName = ParkingName,
                PlaceNumber = PlaceNumber
            };

            // Act
            var LResult = await LHandleLeaveParkingPlace.Handle(LCommand, CancellationToken.None);

            // Assert
            LMockDbContext.Verify(AMockDbContext => AMockDbContext.SaveChangesAsync(CancellationToken.None), Times.Exactly(2));
            LResult.IsSucceeded.Should().BeTrue();

        }

    }

}
