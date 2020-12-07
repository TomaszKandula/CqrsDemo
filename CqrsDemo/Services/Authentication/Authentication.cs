using System;

namespace CqrsDemo.Services.Authentication
{

    public class Authentication : IAuthentication
    {

        public Authentication() 
        {
        }

        public virtual string GetUserId { get => Guid.NewGuid().ToString(); }

    }

}
