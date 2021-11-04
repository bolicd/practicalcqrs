# Practical CQRS

Small CQRS/ES project created from scratch for demo purposes,
based on .NET Core 3.1, LocalDb, EventStore and TacticalDDD package version 1.26.

TacticalDDD is used only for ValueObject, Entity and AggregateRoot definitions to avoid project
bloat. 

https://www.nuget.org/packages/TacticalDDD/1.0.26

## How to start

First, make sure to run DbMigration project. This should create all relevant databases.
This is needed only the first time project is run. After that it is no longer required.

After that, 2 project can be run at the same time:
Hosts/RestAPI
Hosts/ProjectionHost

Once RestAPI is running it will navigate to set IISExpress Route and invoke swagger. 

Swagger is pretty self explanatory, but in short here we can create new person(after person is created,
ID is returned, use this ID to fetch given person) or we can update persons address.

In both cases event is stored into EventStore and eventually is projected into ReadModel( hence eventual consistency ).

ProjectionHost will pool event store for changes each second and apply required events to project them to readmodel if needed.


## Tests

I've tried to include test for each layer. 

TODO: Add repository tests



