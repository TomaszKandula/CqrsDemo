using System;

namespace CqrsDemo.Services.Authentication
{

    public class Authentication : IAuthentication
    {

        private readonly string FUserId;

        public Authentication() 
        {
            FUserId = Guid.NewGuid().ToString();
        }

        public string GetUserId { get => FUserId; }

    }

}
