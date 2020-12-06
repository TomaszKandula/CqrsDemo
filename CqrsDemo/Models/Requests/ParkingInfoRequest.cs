using System.Text.Json.Serialization;

namespace CqrsDemo.Models.Requests
{

    public class ParkingInfoRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

}
