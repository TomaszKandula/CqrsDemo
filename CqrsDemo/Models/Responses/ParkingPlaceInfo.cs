using System.Text.Json.Serialization;

namespace CqrsDemo.Models.Responses
{
    public class ParkingPlaceInfo
    {
        [JsonPropertyName("parkingName")]
        public string ParkingName { get; set; }

        [JsonPropertyName("number")]
        public int Number { get; set; }
    }
}
