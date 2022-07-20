Requirements:

•	NET 6
•	Node v16.15.1 or equivalent to use Angular 13



Steps (cmd or similar):

1-	To download the project from repository run:

    git clone https://github.com/djdiazdiego/MusalaTest.git

2-	Go to the MusalaTest.DoItFast.WebApi.AppClient folder for restore de the Client App            dependencies and run:    

    npm install

3-	Go to MusalaTest folder for restore de the Server App dependencies and run and run:

    dotnet restore

4-	Go to the MusalaTest. DoItFast.WebApi.AppClient to start the Client App and run:

    npm run start

5-	Inside the project in appsettings.js, change the ConnectionStrings to your credentials.

6-	Run Start the project with the Kestrel server and not with IIS Express. Make sure to launch the app with these settings in the launch profile:

    https://localhost:7272;http://localhost:5272 



General Description:

•	It is a project based on a Domain Driven Design architecture and with CQRS patterns, it uses the repository pattern as a form of data persistence and the UnitOfWork pattern to centralize connections to the database.

•	AutoMapper is used as object-to-object mapper, FluentValidation for validations and MediatR as mediator between Controller-Validation-Command and Controller-Validation-Query data transfers.

•	Two contexts are used as the basic principle of CQRS, DbContextRead for reading and DbContextWrite for reading and writing.

•	MSSQL is used as the database. Migrations are applied automatically when you start the project.



Domain Layer:

•	Our abstractions (Domain.Core) and our data model (Domain) are defined, they use .NETStandard by design principle.

•	The domain is defined as a single aggregate root Gateway (AggregateRoot), which integrates a PeripheralDevice entity (Entity) and a PeripheralDeviceStatus enumeration entity (Enumeration); the latter presents a seed that automatically captures the data in the database (the seed is located in the data persistence layer (infrastructure)).



Infrastructure Layer:

•	It is made up of Infrastructure.Persistence and Infrastructure.Shared (Infrastructure.Identity was not used due to lack of time to be integrated), they use .NET 6.

•	It is the data persistence layer, here is the implementation of our DbContext, Repositories, Seeds, UnitOfWork, EntityFramework configurations on domain models and migrations.

•	DbSet and EntityFramework configurations on domain models are loaded dynamically.

•	Both read and read/write repositories are also dynamically loaded.



Application Layer:
•	It is constituted by WebApi and Application, they use .Net 6.
•	Within Application are the Commands and Queries, as well as the Handlers and Validations for them. We also have our DTOs models, which have the function of sharing data with the Client Application.
•	In Behavior we have the intermediate code in charge of validating our commands and queries. In MappingConverters some custom mappings, ApiMessages hosts the application's error messages. In Services are the custom services used by the application (currently the services found in Services are not in use, I did not have time to integrate authorization and authentication).
•	In WebApi we have our Controllers and Middleware, as well as the implementation of the Client application. 


