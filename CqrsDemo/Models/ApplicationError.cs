namespace CqrsDemo.Models
{
    public sealed class ApplicationError
    {
        public string ErrorMessage { get; set; }
        
        public string ErrorCode { get; set; }

        public ApplicationError(string AErrorCode, string AErrorMessage)
        {
            ErrorCode = AErrorCode;
            ErrorMessage = AErrorMessage;
        }
    }
}
