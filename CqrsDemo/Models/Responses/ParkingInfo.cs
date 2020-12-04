using System.Text.Json.Serialization;

namespace CqrsDemo.Models.Responses
{

    public class ParkingInfo
    {

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("isOpened")]
        public bool IsOpened { get; set; }

        [JsonPropertyName("maximumPlaces")]
        public int MaximumPlaces { get; set; }

        [JsonPropertyName("availablePlaces")]
        public int AvailablePlaces { get; set; }

    }

}
