# Architecutre & Framework
## We implemented clean architecture in this project.
Applications that follow the Dependency Inversion Principle as well as the Domain-Driven Design (DDD) principles tend to arrive at a similar architecture. This architecture has gone by many names over the years. One of the first names was Hexagonal Architecture, followed by Ports-and-Adapters. More recently, it's been cited as the Onion Architecture or Clean Architecture. 
Look at the picture below

![alt text](https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/media/image5-7.png "Clean Architecture")

Clean architecture puts the business logic and application model at the center of the application. Instead of having business logic depend on data access or other infrastructure concerns, this dependency is inverted: infrastructure and implementation details depend on the Application Core. This is achieved by defining abstractions, or interfaces, in the Application Core, which are then implemented by types defined in the Infrastructure layer.

In this diagram, dependencies flow toward the innermost circle. The Application Core takes its name from its position at the core of this diagram. And you can see on the diagram that the Application Core has no dependencies on other application layers. The application's entities and interfaces are at the very center. Just outside, but still in the Application Core, are domain services, which typically implement interfaces defined in the inner circle. Outside of the Application Core, both the UI and the Infrastructure layers depend on the Application Core, but not on one another (necessarily).

Figure below shows a more traditional horizontal layer diagram that better reflects the dependency between the UI and other layers.

![alt text](https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/media/image5-8.png "Clean Architecture")

Note that the solid arrows represent compile-time dependencies, while the dashed arrow represents a runtime-only dependency. With the clean architecture, the UI layer works with interfaces defined in the Application Core at compile time, and ideally shouldn't know about the implementation types defined in the Infrastructure layer. At run time, however, these implementation types are required for the app to execute, so they need to be present and wired up to the Application Core interfaces via dependency injection.

Figure below shows a more detailed view of an ASP.NET Core application's architecture when built following these recommendations.

![alt text](https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/media/image5-9.png "Clean Architecture")

Because the Application Core doesn't depend on Infrastructure, it's very easy to write automated unit tests for this layer. Figures 5-10 and 5-11 show how tests fit into this architecture.

![alt text](https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/media/image5-10.png "Clean Architecture")

![alt text](https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/media/image5-11.png "Clean Architecture")

## Organizing code in Clean Architecture
In a Clean Architecture solution, each project has clear responsibilities. As such, certain types belong in each project and you'll frequently find folders corresponding to these types in the appropriate project.

The Application Core holds the business model, which includes entities, services, and interfaces. These interfaces include abstractions for operations that will be performed using Infrastructure, such as data access, file system access, network calls, etc. Sometimes services or interfaces defined at this layer will need to work with non-entity types that have no dependencies on UI or Infrastructure. These can be defined as simple Data Transfer Objects (DTOs).

### Application Core types
* Entities (business model classes that are persisted)
* Interfaces
* Services
* DTOs

