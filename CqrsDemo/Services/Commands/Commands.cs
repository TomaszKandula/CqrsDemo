using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CqrsDemo.Services.Authentication;
using CqrsDemo.Infrastructure.Database;
using CqrsDemo.Infrastructure.Domain.Entities;

namespace CqrsDemo.Services.Commands
{
    public class Commands : ICommands
    {
        private readonly MainDbContext FMainDbContext;
        
        private readonly IAuthentication FAuthentication;

        public Commands(IAuthentication AAuthentication, MainDbContext AMainDbContext) 
        {
            FMainDbContext = AMainDbContext;
            FAuthentication = AAuthentication;
        }

        public Commands() { }

        public virtual async Task Push(object ACommand, CancellationToken ACancellationToken = default)
        {
            FMainDbContext.CommandStore.Add(
                new CommandStore
                {
                    Type      = ACommand.GetType().Name,
                    Data      = JsonConvert.SerializeObject(ACommand),
                    CreatedAt = DateTime.Now,
                    UserId    = FAuthentication.GetUserId
                }
            );

            await FMainDbContext.SaveChangesAsync(ACancellationToken);
        }
    }
}
