using System.Collections.Generic;

namespace CqrsDemo.Infrastructure.Domain.Entities
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
