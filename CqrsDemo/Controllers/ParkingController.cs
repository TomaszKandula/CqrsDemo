using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CqrsDemo.Handlers.Queries.Models;
using AutoMapper;
using MediatR;

namespace CqrsDemo.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class ParkingController : ControllerBase
    {

        private readonly IMapper FMapper;
        private readonly IMediator FMediator;

        public ParkingController(IMediator AMediator, IMapper AMapper) 
        {
            FMediator = AMediator;
            FMapper = AMapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllParkingInfo()
        {

            try 
            {
                var LQuery = await FMediator.Send(new GetAllParkingInfo());
                return StatusCode(200, LQuery);
            }
            catch (Exception LException)
            {
                return StatusCode(400, LException.Message);
            }

        }

        [HttpGet("{ParkingName}")]
        public async Task<IActionResult> GetParkingInfo([FromRoute] string ParkingName)
        {

            try
            {
                var LQuery = await FMediator.Send(new GetParkingInfo
                {
                    ParkingName = ParkingName
                });
                return StatusCode(200, LQuery);
            }
            catch (Exception LException)
            {
                return StatusCode(400, LException.Message);
            }

        }

        [HttpGet("AvailablePlaces/Count")]
        public async Task<IActionResult> GetTotalAvailablePlaces()
        {

            try
            {
                var LQuery = await FMediator.Send(new GetTotalAvailablePlaces());
                return StatusCode(200, LQuery);
            }
            catch (Exception LException)
            {
                return StatusCode(400, LException.Message);
            }

        }

        [HttpGet("AvailablePlaces/Random")]
        public async Task<IActionResult> GetRandomAvailablePlace()
        {

            try
            {
                var LQuery = await FMediator.Send(new GetRandomAvailablePlace());
                return StatusCode(200, LQuery);
            }
            catch (Exception LException)
            {
                return StatusCode(400, LException.Message);
            }

        }

        //[HttpPost]
        //public async Task CreateParking([FromBody] CreateParkingRequest PayLoad)
        //{

        //    //var LCommand = new CreateParking
        //    //{
        //    //    ParkingName = PayLoad.ParkingName,
        //    //    Capacity = PayLoad.Capacity
        //    //};

        //    //await FHandlerContext.CommandHandlers.Handle(LCommand);

        //}

        //[HttpPost("{ParkingName}/Open")]
        //public async Task OpenParking([FromRoute] string ParkingName)
        //{

        //    //var LCommand = new OpenParking 
        //    //{ 
        //    //    ParkingName = ParkingName 
        //    //};

        //    //await FHandlerContext.CommandHandlers.Handle(LCommand);

        //}

        //[HttpPost("{ParkingName}/Close")]
        //public async Task CloseParking([FromRoute] string ParkingName)
        //{

        //    //var LCommand = new CloseParking 
        //    //{ 
        //    //    ParkingName = ParkingName 
        //    //};

        //    //await FHandlerContext.CommandHandlers.Handle(LCommand);

        //}

        //[HttpPost("{ParkingName}/{PlaceNumber}/Take")]
        //public async Task TakeParkingPlace([FromRoute] string ParkingName, int PlaceNumber)
        //{

        //    //var LCommand = new TakeParkingPlace
        //    //{
        //    //    ParkingName = ParkingName,
        //    //    PlaceNumber = PlaceNumber
        //    //};

        //    //await FHandlerContext.CommandHandlers.Handle(LCommand);

        //}

        //[HttpPost("{ParkingName}/{PlaceNumber}/Leave")]
        //public async Task LeaveParkingPlace([FromRoute] string ParkingName, int PlaceNumber)
        //{

        //    //var LCommand = new LeaveParking
        //    //{
        //    //    ParkingName = ParkingName,
        //    //    PlaceNumber = PlaceNumber
        //    //};

        //    //await FHandlerContext.CommandHandlers.Handle(LCommand);

        //}

    }

}
