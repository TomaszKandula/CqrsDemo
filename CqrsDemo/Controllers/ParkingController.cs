using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CqrsDemo.Models.Requests;
using CqrsDemo.Handlers.Queries.Models;
using CqrsDemo.Handlers.Commands.Models;
using MediatR;

namespace CqrsDemo.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class ParkingController : ControllerBase
    {

        private readonly IMediator FMediator;

        public ParkingController(IMediator AMediator) 
        {
            FMediator = AMediator;
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
                return StatusCode(500, LException.Message);
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
                return StatusCode(500, LException.Message);
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
                return StatusCode(500, LException.Message);
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
                return StatusCode(500, LException.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateParking([FromBody] CreateParkingRequest PayLoad)
        {

            try 
            {
                var LCommand = await FMediator.Send(new CreateParking 
                { 
                    ParkingName = PayLoad.ParkingName,
                    Capacity = PayLoad.Capacity
                });
                return StatusCode(200, LCommand);
            }
            catch (Exception LException)
            {
                return StatusCode(500, LException.Message);
            }

        }

        [HttpPost("{ParkingName}/Open")]
        public async Task<IActionResult> OpenParking([FromRoute] string ParkingName)
        {

            try
            {
                var LCommand = await FMediator.Send(new OpenParking
                {
                    ParkingName = ParkingName
                });
                return StatusCode(200, LCommand);
            }
            catch (Exception LException)
            {
                return StatusCode(500, LException.Message);
            }

        }

        [HttpPost("{ParkingName}/Close")]
        public async Task<IActionResult> CloseParking([FromRoute] string ParkingName)
        {

            try
            {
                var LCommand = await FMediator.Send(new CloseParking
                {
                    ParkingName = ParkingName
                });
                return StatusCode(200, LCommand);
            }
            catch (Exception LException)
            {
                return StatusCode(500, LException.Message);
            }

        }

        [HttpPost("{ParkingName}/{PlaceNumber}/Take")]
        public async Task<IActionResult> TakeParkingPlace([FromRoute] string ParkingName, int PlaceNumber)
        {

            try
            {
                var LCommand = await FMediator.Send(new TakeParkingPlace
                {
                    ParkingName = ParkingName,
                    PlaceNumber = PlaceNumber
                });
                return StatusCode(200, LCommand);
            }
            catch (Exception LException)
            {
                return StatusCode(500, LException.Message);
            }

        }

        [HttpPost("{ParkingName}/{PlaceNumber}/Leave")]
        public async Task<IActionResult> LeaveParkingPlace([FromRoute] string ParkingName, int PlaceNumber)
        {

            try
            {
                var LCommand = await FMediator.Send(new LeaveParkingPlace
                {
                    ParkingName = ParkingName,
                    PlaceNumber = PlaceNumber
                });
                return StatusCode(200, LCommand);
            }
            catch (Exception LException)
            {
                return StatusCode(500, LException.Message);
            }

        }

    }

}
