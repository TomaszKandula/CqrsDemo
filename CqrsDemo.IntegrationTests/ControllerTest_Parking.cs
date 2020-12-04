using Xunit;
using FluentAssertions;
using System.Net.Http;
using CqrsDemo.IntegrationTests.Configuration;

namespace CqrsDemo.IntegrationTests
{

    public class ControllerTest_Parking : IClassFixture<TestFixture<Startup>>
    {

        private readonly HttpClient FClient;

        public ControllerTest_Parking(TestFixture<Startup> ACustomFixture)
        {
            FClient = ACustomFixture.FClient;
        }

        [Fact]
        public void Should_GetParkingInfo()
        {

        }

        [Fact]
        public void Should_GetTotalAvailablePlaces() 
        { 
        
        }

        [Fact]
        public void Should_GetRandomAvailablePlace() 
        { 
        
        }

        [Fact]
        public void Should_CreateParking() 
        { 
        
        }

        [Fact]
        public void Should_OpenParking() 
        { 
        
        }

        [Fact]
        public void Should_CloseParking() 
        { 
        
        }

        [Fact]
        public void Should_TakeParkingPlace() 
        { 
        
        }

        [Fact]
        public void Should_LeaveParkingPlace() 
        { 
        
        }

    }

}
