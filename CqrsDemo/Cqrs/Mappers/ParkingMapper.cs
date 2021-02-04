using CqrsDemo.Shared.Dto;
using CqrsDemo.Handlers.Commands.OpenParking;
using CqrsDemo.Handlers.Commands.CloseParking;
using CqrsDemo.Handlers.Commands.CreateParking;
using CqrsDemo.Handlers.Commands.TakeParkingPlace;
using CqrsDemo.Handlers.Commands.LeaveParkingPlace;

namespace CqrsDemo.Cqrs.Mappers
{
    public static class ParkingMapper
    {
        public static OpenParkingCommand MapToOpenParkingCommand(OpenParkingDto Model) 
        {
            return new OpenParkingCommand 
            { 
                ParkingName = Model.ParkingName    
            };
        }

        public static CreateParkingCommand MapToCreateParkingCommand(CreateParkingDto Model) 
        {
            return new CreateParkingCommand 
            { 
                ParkingName = Model.ParkingName,
                Capacity = Model.Capacity
            };
        }

        public static CloseParkingCommand MapToCloseParkingCommand(CloseParkingDto Model) 
        {
            return new CloseParkingCommand 
            { 
                ParkingName = Model.ParkingName
            };
        }

        public static LeaveParkingPlaceCommand MapToLeaveParkingPlaceCommand(LeaveParkingPlaceDto Model) 
        {
            return new LeaveParkingPlaceCommand 
            { 
                ParkingName = Model.ParkingName,
                PlaceNumber = Model.PlaceNumber
            };
        }

        public static TakeParkingPlaceCommand MapToTakeParkingPlaceCommand(TakeParkingPlaceDto Model) 
        {
            return new TakeParkingPlaceCommand 
            { 
                ParkingName = Model.ParkingName,
                PlaceNumber = Model.PlaceNumber
            };
        }
    }
}
