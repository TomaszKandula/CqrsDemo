using CqrsDemo.Handlers.Queries;
using CqrsDemo.Handlers.Commands;

namespace CqrsDemo.Handlers
{

    public interface IHandlerContext
    {
        IParkingQueryHandler QueryHandlers { get; }
        IParkingCommandHandler CommandHandlers { get; }
    }

}
