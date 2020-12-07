# CQRS pattern - basic example with MediatR

## Introduction

CQRS stands for _Command and Query Responsibility Segregation_. Following Martin Fowler ([see here](https://martinfowler.com/bliki/CQRS.html)), this pattern was created to split application logic into commands and queries; it promotes lossely coupled architecture that offers:

1. Ability to logging all of the queries and commands.
1. A separation between each command and query.
1. An isolation of each commands and queries.
1. A clear models for queries (read) and commands (write).
1. Ability to modify existing commands and queries without breaking the code.
1. Easy optimizations.

It is most suitable for large complex systems.

This repositorium holds basic .NET Core application (back-end only) that complies to the below diagram (without SPA). Please note that it does not have event sourcing, but it uses [mediator pattern](https://refactoring.guru/design-patterns/mediator).

![cqrs](https://maindbstorage.blob.core.windows.net/tokanpages/content/drawings/cqrs_c4_model.png)

This example of Parking System was initially inspired by article written by David Bottiau on CQRS ([see here](https://medium.com/@dbottiau/a-naive-introduction-to-cqrs-in-c-9d0d99cd2d54)). However, unlike David example, this example suppose to be a bit more robust, it uses __MediatR__ library and the unit tests and integration tests are provided, also, the application is meant to be _clone, test and run_, once the connection string to database is supplied.

## Tech-stack (back-end)

1. Web API (NET Core 3.1 / C# language).
1. SQL Database, Entity Framework Core.
1. SeriLog for structural logging (sink to file).
1. Swagger-UI.
1. MediatR

Unit Tests and Integration Tests are provided using [XUnit](https://github.com/xunit/xunit) and [FluentAssertions](https://github.com/fluentassertions/fluentassertions).

## Setting-up the database

For testing, local SQL server/database is used, connection string have to be setup in __secrets.json__. Copy content from __appsettings.Development.json__ and replace __set_env__ with proper values:

```
{

  "ConnectionStrings": 
  {
    "DbConnect": "set_env"
  }

}
```

In Package Manager Console (PMC) type and execute command:

`update-database`

And EF Core will create database with all necessary tables. Then, we may populate the tables with the script (on localhost).

## Integration Tests

Focuses on testing HTTP responses, dependencies and theirs configuration.

## Unit Tests

Covers all the logic used in the controllers (please note that the endpoints does not provide any business logic, they are only responsible for handling requests etc.). All dependencies are mocked/faked. For mocking [Moq](https://github.com/moq/moq4) and [MockQueryable.Moq](https://github.com/romantitov/MockQueryable) have been used. 

## REST API

In this example all controllers are public following `async/await` pattern.

__Swagger-UI__ is added for easy API discovery: `/swagger`. Swagger JSON is also available: `/swagger/v1/swagger.json`.

## CQRS - MediatR, handlers

### A Query

Example of controller using MediatR.

```csharp
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
```

Depending on use case, supplied object (`GetAllParkingInfo`) may carry the data. In this case, it is empty.

Query handler for `GetAllParkingInfo` follows:

```csharp
public async Task<IEnumerable<ParkingInfo>> Handle(GetAllParkingInfo Request, CancellationToken CancellationToken) 
{
    var LParkings = await FMainDbContext.Parking
        .Include(AParking => AParking.ParkingPlaces)
        .ToListAsync();

    return LParkings.Select(AParking =>
    {
        return new ParkingInfo
        {
            Name = AParking.Name,
            IsOpened = AParking.IsOpened,
            MaximumPlaces = AParking.ParkingPlaces.Count,
            AvailablePlaces = AParking.IsOpened ? AParking.ParkingPlaces
                .Where(AParkingPlace => AParkingPlace.IsFree)
                .Count() : 0
        };
    });
}
```

Handler returns list of objects (`ParkingInfo`), we have used `IEnumerable` rather than `IList` so the compiler have a chance to optimise the code.

### A Command

Example of controller:

```csharp
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
```

In case of commands, because it is a _call to action_, there is no data in the body, so we use arguments from the route.

Command handler (`HandleTakeParkingPlace`):

```csharp
public async Task<CommandResponse> Handle(TakeParkingPlace Request, CancellationToken CancellationToken)
{
    var LParking = (await FMainDbContext.Parking
        .ToListAsync())
        .FirstOrDefault(p => p.Name == Request.ParkingName);

    if (LParking == null)
        return new CommandResponse 
        { 
            IsSucceeded = false,
            ErrorCode = "",
            ErrorDesc = $"Cannot find parking '{Request.ParkingName}'."
        };

    if (!LParking.IsOpened)
        return new CommandResponse 
        { 
            IsSucceeded = false,
            ErrorCode = "",
            ErrorDesc = $"The parking '{Request.ParkingName}' is closed."
        };

    var LParkingPlace = (await FMainDbContext.ParkingPlaces
        .ToListAsync())
        .FirstOrDefault(p => p.ParkingName == Request.ParkingName && p.Number == Request.PlaceNumber);

    if (LParkingPlace == null)
        return new CommandResponse 
        { 
            IsSucceeded = false,
            ErrorCode = "no_such_place",
            ErrorDesc = $"Cannot find place #{Request.PlaceNumber} in the parking '{Request.ParkingName}'."
        };

    if (!LParkingPlace.IsFree)
        return new CommandResponse
        {
            IsSucceeded = false,
            ErrorCode = "parking_taken",
            ErrorDesc = $"Parking place #{Request.PlaceNumber} is already taken."
        };

    LParkingPlace.IsFree = false;
    LParkingPlace.UserId = FAuthentication.GetUserId;

    await FMainDbContext.SaveChangesAsync();
    await FCommandStore.Push(Request);
    return new CommandResponse { IsSucceeded = true };
}
```

Unlike query handler, all command handlers have one common response model (`CommandResponse`):

```csharp
public class CommandResponse
{
    [JsonPropertyName("isSucceeded")]
    public bool IsSucceeded { get; set; }
    [JsonPropertyName("errorCode")]
    public string ErrorCode { get; set; } = "no_errors";
    [JsonPropertyName("errorDesc")]
    public string ErrorDesc { get; set; } = "n/a";
}
```

## Command Sourcing

This example does not provide event sourcing, but have example of command sourcing using SQL database, thus each command execution is stored in separate table (`CommandStore`). Therefore, it acts as a logging system for commands that can be retrieved and examined if necessary, however, in real world application this may be implemented with logger of choice (SeriLog + Sentry etc.).

The service implements just one method (`push`):

```csharp
public virtual async Task Push(object ACommand)
{
    FMainDbContext.CommandStore.Add(
        new CommandStore
        {
            Type      = ACommand.GetType().Name,
            Data      = JsonConvert.SerializeObject(ACommand),
            CreatedAt = DateTime.Now,
            UserId    = FAuthentication.GetUserId
        }
    );
    await FMainDbContext.SaveChangesAsync();
}
```
