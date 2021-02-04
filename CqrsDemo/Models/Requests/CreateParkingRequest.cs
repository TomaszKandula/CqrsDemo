using System.Text.Json.Serialization;

namespace CqrsDemo.Models.Requests
{
    public class CreateParkingRequest
    {
        [JsonPropertyName("parkingName")]
        public string ParkingName { get; set; }

        [JsonPropertyName("capacity")]
        public int Capacity { get; set; }
    }
}
