using CqrsDemo.Database;
using CqrsDemo.Handlers.Queries;
using CqrsDemo.Handlers.Commands;
using CqrsDemo.Services.Commands;
using CqrsDemo.Services.Authentication;

namespace CqrsDemo.Handlers
{

    public class HandlerContext : IHandlerContext
    {

        private readonly MainDbContext FMainDbContext;
        private readonly ICommands FCommandStore;
        private readonly IAuthentication FAuthentication;

        private ParkingQueryHandler FParkingQueryHandler;
        private ParkingCommandHandler FParkingCommandHandler;

        public HandlerContext(MainDbContext AMainDbContext, ICommands ACommandStore, IAuthentication AAuthentication) 
        {
            FMainDbContext = AMainDbContext;
            FCommandStore = ACommandStore;
            FAuthentication = AAuthentication;
        }

        public IParkingCommandHandler CommandHandlers
        {
            get
            {
                if (FParkingCommandHandler == null) FParkingCommandHandler = new ParkingCommandHandler(FMainDbContext, FCommandStore, FAuthentication);
                return FParkingCommandHandler;
            }
        }

        public IParkingQueryHandler QueryHandlers
        {
            get
            {
                if (FParkingQueryHandler == null) FParkingQueryHandler = new ParkingQueryHandler(FMainDbContext);
                return FParkingQueryHandler;
            }
        }

    }

}
