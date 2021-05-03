# CQRS pattern - basic example with MediatR

This repository is an example of possible CQRS pattern implementation (with no event sourcing) that uses a mediator pattern.

## Introduction

CQRS stands for _Command and Query Responsibility Segregation_. Following Martin Fowler ([see here](https://martinfowler.com/bliki/CQRS.html)), this pattern was created to split application logic into commands and queries; it promotes loosely coupled architecture that offers:

1. Ability to logging all of the queries and commands.
1. A separation between each command and query.
1. An isolation of each command and queries.
1. An explicit models for queries (read) and commands (write).
1. Ability to modify existing commands and queries without breaking the code.
1. Easy optimizations.

It is most suitable for large complex systems.

This repository holds a basic .NET Core application (back-end only) that complies with the below diagram (without SPA). Please note that it does not have event sourcing, but it uses [mediator pattern](https://refactoring.guru/design-patterns/mediator).

![cqrs](https://maindbstorage.blob.core.windows.net/tokanpages/content/drawings/cqrs_c4_model_v2.png)

This example of a Parking System was initially inspired by an article written by David Bottiau on CQRS ([see here](https://medium.com/@dbottiau/a-naive-introduction-to-cqrs-in-c-9d0d99cd2d54)). However, unlike David example, this example supposes to be a bit more robust; it uses __MediatR__ library, unit tests and integration tests are provided. The application is meant to be _clone, test and run_, once the connection string to a database is supplied.

Please note that this demo focuses on __CQRS__, and thus it keeps a simple structure (there is no split into separate services).

## Tech-stack (back-end)

1. Web API (NET Core 3.1 / C# language).
1. SQL Database, Entity Framework Core.
1. SeriLog for structural logging (sink to file).
1. Swagger-UI.
1. MediatR

Unit Tests and Integration Tests are provided using [XUnit](https://github.com/xunit/xunit) and [FluentAssertions](https://github.com/fluentassertions/fluentassertions).

## Setting-up the database

For testing, local SQL server/database is used, connection string have to be set up by replacing __set_env__ with a value of choice:

```
{
  "ConnectionStrings": 
  {
    "DbConnect": "set_env"
  }
}
```

Go to Package Manager Console (PMC) to execute the following command:

`Update-Database -StartupProject CqrsDemo -Project CqrsDemo -Context MainDbContext`

EF Core will create all the necessary tables and will seed test data. More on migrations here: [Infrastructure](https://github.com/TomaszKandula/CqrsDemo/tree/master/CqrsDemo/Infrastructure).

Please make sure your connection string points to an example database that has a user with the following permissions:

1. db_datareader,
1. db_datawriter,
1. db_owner.

## Integration Tests

Focuses on testing HTTP responses, dependencies and theirs configuration.

## Unit Tests

It covers all the logic used in the controllers (please note that the endpoints do not provide any business logic, they are only responsible for handling requests etc.). All dependencies are mocked/faked for mocking [Moq](https://github.com/moq/moq4) and [MockQueryable.Moq](https://github.com/romantitov/MockQueryable) have been used. 

## REST API

In this example, all controllers are public following the `async/await` pattern.

__Swagger-UI__ is added for easy API discovery: `/swagger`. Swagger JSON is also available: `/swagger/v1/swagger.json`.

## CQRS - MediatR, handlers

### A Query

Example of the controller using MediatR.

```csharp
[HttpGet]
public async Task<IEnumerable<GetAllParkingInfoQueryResult>> GetAllParkingInfo()
    => return await FMediator.Send(new GetAllParkingInfoQuery());
```

Depending on the use case, supplied object (`GetAllParkingInfo`) may carry the data. In this case, it is empty.

Query handler for `GetAllParkingInfo` follows:

```csharp
public class GetAllParkingInfoQueryHandler : IRequestHandler<GetAllParkingInfoQuery, IEnumerable<GetAllParkingInfoQueryResult>>
{
    private readonly MainDbContext FMainDbContext;

    public GetAllParkingInfoQueryHandler(MainDbContext AMainDbContext) 
        => FMainDbContext = AMainDbContext;

    public async Task<IEnumerable<GetAllParkingInfoQueryResult>> Handle(GetAllParkingInfoQuery ARequest, CancellationToken ACancellationToken) 
    {
        var LParkings = await FMainDbContext.Parking
            .Include(AParking => AParking.ParkingPlaces)
            .ToListAsync(ACancellationToken);

        var LSelection = LParkings.Select(AParking => new GetAllParkingInfoQueryResult
        {
            Name = AParking.Name,
            IsOpened = AParking.IsOpened,
            MaximumPlaces = AParking.ParkingPlaces.Count,
            AvailablePlaces = AParking.IsOpened
                ? AParking.ParkingPlaces.Count(AParkingPlace => AParkingPlace.IsFree)
                : 0
        });

        return LSelection;
    }
}
```

The handler returns a list of objects (`GetAllParkingInfoQueryResult`); we have used `IEnumerable` rather than `IList` so the compiler has a chance to optimise the code:

```csharp
public class GetAllParkingInfoQueryResult
{
    public string Name { get; set; }
    public bool IsOpened { get; set; }
    public int MaximumPlaces { get; set; }
    public int AvailablePlaces { get; set; }
}
```

### A Command

Example of controller:

```csharp
[HttpPost]
public async Task<Unit> CreateParking([FromBody] CreateParkingDto PayLoad)
    => return await FMediator.Send(ParkingMapper.MapToCreateParkingCommand(PayLoad));
```

Command handler (`CreateParkingCommandHandler`):

```csharp
public class CreateParkingCommandHandler : IRequestHandler<CreateParkingCommand, Unit>
{
    private readonly MainDbContext FMainDbContext;
    
    private readonly ICommands FCommandStore;

    public CreateParkingCommandHandler(MainDbContext AMainDbContext, ICommands ACommandStore) 
    {
        FMainDbContext = AMainDbContext;
        FCommandStore = ACommandStore;
    }

    public async Task<Unit> Handle(CreateParkingCommand ARequest, CancellationToken ACancellationToken)
    {
        var LPlaces = Enumerable.Range(1, ARequest.Capacity)
            .Select(ANumber =>
            {
                return new ParkingPlace
                {
                    ParkingName = ARequest.ParkingName,
                    Number = ANumber,
                    IsFree = true
                };
            })
            .ToList();

        var LParking = new Parking
        {
            Name = Request.ParkingName,
            IsOpened = true,
            ParkingPlaces = LPlaces
        };

        FMainDbContext.Add(LParking);

        await FMainDbContext.SaveChangesAsync(ACancellationToken);
        await FCommandStore.Push(ARequest, ACancellationToken);
        return await Task.FromResult(Unit.Value);
    }
}
```

Unlike query handler, all command handlers return `Unit.Value`.

## Command Sourcing

This example does not provide event sourcing but has an instance of command sourcing using SQL database. Thus each command execution is stored in a separate table (`CommandStore`). Therefore, it acts as a logging system for commands that can be retrieved and examined. One may also wish to read about [Event Sourcing](https://martinfowler.com/eaaDev/EventSourcing.html).

The service implements just one method (`push`):

```csharp
public virtual async Task Push(object ACommand, CancellationToken ACancellationToken = default)
{
    FMainDbContext.CommandStore.Add(
        new CommandStore
        {
            Type = ACommand.GetType().Name,
            Data = JsonConvert.SerializeObject(ACommand),
            CreatedAt = DateTime.Now,
            UserId = FAuthentication.GetUserId
        }
    );
    await FMainDbContext.SaveChangesAsync(ACancellationToken);
}
```
