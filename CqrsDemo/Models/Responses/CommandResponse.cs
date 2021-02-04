namespace CqrsDemo.Models.Responses
{
    public class CommandResponse
    {
        public bool IsSucceeded { get; set; }

        public string ErrorCode { get; set; } = "no_errors";

        public string ErrorDesc { get; set; } = "n/a";
    }
}
