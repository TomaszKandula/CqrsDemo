﻿using System;
using System.Collections.Generic;
using CqrsDemo.Infrastructure.Domain.Entities;

namespace CqrsDemo.UnitTests.Database
{
    public static class DummyLoad
    {
        public static IEnumerable<CommandStore> GetDummyCommands() 
        {
            return new List<CommandStore>
            {
                new CommandStore
                { 
                    Id = 1,
                    Type = "CreateParking",
                    Data = "{\"ParkingName\":\"Poznan Plaza\",\"Capacity\":2}",
                    CreatedAt = DateTime.Parse("2020-12-04 20:28:03"),
                    UserId = "30fb43bf-9689-4a16-b41f-75775d11a02f"
                },
                new CommandStore
                {
                    Id = 2,
                    Type = "CreateParking",
                    Data = "{\"ParkingName\":\"Parking-786359\",\"Capacity\":2}",
                    CreatedAt = DateTime.Parse("2020-12-04 20:28:03"),
                    UserId = "30fb43bf-9689-4a16-b41f-75775d11a02f"
                }
            };
        }

        public static IEnumerable<Parking> GetDummyParkingList() 
        {
            return new List<Parking>
            {
                new Parking
                { 
                    Name = "Poznan Plaza",
                    IsOpened = false
                },
                new Parking
                {
                    Name = "Parking-786359",
                    IsOpened = true,
                }
            };       
        }

        public static IEnumerable<ParkingPlace> GetDummyParkingPlaces() 
        {
            return new List<ParkingPlace>
            {
                new ParkingPlace
                { 
                    ParkingName = "Poznan Plaza",
                    Number = 1,
                    IsFree = true,
                    UserId = null,
                    ParkingNameNavigation = new Parking
                    {
                        Name = "Poznan Plaza",
                        IsOpened = true
                    }
                },
                new ParkingPlace
                {
                    ParkingName = "Poznan Plaza",
                    Number = 2,
                    IsFree = true,
                    UserId = null,
                    ParkingNameNavigation = new Parking
                    {
                        Name = "Poznan Plaza",
                        IsOpened = true
                    }
                },
                new ParkingPlace
                {
                    ParkingName = "Parking-786359",
                    Number = 3,
                    IsFree = true,
                    UserId = null,
                    ParkingNameNavigation = new Parking
                    {
                        Name = "Parking-786359",
                        IsOpened = true
                    }
                },
                new ParkingPlace
                {
                    ParkingName = "Parking-786359",
                    Number = 4,
                    IsFree = false,
                    UserId = null,
                    ParkingNameNavigation = new Parking
                    {
                        Name = "Parking-786359",
                        IsOpened = true
                    }
                }
            };       
        } 
    }
}
