using System.Threading;
using System.Threading.Tasks;
using CqrsDemo.Services.Commands;

namespace CqrsDemo.UnitTests.Services
{
    public class FakeCommands : Commands
    {
        public FakeCommands() 
        { 
        }

        public override async Task Push(object ACommand, CancellationToken ACancellationToken)
        {
            await Task.Run(() => { /* do nothing */ });
        }
   
    }
}