The Infrastructure project typically includes data access implementations. In a typical ASP.NET Core web application, these implementations include the Entity Framework (EF) DbContext, any EF Core Migration objects that have been defined, and data access implementation classes. The most common way to abstract data access implementation code is through the use of the [Repository design pattern](https://deviq.com/repository-pattern/).

In addition to data access implementations, the Infrastructure project should contain implementations of services that must interact with infrastructure concerns. These services should implement interfaces defined in the Application Core, and so Infrastructure should have a reference to the Application Core project.

### Infrastructure types
* EF Core types (DbContext, Migration)
* Data access implementation types (Repositories)
* Infrastructure-specific services (for example, FileLogger or SmtpNotifier)

The user interface layer in an ASP.NET Core MVC application is the entry point for the application. This project should reference the Application Core project, and its types should interact with infrastructure strictly through interfaces defined in Application Core. No direct instantiation of or static calls to the Infrastructure layer types should be allowed in the UI layer.

### UI layer types
* Controllers
* Filters
* Views
* ViewModels
* Startup

The Startup class is responsible for configuring the application, and for wiring up implementation types to interfaces, allowing dependency injection to work properly at run time.

# How To
* [How to Add new Entity Class (i.e Table)](#how-to-add-entity-class)
* [How to run migrations](#how-to-run-migrations)
* [How to Add New Repository](#how-to-add-new-repository)
* [How to Add New Service](#how-to-add-new-service)
* [How to Add new Controller](#how-to-add-new-controller)
* [How to add client side library](#how-to-add-client-side-library)
* [How to write UI or View](#how-to-write-ui-or-view)

## How to Add new Entity Class
#### To Create a new table in DB you have to go to 
##### Add Entity Class
* ApplicationCore Procet -> Entity Folder -> Add a new class there
* All Entity classes must inherit from BaseEntity
* Your Entity class must be public
```c#
public class User : BaseEntity 
{
  // Properties & Methods goes here
}
```

##### Add Configuration Class
* Then go to Infrastructure Project -> Data -> Configurations -> Add new Configuration Class. Class name should be suffixed with "Configuration". For example - User entity class should have a configuration named UserConfiguration.
* Namespace must be Infrastructure.Data.Config
* Must be a public class
```c#
namespace Infrastructure.Data.Config
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
      // Configuration codes goes here
    }
}
```

##### Add DbSet in *AppDbContext*
* Add related DbSet in AppDbContext
```c#
public class AppDbContext : DbContext
{
  // Other code
  public virtual DbSet<User> UserSet { get; set; }
  // Other Code
}
```

## How to run migrations
* In VS Open *Package Manager Console* 
* Select *Infrastructure* as default project
* Run this command
```
Add-Migration 001 -OutputDir "Data/Migrations"
```

> ##### Notes
> It is important to give a constraint name to all constraint.

## How to Add New Repository

## How to Add New Service

## How to Add new Controller
* Go to *Presentation* Project -> Controllers folder
  * If MVC view controller -  add it to to Controller folder directly. You must choos MVC controller in VS create controller template.
```c#
namespace Presentation.Controllers
{
  [ApiExplorerSettings(IgnoreApi = true)]
  [Route("[controller]/[action]")]
  public class AccountController : Controller
  {
    private readonly IEfRepository<User> _userRepository;

    public AccountController(
        IEfRepository<User> userRepository)
    {
      // Add more services from DI as you require
      _userRepository = userRepository;
    }

    // Controller Actions
    ...
  }
}
```
  * If Web API controller - Add the Controller to *Api* folder. However namespace should be *Presentation.Controllers*
```c#
namespace Presentation.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UsersController : ControllerBase
  {
    private readonly IEfRepository<User> _repository;

    public UsersController(IEfRepository<User> repository)
    {
      // Add more services from DI as you require
      _repository = repository;
    }

    // Controller Actions
    ...
  }
}
```
* All Controller Actions must decorated with HttpMethod i.e HttpGet, HttpPost, HttpPut, HttpDelete etc.
```c#
[HttpGet]
public async Task<IActionResult> Get(int offset = 0, int limit = 10, string filter = null, string sort = null, string order = null)
{
  // Rest of code goes here
}
```
* It is important to Add [ApiExplorerSettings(IgnoreApi = true)] in MVC controllers. This will prevent swagger documentation generation for Veiw Actions
```c#
namespace Presentation.Controllers
{
  [ApiExplorerSettings(IgnoreApi = true)]
  [Route("[controller]/[action]")]
  public class AccountController : Controller
  {
    // Controller Actions
    ...
  }
}
```

## How to add client side library
#### To add any library like *Bootstrap*
Right click on Presentation -> Add Client Script -> Write name of your script. Script will be installed in wwwroot/lib folder

## How to write UI or View
#### To Create a new UI or Veiw Page, depending on your requirements, you may need to follow these steps below.
* Add A Controller Action in your MVC controller
```c#
[HttpGet]
public IActionResult Login()
{
    return View();
}
```
* Add A Veiw in related folder. For example this Login View in AccountController should be in Presentation -> Views -> Account -> Login.cshtml
* Add a js file if required in Presentation -> wwwroot -> Account (Controller name) -> login.js