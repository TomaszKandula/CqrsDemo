using System.Text.Json.Serialization;

namespace CqrsDemo.Models.Responses
{

    public class AvailablePlaceInfo
    {
        [JsonPropertyName("number")]
        public int Number { get; set; }
    }

}
