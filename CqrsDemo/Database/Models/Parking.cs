using System.Collections.Generic;

namespace CqrsDemo.Database.Models
{
    public class Parking
    {
        public Parking()
        {
            ParkingPlaces = new HashSet<ParkingPlace>();
        }

        public string Name { get; set; }
        public bool IsOpened { get; set; }

        public virtual ICollection<ParkingPlace> ParkingPlaces { get; set; }
    }
}
