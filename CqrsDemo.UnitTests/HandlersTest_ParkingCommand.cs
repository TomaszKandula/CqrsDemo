using Xunit;
using Moq;
using MockQueryable.Moq;
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

    public class HandlersTest_ParkingCommand
    {

        private readonly Mock<MainDbContext> LMockDbContext;
        private readonly IParkingCommandHandler FParkingCommandHandler;

        public HandlersTest_ParkingCommand() 
        {

            // Create instances to mocked all dependencies        
            LMockDbContext = new Mock<MainDbContext>();

            // Upload pre-fixed dummy data
            var LCommandDbSet = DummyLoad.GetDummyCommands().AsQueryable().BuildMockDbSet();
            var LParkingDbSet = DummyLoad.GetDummyParkings().AsQueryable().BuildMockDbSet();
            var LParkingPlaceDbSet = DummyLoad.GetDummyParkingPlaces().AsQueryable().BuildMockDbSet();

            // Populate database tables with dummy data
            LMockDbContext.Setup(AMainDbContext => AMainDbContext.CommandStore).Returns(LCommandDbSet.Object);
            LMockDbContext.Setup(AMainDbContext => AMainDbContext.Parking).Returns(LParkingDbSet.Object);
            LMockDbContext.Setup(AMainDbContext => AMainDbContext.ParkingPlaces).Returns(LParkingPlaceDbSet.Object);

            // Create test instance with all dependencies
            // Note: we do not fake IAuthentication because it only returns GUID for demo purposes,
            // also we use ICommands that invokes SaveChangesAsync on mocked DB context,
            // therefore we have to expect more than one call of SaveChangesAsync when invoking CommandHandler
            var LAuthentication = new Authentication();
            var LCommandStore = new Commands(LAuthentication, LMockDbContext.Object);
            FParkingCommandHandler = new ParkingCommandHandler(LMockDbContext.Object, LCommandStore, LAuthentication);

        }

        [Fact]
        public async Task Should_CreateParking() 
        {

            // Arrange
            var LCommand = new CreateParking 
            { 
                ParkingName = "Best Parking",
                Capacity = 1000
            };

            // Act
            await FParkingCommandHandler.Handle(LCommand);

            // Assert
            LMockDbContext.Verify(AMockDbContext => AMockDbContext.SaveChangesAsync(CancellationToken.None), Times.Exactly(2));

        }

        [Fact]
        public async Task Should_OpenParking() 
        {

            // Arrange
            var LCommand = new OpenParking 
            { 
                ParkingName = "Poznan Plaza"
            };

            // Act
            await FParkingCommandHandler.Handle(LCommand);

            // Assert
            LMockDbContext.Verify(AMockDbContext => AMockDbContext.SaveChangesAsync(CancellationToken.None), Times.Exactly(2));

        }

        [Fact]
        public async Task Should_CloseParking() 
        {

            // Arrange
            var LCommand = new CloseParking
            {
                ParkingName = "Parking-786359"
            };

            // Act
            await FParkingCommandHandler.Handle(LCommand);

            // Assert
            LMockDbContext.Verify(AMockDbContext => AMockDbContext.SaveChangesAsync(CancellationToken.None), Times.Exactly(2));

        }

        [Theory]
        [InlineData("Parking-786359", 3)]
        public async Task Should_TakeParkingPlace(string ParkingName, int PlaceNumber) 
        {

            // Arrange
            var LCommand = new TakeParkingPlace
            {
                ParkingName = ParkingName,
                PlaceNumber = PlaceNumber
            };

            // Act
            await FParkingCommandHandler.Handle(LCommand);

            // Assert
            LMockDbContext.Verify(AMockDbContext => AMockDbContext.SaveChangesAsync(CancellationToken.None), Times.Exactly(2));

        }

        [Theory]
        [InlineData("Parking-786359", 4)]
        public async Task Should_LeaveParkingPlace(string ParkingName, int PlaceNumber) 
        {

            // Arrange
            var LCommand = new LeaveParkingPlace
            {
                ParkingName = ParkingName,
                PlaceNumber = PlaceNumber
            };

            // Act
            await FParkingCommandHandler.Handle(LCommand);

            // Assert
            LMockDbContext.Verify(AMockDbContext => AMockDbContext.SaveChangesAsync(CancellationToken.None), Times.Exactly(2));

        }

    }

}
