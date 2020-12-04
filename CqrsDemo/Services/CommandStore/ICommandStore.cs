using System.Threading.Tasks;

namespace CqrsDemo.Services.CommandStore
{

    public interface ICommandStore
    {
        Task Push(object Command);
    }

}
