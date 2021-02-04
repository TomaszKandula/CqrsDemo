﻿using CqrsDemo.Models.Responses;
using MediatR;

namespace CqrsDemo.Handlers.Commands.Models
{
    public class TakeParkingPlace : IRequest<CommandResponse>
    {
        public string ParkingName { get; set; }

        public int PlaceNumber { get; set; }
    }
}
