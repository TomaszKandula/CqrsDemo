using Xunit;
using FluentAssertions;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using CqrsDemo.Shared.Dto;
using CqrsDemo.IntegrationTests.Configuration;
using CqrsDemo.Handlers.Queries.GetParkingInfo;
using CqrsDemo.Handlers.Queries.GetAllParkingInfo;
using CqrsDemo.Handlers.Queries.GetTotalAvailablePlaces;
using CqrsDemo.Handlers.Queries.GetRandomAvailablePlace;

namespace CqrsDemo.IntegrationTests
{
    public class ControllerTest_Parking : IClassFixture<TestFixture<Startup>>
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
            var LRequest = $"/api/v1/parking/GetAllParkingInfo/";

            // Act
            var LResponse = await FHttpClient.GetAsync(LRequest);
            var LContent = await LResponse.Content.ReadAsStringAsync();

            // Assert
            LResponse.EnsureSuccessStatusCode();
            LContent.Should().NotBeNull();

            var LDeserialized = JsonConvert.DeserializeObject<IEnumerable<GetAllParkingInfoQueryResult>>(LContent);
            LDeserialized.Should().HaveCountGreaterThan(0);
        }

        [Theory]
        [InlineData("Poznan Plaza")]
        public async Task Should_GetParkingInfo(string ParkingName)
        {
            // Arrange
            var LRequest = $"/api/v1/parking/GetParkingInfo/{ParkingName}/";

            // Act
            var LResponse = await FHttpClient.GetAsync(LRequest);
            var LContent = await LResponse.Content.ReadAsStringAsync();

            // Assert
            LResponse.EnsureSuccessStatusCode();
            LContent.Should().NotBeNull();

            var LDeserialized = JsonConvert.DeserializeObject<GetParkingInfoQueryResult>(LContent);
            LDeserialized.Name.Should().Be(ParkingName);
        }

        [Fact]
        public async Task Should_GetTotalAvailablePlaces()
        {
            // Arrange
            var LRequest = $"/api/v1/parking/GetTotalAvailablePlaces/";

            // Act
            var LResponse = await FHttpClient.GetAsync(LRequest);
            var LContent = await LResponse.Content.ReadAsStringAsync();

            // Assert
            LResponse.EnsureSuccessStatusCode();
            LContent.Should().NotBeNull();

            var LDeserialized = JsonConvert.DeserializeObject<GetTotalAvailablePlacesQueryResult>(LContent);
            LDeserialized.Number.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Should_GetRandomAvailablePlace() 
        {
            // Arrange
            var LRequest = $"/api/v1/parking/GetRandomAvailablePlace/";

            // Act
            var LResponse = await FHttpClient.GetAsync(LRequest);
            var LContent = await LResponse.Content.ReadAsStringAsync();

            // Assert
            LResponse.EnsureSuccessStatusCode();
            LContent.Should().NotBeNull();

            var LDeserialized = JsonConvert.DeserializeObject<GetRandomAvailablePlaceQueryResult>(LContent);
            LDeserialized.Number.Should().NotBe(0);
            LDeserialized.ParkingName.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Should_CreateParking() 
        {
            // Arrange
            var LRequest = $"/api/v1/parking/CreateParking/";

            var NewId = Guid.NewGuid().ToString();
            var LPayLoad = new CreateParkingDto
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
        }

        [Theory]
        [InlineData("BlaBlaBla")]
        public async Task Should_FailToOpenParking(string ParkingName) 
        {
            // Arrange
            var LRequest = $"/api/v1/parking/OpenParking/";
            var LPayLoad = new OpenParkingDto
            {
                ParkingName = ParkingName
            };

            var LNewRequest = new HttpRequestMessage(HttpMethod.Post, LRequest);
            LNewRequest.Content = new StringContent(JsonConvert.SerializeObject(LPayLoad), System.Text.Encoding.Default, "application/json");

            // Act
            var LResponse = await FHttpClient.SendAsync(LNewRequest);

            // Assert
            LResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("BlaBlaBla")]
        public async Task Should_FailToCloseParking(string ParkingName) 
        {
            // Arrange
            var LRequest = $"/api/v1/parking/CloseParking/";
            var LPayLoad = new CloseParkingDto
            {
                ParkingName = ParkingName
            };

            var LNewRequest = new HttpRequestMessage(HttpMethod.Post, LRequest);
            LNewRequest.Content = new StringContent(JsonConvert.SerializeObject(LPayLoad), System.Text.Encoding.Default, "application/json");

            // Act
            var LResponse = await FHttpClient.SendAsync(LNewRequest);

            // Assert           
            LResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("Poznan Plaza", 100)]
        public async Task Should_FailToTakeParkingPlace(string ParkingName, int PlaceNumber) 
        {
            // Arrange
            var LRequest = $"/api/v1/parking/TakeParkingPlace/";
            var LPayLoad = new TakeParkingPlaceDto
            {
                ParkingName = ParkingName,
                PlaceNumber = PlaceNumber
            };

            var LNewRequest = new HttpRequestMessage(HttpMethod.Post, LRequest);
            LNewRequest.Content = new StringContent(JsonConvert.SerializeObject(LPayLoad), System.Text.Encoding.Default, "application/json");

            // Act
            var LResponse = await FHttpClient.SendAsync(LNewRequest);

            // Assert
            LResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("Poznan Plaza", 999)]
        public async Task Should_FailToLeaveParkingPlace(string ParkingName, int PlaceNumber) 
        {
            // Arrange
            var LRequest = $"/api/v1/parking/LeaveParkingPlace/";
            var LPayLoad = new LeaveParkingPlaceDto
            {
                ParkingName = ParkingName,
                PlaceNumber = PlaceNumber
            };

            var LNewRequest = new HttpRequestMessage(HttpMethod.Post, LRequest);
            LNewRequest.Content = new StringContent(JsonConvert.SerializeObject(LPayLoad), System.Text.Encoding.Default, "application/json");

            // Act
            var LResponse = await FHttpClient.SendAsync(LNewRequest);

            // Assert
            LResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
