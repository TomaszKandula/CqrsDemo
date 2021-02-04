namespace CqrsDemo.Handlers.Queries.GetParkingInfo
{
    public class GetParkingInfoQueryResult
    {
        public string Name { get; set; }

        public bool IsOpened { get; set; }

        public int MaximumPlaces { get; set; }

        public int AvailablePlaces { get; set; }
    }
}
