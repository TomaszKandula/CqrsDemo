using CqrsDemo.Services.Authentication;

namespace CqrsDemo.UnitTests.Services
{   
    public class FakeAuthentication : Authentication
    {
        public FakeAuthentication() 
        {        
        }

        public override string GetUserId { get => "69078034-7ca9-4c66-893f-2e48c7bdc14a"; }
    }
}
