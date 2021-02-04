namespace CqrsDemo.Models.Requests
{
    public class CreateParkingRequest
    {
        public string ParkingName { get; set; }

        public int Capacity { get; set; }
    }
}
