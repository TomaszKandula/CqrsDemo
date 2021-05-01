using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CqrsDemo.Shared.Dto;
using CqrsDemo.Cqrs.Mappers;
using CqrsDemo.Cqrs.Handlers.Queries.GetParkingInfo;
using CqrsDemo.Cqrs.Handlers.Queries.GetAllParkingInfo;
using CqrsDemo.Cqrs.Handlers.Queries.GetTotalAvailablePlaces;
using CqrsDemo.Cqrs.Handlers.Queries.GetRandomAvailablePlace;
using MediatR;

namespace CqrsDemo.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class ParkingController : ControllerBase
    {
        private readonly IMediator FMediator;

        public ParkingController(IMediator AMediator) => FMediator = AMediator;

        [HttpGet]
        public async Task<IEnumerable<GetAllParkingInfoQueryResult>> GetAllParkingInfo()
            => await FMediator.Send(new GetAllParkingInfoQuery());

        [HttpGet("{ParkingName}")]
        public async Task<GetParkingInfoQueryResult> GetParkingInfo([FromRoute] string ParkingName)
            => await FMediator.Send(new GetParkingInfoQuery { ParkingName = ParkingName });

        [HttpGet]
        public async Task<GetTotalAvailablePlacesQueryResult> GetTotalAvailablePlaces()
            => await FMediator.Send(new GetTotalAvailablePlacesQuery());

        [HttpGet]
        public async Task<GetRandomAvailablePlaceQueryResult> GetRandomAvailablePlace()
            => await FMediator.Send(new GetRandomAvailablePlaceQuery());

        [HttpPost]
        public async Task<Unit> CreateParking([FromBody] CreateParkingDto PayLoad)
            => await FMediator.Send(ParkingMapper.MapToCreateParkingCommand(PayLoad));

        [HttpPost]
        public async Task<Unit> OpenParking([FromBody] OpenParkingDto PayLoad)
            => await FMediator.Send(ParkingMapper.MapToOpenParkingCommand(PayLoad));

        [HttpPost]
        public async Task<Unit> CloseParking([FromBody] CloseParkingDto PayLoad)
            => await FMediator.Send(ParkingMapper.MapToCloseParkingCommand(PayLoad));

        [HttpPost]
        public async Task<Unit> TakeParkingPlace([FromBody] TakeParkingPlaceDto PayLoad)
            => await FMediator.Send(ParkingMapper.MapToTakeParkingPlaceCommand(PayLoad));

        [HttpPost]
        public async Task<Unit> LeaveParkingPlace([FromBody] LeaveParkingPlaceDto PayLoad)
            => await FMediator.Send(ParkingMapper.MapToLeaveParkingPlaceCommand(PayLoad));
    }
}
