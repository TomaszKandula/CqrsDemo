using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CqrsDemo.Shared.Dto;
using CqrsDemo.Cqrs.Mappers;
using CqrsDemo.Handlers.Queries.GetParkingInfo;
using CqrsDemo.Handlers.Queries.GetAllParkingInfo;
using CqrsDemo.Handlers.Queries.GetTotalAvailablePlaces;
using CqrsDemo.Handlers.Queries.GetRandomAvailablePlace;
using MediatR;

namespace CqrsDemo.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class ParkingController : ControllerBase
    {
        private readonly IMediator FMediator;

        public ParkingController(IMediator AMediator)
        {
            FMediator = AMediator;
        }

        [HttpGet]
        public async Task<IEnumerable<GetAllParkingInfoQueryResult>> GetAllParkingInfo()
        {
            var LQuery = new GetAllParkingInfoQuery();
            return await FMediator.Send(LQuery);
        }

        [HttpGet("{ParkingName}")]
        public async Task<GetParkingInfoQueryResult> GetParkingInfo([FromRoute] string ParkingName)
        {
            var LQuery = new GetParkingInfoQuery { ParkingName = ParkingName };
            return await FMediator.Send(LQuery);
        }

        [HttpGet()]
        public async Task<GetTotalAvailablePlacesQueryResult> GetTotalAvailablePlaces()
        {
            var LQuery = new GetTotalAvailablePlacesQuery();
            return await FMediator.Send(LQuery);
        }

        [HttpGet()]
        public async Task<GetRandomAvailablePlaceQueryResult> GetRandomAvailablePlace()
        {
            var LQuery = new GetRandomAvailablePlaceQuery();
            return await FMediator.Send(LQuery);
        }

        [HttpPost]
        public async Task<Unit> CreateParking([FromBody] CreateParkingDto PayLoad)
        {
            var LCommand = ParkingMapper.MapToCreateParkingCommand(PayLoad);
            return await FMediator.Send(LCommand);
        }

        [HttpPost()]
        public async Task<Unit> OpenParking([FromBody] OpenParkingDto PayLoad)
        {
            var LCommand = ParkingMapper.MapToOpenParkingCommand(PayLoad);
            return await FMediator.Send(LCommand);
        }

        [HttpPost()]
        public async Task<Unit> CloseParking([FromBody] CloseParkingDto PayLoad)
        {
            var LCommand = ParkingMapper.MapToCloseParkingCommand(PayLoad);
            return await FMediator.Send(LCommand);
        }

        [HttpPost()]
        public async Task<Unit> TakeParkingPlace([FromBody] TakeParkingPlaceDto PayLoad)
        {
            var LCommand = ParkingMapper.MapToTakeParkingPlaceCommand(PayLoad);
            return await FMediator.Send(LCommand);
        }

        [HttpPost()]
        public async Task<Unit> LeaveParkingPlace([FromBody] LeaveParkingPlaceDto PayLoad)
        {
            var LCommand = ParkingMapper.MapToLeaveParkingPlaceCommand(PayLoad);
            return await FMediator.Send(LCommand);
        }
    }
}
