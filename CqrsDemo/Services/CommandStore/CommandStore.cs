using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CqrsDemo.Database;
using CqrsDemo.Database.Models;
using CqrsDemo.Services.Authentication;

namespace CqrsDemo.Services.CommandStore
{

    public class CommandStore : ICommandStore
    {

        private readonly MainDbContext FMainDbContext;
        private readonly IAuthentication FAuthentication;

        public CommandStore(IAuthentication AAuthentication, MainDbContext AMainDbContext) 
        {
            FMainDbContext = AMainDbContext;
            FAuthentication = AAuthentication;
        }

        public async Task Push(object ACommand)
        {

            FMainDbContext.Set<Database.Models.CommandStore>().Add(
                new Database.Models.CommandStore
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
