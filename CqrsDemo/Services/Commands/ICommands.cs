using System.Threading.Tasks;

namespace CqrsDemo.Services.Commands
{

    public interface ICommands
    {
        Task Push(object Command);
    }

}
