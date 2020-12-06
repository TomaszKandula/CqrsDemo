using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CqrsDemo.Handlers;
using CqrsDemo.Models.Responses;
using CqrsDemo.Handlers.Queries.Models;
using CqrsDemo.Handlers.Commands.Models;
using CqrsDemo.Models.Requests;

namespace CqrsDemo.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class ParkingController : ControllerBase
    {

        private readonly IHandlerContext FHandlerContext;

        public ParkingController(IHandlerContext AHandlerContext) 
        {
            FHandlerContext = AHandlerContext;
        }

        [HttpGet]
        public async Task<IEnumerable<ParkingInfo>> GetAllParkingInfos()
        {
            var LQuery = new GetAllParkingInfo();
            return await FHandlerContext.QueryHandlers.Handle(LQuery);
        }

        [HttpGet("{ParkingName}")]
        public ParkingInfo GetParkingInfo([FromRoute] string ParkingName)
        {

            var LQuery = new GetParkingInfo
            {
                ParkingName = ParkingName
            };

            return FHandlerContext.QueryHandlers.Handle(LQuery);

        }

        [HttpGet("AvailablePlaces/Count")]
        public int GetTotalAvailablePlaces()
        {
            var LQuery = new GetTotalAvailablePlaces();
            return FHandlerContext.QueryHandlers.Handle(LQuery);
        }

        [HttpGet("AvailablePlaces/Random")]
        public ParkingPlaceInfo GetRandomAvailablePlace()
        {
            var LQuery = new GetRandomAvailablePlace();
            return FHandlerContext.QueryHandlers.Handle(LQuery);
        }

        [HttpPost]
        public async Task CreateParking([FromBody] CreateParkingRequest PayLoad)
        {

            var LCommand = new CreateParking
            {
                ParkingName = PayLoad.ParkingName,
                Capacity = PayLoad.Capacity
            };
            
            await FHandlerContext.CommandHandlers.Handle(LCommand);
        
        }

        [HttpPost("{ParkingName}/Open")]
        public async Task OpenParking([FromRoute] string ParkingName)
        {

            var LCommand = new OpenParking 
            { 
                ParkingName = ParkingName 
            };

            await FHandlerContext.CommandHandlers.Handle(LCommand);

        }

        [HttpPost("{ParkingName}/Close")]
        public async Task CloseParking([FromRoute] string ParkingName)
        {

            var LCommand = new CloseParking 
            { 
                ParkingName = ParkingName 
            };
            
            await FHandlerContext.CommandHandlers.Handle(LCommand);

        }

        [HttpPost("{ParkingName}/{PlaceNumber}/Take")]
        public async Task TakeParkingPlace([FromRoute] string ParkingName, int PlaceNumber)
        {

            var LCommand = new TakeParkingPlace
            {
                ParkingName = ParkingName,
                PlaceNumber = PlaceNumber
            };

            await FHandlerContext.CommandHandlers.Handle(LCommand);

        }

        [HttpPost("{ParkingName}/{PlaceNumber}/Leave")]
        public async Task LeaveParkingPlace([FromRoute] string ParkingName, int PlaceNumber)
        {

            var LCommand = new LeaveParking
            {
                ParkingName = ParkingName,
                PlaceNumber = PlaceNumber
            };

            await FHandlerContext.CommandHandlers.Handle(LCommand);

        }

    }

}
