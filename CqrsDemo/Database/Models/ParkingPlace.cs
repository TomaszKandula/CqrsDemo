namespace CqrsDemo.Database.Models
{

    public class ParkingPlace
    {
        public string ParkingName { get; set; }
        public int Number { get; set; }
        public bool IsFree { get; set; }
        public string UserId { get; set; }
        public virtual Parking ParkingNameNavigation { get; set; }
    }

}
