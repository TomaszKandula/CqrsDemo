namespace CqrsDemo.Handlers.Commands.Models
{

    public class CreateParking
    {
        public string ParkingName { get; set; }
        public int Capacity { get; set; }
    }

}
