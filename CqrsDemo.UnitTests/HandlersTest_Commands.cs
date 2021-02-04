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
using CqrsDemo.UnitTests.Services;
using CqrsDemo.Handlers.Commands.Models;

namespace CqrsDemo.UnitTests
{
    public class HandlersTest_Commands
    {
        private readonly Mock<MainDbContext> LMockDbContext;
        private readonly FakeAuthentication FAuthentication;
        private readonly FakeCommands FCommands;

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

            // Create fake services
            FAuthentication = new FakeAuthentication();
            FCommands = new FakeCommands();
        }

        [Fact]
        public async Task Should_CreateParking()
        {
            // Arrange
            var LHandleCreateParking = new CreateParkingCommandHandler(LMockDbContext.Object, FCommands);
            var LCommand = new CreateParkingCommand
            {
                ParkingName = "Best Parking",
                Capacity = 1000
            };

            // Act
            var LResult = await LHandleCreateParking.Handle(LCommand, CancellationToken.None);

            // Assert
            LMockDbContext.Verify(AMockDbContext => AMockDbContext.SaveChangesAsync(CancellationToken.None), Times.Once);
            LResult.IsSucceeded.Should().BeTrue();
        }

        [Fact]
        public async Task Should_OpenParking()
        {
            // Arrange
            var LHandleOpenParking = new OpenParkingCommandHandler(LMockDbContext.Object, FCommands);
            var LCommand = new OpenParkingCommand
            {
                ParkingName = "Poznan Plaza"
            };

            // Act
            var LResult = await LHandleOpenParking.Handle(LCommand, CancellationToken.None);

            // Assert
            LMockDbContext.Verify(AMockDbContext => AMockDbContext.SaveChangesAsync(CancellationToken.None), Times.Once);
            LResult.IsSucceeded.Should().BeTrue();
        }

        [Fact]
        public async Task Should_CloseParking()
        {
            // Arrange
            var LHandleCloseParking = new CloseParkingCommandHandler(LMockDbContext.Object, FCommands);
            var LCommand = new CloseParkingCommand
            {
                ParkingName = "Parking-786359"
            };

            // Act
            var LResult = await LHandleCloseParking.Handle(LCommand, CancellationToken.None);

            // Assert
            LMockDbContext.Verify(AMockDbContext => AMockDbContext.SaveChangesAsync(CancellationToken.None), Times.Once);
            LResult.IsSucceeded.Should().BeTrue();
        }

        [Theory]
        [InlineData("Parking-786359", 3)]
        public async Task Should_TakeParkingPlace(string ParkingName, int PlaceNumber)
        {
            // Arrange
            var LHandleTakeParkingPlace = new TakeParkingPlaceCommandHandler(LMockDbContext.Object, FCommands, FAuthentication);
            var LCommand = new TakeParkingPlaceCommand
            {
                ParkingName = ParkingName,
                PlaceNumber = PlaceNumber
            };

            // Act
            var LResult = await LHandleTakeParkingPlace.Handle(LCommand, CancellationToken.None);

            // Assert
            LMockDbContext.Verify(AMockDbContext => AMockDbContext.SaveChangesAsync(CancellationToken.None), Times.Once);
            LResult.IsSucceeded.Should().BeTrue();
        }

        [Theory]
        [InlineData("Parking-786359", 4)]
        public async Task Should_LeaveParkingPlace(string ParkingName, int PlaceNumber)
        {
            // Arrange
            var LHandleLeaveParkingPlace = new LeaveParkingPlaceCommandHandler(LMockDbContext.Object, FCommands);
            var LCommand = new LeaveParkingPlaceCommand
            {
                ParkingName = ParkingName,
                PlaceNumber = PlaceNumber
            };

            // Act
            var LResult = await LHandleLeaveParkingPlace.Handle(LCommand, CancellationToken.None);

            // Assert
            LMockDbContext.Verify(AMockDbContext => AMockDbContext.SaveChangesAsync(CancellationToken.None), Times.Once);
            LResult.IsSucceeded.Should().BeTrue();
        }
    }
}
