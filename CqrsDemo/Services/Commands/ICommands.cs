using System.Threading;
using System.Threading.Tasks;

namespace CqrsDemo.Services.Commands
{
    public interface ICommands
    {
        Task Push(object ACommand, CancellationToken ACancellationToken = default);
    }
}
