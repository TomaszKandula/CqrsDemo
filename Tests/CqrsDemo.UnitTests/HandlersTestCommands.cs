using Xunit;
using Moq;
using MockQueryable.Moq;
using FluentAssertions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CqrsDemo.UnitTests.Database;
using CqrsDemo.UnitTests.Services;
using CqrsDemo.Infrastructure.Database;
using CqrsDemo.Cqrs.Handlers.Commands.CreateParking;
using CqrsDemo.Cqrs.Handlers.Commands.OpenParking;
using CqrsDemo.Cqrs.Handlers.Commands.CloseParking;
using CqrsDemo.Cqrs.Handlers.Commands.TakeParkingPlace;
using CqrsDemo.Cqrs.Handlers.Commands.LeaveParkingPlace;

namespace CqrsDemo.UnitTests
{
    public class HandlersTestCommands
    {
        private readonly Mock<MainDbContext> FMockDbContext;
        private readonly FakeAuthentication FAuthentication;
        private readonly FakeCommands FCommands;

        public HandlersTestCommands() 
        {
            // Create mock instances
            FMockDbContext = new Mock<MainDbContext>();

            // Upload pre-fixed dummy data
            var LCommandDbSet = DummyLoad.GetDummyCommands().AsQueryable().BuildMockDbSet();
            var LParkingDbSet = DummyLoad.GetDummyParkingList().AsQueryable().BuildMockDbSet();
            var LParkingPlaceDbSet = DummyLoad.GetDummyParkingPlaces().AsQueryable().BuildMockDbSet();

            // Populate database tables with dummy data
            FMockDbContext.Setup(AMainDbContext => AMainDbContext.CommandStore).Returns(LCommandDbSet.Object);
            FMockDbContext.Setup(AMainDbContext => AMainDbContext.Parking).Returns(LParkingDbSet.Object);
            FMockDbContext.Setup(AMainDbContext => AMainDbContext.ParkingPlaces).Returns(LParkingPlaceDbSet.Object);

            // Create fake services
            FAuthentication = new FakeAuthentication();
            FCommands = new FakeCommands();
        }

        [Fact]
        public async Task Should_CreateParking()
        {
            // Arrange
            var LHandleCreateParking = new CreateParkingCommandHandler(FMockDbContext.Object, FCommands);
            var LCommand = new CreateParkingCommand
            {
                ParkingName = "Best Parking",
                Capacity = 1000
            };

            // Act
            var LResult = await LHandleCreateParking.Handle(LCommand, CancellationToken.None);

            // Assert
            FMockDbContext
                .Verify(AMockDbContext => AMockDbContext.SaveChangesAsync(CancellationToken.None), Times.Once);
            
            LResult.ToString().Should().Be("()");
        }

        [Fact]
        public async Task Should_OpenParking()
        {
            // Arrange
            var LHandleOpenParking = new OpenParkingCommandHandler(FMockDbContext.Object, FCommands);
            var LCommand = new OpenParkingCommand
            {
                ParkingName = "Poznan Plaza"
            };

            // Act
            var LResult = await LHandleOpenParking.Handle(LCommand, CancellationToken.None);

            // Assert
            FMockDbContext
                .Verify(AMockDbContext => AMockDbContext.SaveChangesAsync(CancellationToken.None), Times.Once);
            
            LResult.ToString().Should().Be("()");
        }

        [Fact]
        public async Task Should_CloseParking()
        {
            // Arrange
            var LHandleCloseParking = new CloseParkingCommandHandler(FMockDbContext.Object, FCommands);
            var LCommand = new CloseParkingCommand
            {
                ParkingName = "Parking-786359"
            };

            // Act
            var LResult = await LHandleCloseParking.Handle(LCommand, CancellationToken.None);

            // Assert
            FMockDbContext
                .Verify(AMockDbContext => AMockDbContext.SaveChangesAsync(CancellationToken.None), Times.Once);
            
            LResult.ToString().Should().Be("()");
        }

        [Theory]
        [InlineData("Parking-786359", 3)]
        public async Task Should_TakeParkingPlace(string AParkingName, int APlaceNumber)
        {
            // Arrange
            var LHandleTakeParkingPlace = new TakeParkingPlaceCommandHandler(FMockDbContext.Object, FCommands, FAuthentication);
            var LCommand = new TakeParkingPlaceCommand
            {
                ParkingName = AParkingName,
                PlaceNumber = APlaceNumber
            };

            // Act
            var LResult = await LHandleTakeParkingPlace.Handle(LCommand, CancellationToken.None);

            // Assert
            FMockDbContext
                .Verify(AMockDbContext => AMockDbContext.SaveChangesAsync(CancellationToken.None), Times.Once);
            
            LResult.ToString().Should().Be("()");
        }

        [Theory]
        [InlineData("Parking-786359", 4)]
        public async Task Should_LeaveParkingPlace(string AParkingName, int APlaceNumber)
        {
            // Arrange
            var LHandleLeaveParkingPlace = new LeaveParkingPlaceCommandHandler(FMockDbContext.Object, FCommands);
            var LCommand = new LeaveParkingPlaceCommand
            {
                ParkingName = AParkingName,
                PlaceNumber = APlaceNumber
            };

            // Act
            var LResult = await LHandleLeaveParkingPlace.Handle(LCommand, CancellationToken.None);

            // Assert
            FMockDbContext
                .Verify(AMockDbContext => AMockDbContext.SaveChangesAsync(CancellationToken.None), Times.Once);
            
            LResult.ToString().Should().Be("()");
        }
    }
}
