using Xunit;
using FluentAssertions;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using CqrsDemo.Models.Requests;
using CqrsDemo.Models.Responses;
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
            var LContent = await LResponse.Content.ReadAsStringAsync();

            // Assert
            LResponse.EnsureSuccessStatusCode();
            LContent.Should().NotBeNull();

            var LDeserialized = JsonConvert.DeserializeObject<IEnumerable<ParkingInfo>>(LContent);
            LDeserialized.Should().HaveCountGreaterThan(0);

        }

        [Theory]
        [InlineData("Lidl Parking")]
        public async Task Should_GetParkingInfo(string ParkingName)
        {

            // Arrange
            var LRequest = $"/api/v1/parking/{ParkingName}/";

            // Act
            var LResponse = await FHttpClient.GetAsync(LRequest);
            var LContent = await LResponse.Content.ReadAsStringAsync();

            // Assert
            LResponse.EnsureSuccessStatusCode();
            LContent.Should().NotBeNull();

            var LDeserialized = JsonConvert.DeserializeObject<ParkingInfo>(LContent);
            LDeserialized.Name.Should().Be("Lidl Parking");

        }

        [Fact]
        public async Task Should_GetTotalAvailablePlaces()
        {

            // Arrange
            var LRequest = $"/api/v1/parking/availableplaces/count/";

            // Act
            var LResponse = await FHttpClient.GetAsync(LRequest);
            var LContent = await LResponse.Content.ReadAsStringAsync();

            // Assert
            LResponse.EnsureSuccessStatusCode();
            LContent.Should().NotBeNull();

            var LDeserialized = JsonConvert.DeserializeObject<AvailablePlaceInfo>(LContent);
            LDeserialized.Number.Should().BeGreaterThan(0);

        }

        [Fact]
        public async Task Should_GetRandomAvailablePlace() 
        {

            // Arrange
            var LRequest = $"/api/v1/parking/availableplaces/random/";

            // Act
            var LResponse = await FHttpClient.GetAsync(LRequest);
            var LContent = await LResponse.Content.ReadAsStringAsync();

            // Assert
            LResponse.EnsureSuccessStatusCode();
            LContent.Should().NotBeNull();

            var LDeserialized = JsonConvert.DeserializeObject<ParkingPlaceInfo>(LContent);
            LDeserialized.Number.Should().NotBe(0);
            LDeserialized.ParkingName.Should().NotBeNullOrEmpty();

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
                Capacity = 10
            };

            var LNewRequest = new HttpRequestMessage(HttpMethod.Post, LRequest);
            LNewRequest.Content = new StringContent(JsonConvert.SerializeObject(LPayLoad), System.Text.Encoding.Default, "application/json");

            // Act
            var LResponse = await FHttpClient.SendAsync(LNewRequest);
            var LContent = await LResponse.Content.ReadAsStringAsync();

            // Assert
            LResponse.EnsureSuccessStatusCode();
            LContent.Should().NotBeNull();

            var LDeserialized = JsonConvert.DeserializeObject<CommandResponse>(LContent);
            LDeserialized.IsSucceeded.Should().BeTrue();

        }

        [Theory]
        [InlineData("BlaBlaBla")]
        public async Task Should_FailToOpenParking(string ParkingName) 
        {

            // Arrange
            var LRequest = $"/api/v1/parking/{ParkingName}/open";
            var LNewRequest = new HttpRequestMessage(HttpMethod.Post, LRequest);

            // Act
            var LResponse = await FHttpClient.SendAsync(LNewRequest);
            var LContent = await LResponse.Content.ReadAsStringAsync();

            // Assert
            LResponse.EnsureSuccessStatusCode();
            LContent.Should().NotBeNull();

            var LDeserialized = JsonConvert.DeserializeObject<CommandResponse>(LContent);
            LDeserialized.IsSucceeded.Should().BeFalse();

        }

        [Theory]
        [InlineData("BlaBlaBla")]
        public async Task Should_FailToCloseParking(string ParkingName) 
        {

            // Arrange
            var LRequest = $"/api/v1/parking/{ParkingName}/close";
            var LNewRequest = new HttpRequestMessage(HttpMethod.Post, LRequest);

            // Act
            var LResponse = await FHttpClient.SendAsync(LNewRequest);
            var LContent = await LResponse.Content.ReadAsStringAsync();

            // Assert
            LResponse.EnsureSuccessStatusCode();
            LContent.Should().NotBeNull();

            var LDeserialized = JsonConvert.DeserializeObject<CommandResponse>(LContent);
            LDeserialized.IsSucceeded.Should().BeFalse();

        }

        [Theory]
        [InlineData("Poznan Plaza", 1)]
        public async Task Should_TakeParkingPlace(string ParkingName, int PlaceNumber) 
        {

            // Arrange
            var LRequest = $"/api/v1/parking/{ParkingName}/{PlaceNumber}/take";
            var LNewRequest = new HttpRequestMessage(HttpMethod.Post, LRequest);

            // Act
            var LResponse = await FHttpClient.SendAsync(LNewRequest);
            var LContent = await LResponse.Content.ReadAsStringAsync();

            // Assert
            LResponse.EnsureSuccessStatusCode();
            LContent.Should().NotBeNull();

            var LDeserialized = JsonConvert.DeserializeObject<CommandResponse>(LContent);
            LDeserialized.IsSucceeded.Should().BeTrue();

        }

        [Theory]
        [InlineData("Poznan Plaza", 1)]
        public async Task Should_FailToLeaveParkingPlace(string ParkingName, int PlaceNumber) 
        {

            // Arrange
            var LRequest = $"/api/v1/parking/{ParkingName}/{PlaceNumber}/take";
            var LNewRequest = new HttpRequestMessage(HttpMethod.Post, LRequest);

            // Act
            var LResponse = await FHttpClient.SendAsync(LNewRequest);
            var LContent = await LResponse.Content.ReadAsStringAsync();

            // Assert
            LResponse.EnsureSuccessStatusCode();
            LContent.Should().NotBeNull();

            var LDeserialized = JsonConvert.DeserializeObject<CommandResponse>(LContent);
            LDeserialized.IsSucceeded.Should().BeFalse();

        }

    }

}
