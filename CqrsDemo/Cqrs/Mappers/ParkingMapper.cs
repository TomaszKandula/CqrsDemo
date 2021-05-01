using CqrsDemo.Shared.Dto;
using CqrsDemo.Cqrs.Handlers.Commands.OpenParking;
using CqrsDemo.Cqrs.Handlers.Commands.CloseParking;
using CqrsDemo.Cqrs.Handlers.Commands.CreateParking;
using CqrsDemo.Cqrs.Handlers.Commands.TakeParkingPlace;
using CqrsDemo.Cqrs.Handlers.Commands.LeaveParkingPlace;

namespace CqrsDemo.Cqrs.Mappers
{
    public static class ParkingMapper
    {
        public static OpenParkingCommand MapToOpenParkingCommand(OpenParkingDto AModel) 
        {
            return new OpenParkingCommand 
            { 
                ParkingName = AModel.ParkingName    
            };
        }

        public static CreateParkingCommand MapToCreateParkingCommand(CreateParkingDto AModel) 
        {
            return new CreateParkingCommand 
            { 
                ParkingName = AModel.ParkingName,
                Capacity = AModel.Capacity
            };
        }

        public static CloseParkingCommand MapToCloseParkingCommand(CloseParkingDto AModel) 
        {
            return new CloseParkingCommand 
            { 
                ParkingName = AModel.ParkingName
            };
        }

        public static LeaveParkingPlaceCommand MapToLeaveParkingPlaceCommand(LeaveParkingPlaceDto AModel) 
        {
            return new LeaveParkingPlaceCommand 
            { 
                ParkingName = AModel.ParkingName,
                PlaceNumber = AModel.PlaceNumber
            };
        }

        public static TakeParkingPlaceCommand MapToTakeParkingPlaceCommand(TakeParkingPlaceDto AModel) 
        {
            return new TakeParkingPlaceCommand 
            { 
                ParkingName = AModel.ParkingName,
                PlaceNumber = AModel.PlaceNumber
            };
        }
    }
}
