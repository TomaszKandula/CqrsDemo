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
using CqrsDemo.Cqrs.Handlers.Queries.GetParkingInfo;
using CqrsDemo.Cqrs.Handlers.Queries.GetAllParkingInfo;
using CqrsDemo.Cqrs.Handlers.Queries.GetTotalAvailablePlaces;
using CqrsDemo.Cqrs.Handlers.Queries.GetRandomAvailablePlace;

namespace CqrsDemo.IntegrationTests
{
    public class ControllerTestParking : IClassFixture<TestFixture<Startup>>
    {
        private readonly HttpClient FHttpClient;

        public ControllerTestParking(TestFixture<Startup> ACustomFixture)
            => FHttpClient = ACustomFixture.Client;

        [Fact]
        public async Task Should_GetAllParkingInfos() 
        {
            // Arrange
            const string REQUEST = "/api/v1/parking/GetAllParkingInfo/";

            // Act
            var LResponse = await FHttpClient.GetAsync(REQUEST);
            var LContent = await LResponse.Content.ReadAsStringAsync();

            // Assert
            LResponse.EnsureSuccessStatusCode();
            LContent.Should().NotBeNull();

            var LDeserialized = JsonConvert.DeserializeObject<IEnumerable<GetAllParkingInfoQueryResult>>(LContent);
            LDeserialized.Should().HaveCountGreaterThan(0);
        }

        [Theory]
        [InlineData("Poznan Plaza")]
        public async Task Should_GetParkingInfo(string AParkingName)
        {
            // Arrange
            var LRequest = $"/api/v1/parking/GetParkingInfo/{AParkingName}/";

            // Act
            var LResponse = await FHttpClient.GetAsync(LRequest);
            var LContent = await LResponse.Content.ReadAsStringAsync();

            // Assert
            LResponse.EnsureSuccessStatusCode();
            LContent.Should().NotBeNull();

            var LDeserialized = JsonConvert.DeserializeObject<GetParkingInfoQueryResult>(LContent);
            LDeserialized.Name.Should().Be(AParkingName);
        }

        [Fact]
        public async Task Should_GetTotalAvailablePlaces()
        {
            // Arrange
            const string REQUEST = "/api/v1/parking/GetTotalAvailablePlaces/";

            // Act
            var LResponse = await FHttpClient.GetAsync(REQUEST);
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
            const string REQUEST = "/api/v1/parking/GetRandomAvailablePlace/";

            // Act
            var LResponse = await FHttpClient.GetAsync(REQUEST);
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
            const string REQUEST = "/api/v1/parking/CreateParking/";

            var LNewId = Guid.NewGuid().ToString();
            var LPayLoad = new CreateParkingDto
            {
                ParkingName = $"Parking-{LNewId[..6]}",
                Capacity = 10
            };

            var LNewRequest = new HttpRequestMessage(HttpMethod.Post, REQUEST)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(LPayLoad), 
                    System.Text.Encoding.Default,
                    "application/json")
            };

            // Act
            var LResponse = await FHttpClient.SendAsync(LNewRequest);
            var LContent = await LResponse.Content.ReadAsStringAsync();

            // Assert
            LResponse.EnsureSuccessStatusCode();
            LContent.Should().NotBeNull();
        }

        [Theory]
        [InlineData("BlaBlaBla")]
        public async Task Should_FailToOpenParking(string AParkingName) 
        {
            // Arrange
            const string REQUEST = "/api/v1/parking/OpenParking/";
            var LPayLoad = new OpenParkingDto
            {
                ParkingName = AParkingName
            };

            var LNewRequest = new HttpRequestMessage(HttpMethod.Post, REQUEST)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(LPayLoad), 
                    System.Text.Encoding.Default,
                    "application/json")
            };

            // Act
            var LResponse = await FHttpClient.SendAsync(LNewRequest);

            // Assert
            LResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("BlaBlaBla")]
        public async Task Should_FailToCloseParking(string AParkingName) 
        {
            // Arrange
            const string REQUEST = "/api/v1/parking/CloseParking/";
            var LPayLoad = new CloseParkingDto
            {
                ParkingName = AParkingName
            };

            var LNewRequest = new HttpRequestMessage(HttpMethod.Post, REQUEST)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(LPayLoad), 
                    System.Text.Encoding.Default,
                    "application/json")
            };

            // Act
            var LResponse = await FHttpClient.SendAsync(LNewRequest);

            // Assert           
            LResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("Poznan Plaza", 100)]
        public async Task Should_FailToTakeParkingPlace(string AParkingName, int APlaceNumber) 
        {
            // Arrange
            const string REQUEST = "/api/v1/parking/TakeParkingPlace/";
            var LPayLoad = new TakeParkingPlaceDto
            {
                ParkingName = AParkingName,
                PlaceNumber = APlaceNumber
            };

            var LNewRequest = new HttpRequestMessage(HttpMethod.Post, REQUEST)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(LPayLoad), 
                    System.Text.Encoding.Default,
                    "application/json")
            };

            // Act
            var LResponse = await FHttpClient.SendAsync(LNewRequest);

            // Assert
            LResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("Poznan Plaza", 999)]
        public async Task Should_FailToLeaveParkingPlace(string AParkingName, int APlaceNumber) 
        {
            // Arrange
            const string REQUEST = "/api/v1/parking/LeaveParkingPlace/";
            var LPayLoad = new LeaveParkingPlaceDto
            {
                ParkingName = AParkingName,
                PlaceNumber = APlaceNumber
            };

            var LNewRequest = new HttpRequestMessage(HttpMethod.Post, REQUEST)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(LPayLoad), 
                    System.Text.Encoding.Default,
                    "application/json")
            };

            // Act
            var LResponse = await FHttpClient.SendAsync(LNewRequest);

            // Assert
            LResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
