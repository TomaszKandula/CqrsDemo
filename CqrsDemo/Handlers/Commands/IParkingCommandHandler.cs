using System.Threading.Tasks;
using CqrsDemo.Handlers.Commands.Models;

namespace CqrsDemo.Handlers.Commands
{

    public interface IParkingCommandHandler
    {
        Task Handle(CloseParking ACommand);
        Task Handle(CreateParking ACommand);
        Task Handle(LeaveParking ACommand);
        Task Handle(OpenParking ACommand);
        Task Handle(TakeParkingPlace ACommand);
    }

}
