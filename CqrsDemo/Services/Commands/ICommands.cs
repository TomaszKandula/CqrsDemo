using System.Threading;
using System.Threading.Tasks;

namespace CqrsDemo.Services.Commands
{
    public interface ICommands
    {
        Task Push(object Command, CancellationToken ACancellationToken = default);
    }
}
