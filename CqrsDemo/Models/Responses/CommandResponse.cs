using System.Text.Json.Serialization;

namespace CqrsDemo.Models.Responses
{
    public class CommandResponse
    {
        [JsonPropertyName("isSucceeded")]
        public bool IsSucceeded { get; set; }
        [JsonPropertyName("errorCode")]
        public string ErrorCode { get; set; } = "no_errors";
        [JsonPropertyName("errorDesc")]
        public string ErrorDesc { get; set; } = "n/a";
    }
}
