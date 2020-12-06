using System.Text.Json.Serialization;

namespace CqrsDemo.Models.Responses
{
    public class CommandResponse
    {
        [JsonPropertyName("isSucceeded")]
        public bool IsSucceeded { get; set; }
    }
}
