namespace CqrsDemo.Cqrs.Handlers.Queries.GetAllParkingInfo
{
    public class GetAllParkingInfoQueryResult
    {
        public string Name { get; set; }

        public bool IsOpened { get; set; }

        public int MaximumPlaces { get; set; }

        public int AvailablePlaces { get; set; }
    }
}
