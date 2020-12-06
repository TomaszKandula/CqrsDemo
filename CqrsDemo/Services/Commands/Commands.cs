using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CqrsDemo.Database;
using CqrsDemo.Database.Models;
using CqrsDemo.Services.Authentication;

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

        public async Task Push(object ACommand)
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

            await FMainDbContext.SaveChangesAsync();

        }

    }

}
