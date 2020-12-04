using Xunit;
using FluentAssertions;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CqrsDemo.Models.Requests;
using CqrsDemo.IntegrationTests.Configuration;

namespace CqrsDemo.IntegrationTests
{

    public class ControllerTest_Parking : IClassFixture<TestFixture<Startup>> // refactor!
    {

        private readonly HttpClient FHttpClient;

        public ControllerTest_Parking(TestFixture<Startup> ACustomFixture)
        {
            FHttpClient = ACustomFixture.FClient;
        }

        [Fact]
        public async Task Should_GetAllParkingInfos() 
        {

            // Arrange
            var LRequest = $"/api/v1/parking/";

            // Act
            var LResponse = await FHttpClient.GetAsync(LRequest);
            LResponse.EnsureSuccessStatusCode();

            // Assert
            var LStringResponse = await LResponse.Content.ReadAsStringAsync();
            LStringResponse.Should().NotBeNullOrEmpty(); // [] if empty

        }

        [Theory]
        [InlineData("Mercury")]
        public async Task Should_GetParkingInfo(string ParkingName)
        {

            // Arrange
            var LRequest = $"/api/v1/parking/?parkingname={ParkingName}/";

            // Act
            var LResponse = await FHttpClient.GetAsync(LRequest);
            LResponse.EnsureSuccessStatusCode();

            // Assert
            var LStringResponse = await LResponse.Content.ReadAsStringAsync();
            LStringResponse.Should().NotBeNullOrEmpty(); // [] if empty

        }

        [Fact]
        public async Task Should_GetTotalAvailablePlaces()
        {

            // Arrange
            var LRequest = $"/api/v1/parking/availableplaces/count/";

            // Act
            var LResponse = await FHttpClient.GetAsync(LRequest);
            LResponse.EnsureSuccessStatusCode();

            // Assert
            var LStringResponse = await LResponse.Content.ReadAsStringAsync();
            LStringResponse.Should().Be("0"); // "0" if empty

        }

        [Fact]
        public async Task Should_GetRandomAvailablePlace() 
        {

            // Arrange
            var LRequest = $"/api/v1/parking/availableplaces/random/";

            // Act
            var LResponse = await FHttpClient.GetAsync(LRequest);
            LResponse.EnsureSuccessStatusCode();

            // Assert
            var LStringResponse = await LResponse.Content.ReadAsStringAsync();
            LStringResponse.Should().NotBeNullOrEmpty(); // [] if empty

        }

        [Fact]
        public async Task Should_CreateParking() 
        {

            // Arrange
            var LRequest = $"/api/v1/parking/";

            var NewId = Guid.NewGuid().ToString();
            var LPayLoad = new CreateParkingRequest
            {
                ParkingName = $"Parking-{NewId[0..6]}",
                Capacity = 1000
            };

            var LNewRequest = new HttpRequestMessage(HttpMethod.Post, LRequest);
            LNewRequest.Content = new StringContent(JsonConvert.SerializeObject(LPayLoad), System.Text.Encoding.Default, "application/json");

            // Act
            var LResponse = await FHttpClient.SendAsync(LNewRequest);
            var LContent = await LResponse.Content.ReadAsStringAsync();
            LResponse.EnsureSuccessStatusCode();

            // Assert
            LContent.Should().Be(""); // "" if OK

        }

        [Theory]
        [InlineData("Poznan")]
        public async Task Should_OpenParking(string ParkingName) 
        {

            // Arrange
            var LRequest = $"/api/v1/parking/{ParkingName}/open";
            var LNewRequest = new HttpRequestMessage(HttpMethod.Post, LRequest);

            // Act
            var LResponse = await FHttpClient.SendAsync(LNewRequest);
            var LContent = await LResponse.Content.ReadAsStringAsync();

            LResponse.EnsureSuccessStatusCode();
            //LResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // Assert
            LContent.Should().Be("Cannot find parking 'Poznan'.");

        }

        [Theory]
        [InlineData("Poznan")]
        public async Task Should_CloseParking(string ParkingName) 
        {

            // Arrange
            var LRequest = $"/api/v1/parking/{ParkingName}/close";
            var LNewRequest = new HttpRequestMessage(HttpMethod.Post, LRequest);

            // Act
            var LResponse = await FHttpClient.SendAsync(LNewRequest);
            var LContent = await LResponse.Content.ReadAsStringAsync();

            LResponse.EnsureSuccessStatusCode();
            //LResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // Assert
            LContent.Should().Be("Cannot find parking 'Poznan'.");

        }

        [Theory]
        [InlineData("Poznan", 123456)]
        public async Task Should_TakeParkingPlace(string ParkingName, int PlaceNumber) 
        {

            // Arrange
            var LRequest = $"/api/v1/parking/{ParkingName}/{PlaceNumber}/take";
            var LNewRequest = new HttpRequestMessage(HttpMethod.Post, LRequest);

            // Act
            var LResponse = await FHttpClient.SendAsync(LNewRequest);
            var LContent = await LResponse.Content.ReadAsStringAsync();

            LResponse.EnsureSuccessStatusCode();
            //LResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // Assert
            LContent.Should().Be("Cannot find place #123456 in 'Poznan'.");

        }

        [Theory]
        [InlineData("Poznan", 123456)]
        public async Task Should_LeaveParkingPlace(string ParkingName, int PlaceNumber) 
        {

            // Arrange
            var LRequest = $"/api/v1/parking/{ParkingName}/{PlaceNumber}/take";
            var LNewRequest = new HttpRequestMessage(HttpMethod.Post, LRequest);

            // Act
            var LResponse = await FHttpClient.SendAsync(LNewRequest);
            var LContent = await LResponse.Content.ReadAsStringAsync();

            LResponse.EnsureSuccessStatusCode();
            //LResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // Assert
            LContent.Should().Be("Cannot find place #123456 in 'Poznan'.");

        }

    }

}
