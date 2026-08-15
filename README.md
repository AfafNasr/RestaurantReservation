# Restaurant Reservation System

A .NET solution built with Entity Framework Core, SQL Server, ASP.NET Core Minimal APIs, JWT authentication, OpenAPI/Swagger to manage restaurants, customers, employees, tables, reservations, orders, menu items, and order items.

The project demonstrates Code First development, EF Core migrations, Fluent API configuration, data seeding, asynchronous repositories, LINQ queries, database views, a scalar database function, and a stored procedure, REST APIs, JWT-based authorization, input validation, API documentation, and gRPC CRUD operations.

---

## Solution Structure

The solution contains three projects:

### `RestaurantReservation`

A console application responsible for:

- Reading the database connection string.
- Creating the `DbContext` at runtime.
- Calling repository methods.
- Demonstrating CRUD operations using sample data.
- Demonstrating the required LINQ queries.
- Querying database views.
- Calling the database function.
- Executing the stored procedure.
- Displaying all results in the console.

### `RestaurantReservation.Db`

A class library responsible for:

- Entity models.
- Entity Framework Core configurations.
- Migrations.
- Seed data.
- Database view models.
- Stored procedure models.

  ### `RestaurantReservation.API`

An ASP.NET Core Web API project responsible for exposing the application functionality through HTTP and gRPC endpoints.

It includes:

- Minimal API endpoints for reservation CRUD operations.
- Additional endpoints for managers, customer reservations, reservation orders, ordered menu items, and employee average order amounts.
- JWT authentication and authorization.
- Input validation and business-rule validation.
- Global error handling with user-friendly error responses.
- OpenAPI documentation with Swagger UI.
- gRPC CRUD operations for reservations.

---

## Database

The project uses a SQL Server database named:

```text
RestaurantReservationCore
```

The database contains the following tables:

- `Restaurants`
- `Customers`
- `Employees`
- `Tables`
- `Reservations`
- `Orders`
- `MenuItems`
- `OrderItems`

The database schema is created and updated through Entity Framework Core migrations.

---

## Entity Framework Core Configuration

The project uses Fluent API instead of placing Data Annotations inside the entity models.

A separate configuration class is provided for each entity.

The configurations define:

- Table names.
- Column names.
- Primary keys.
- Foreign keys.
- Required properties.
- Check constraints.
- Navigation relationships.
- Delete behaviors.
- Seed data.

All configuration classes are loaded automatically inside the `DbContext`:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.ApplyConfigurationsFromAssembly(
        typeof(RestaurantReservationDbContext).Assembly);

    base.OnModelCreating(modelBuilder);
}
```

---

## Delete Behavior

Most parent-child relationships use restricted deletion.

For example, a restaurant cannot be deleted while it is referenced by employees, tables, reservations, or menu items.

The relationship between `Order` and `OrderItem` uses cascade deletion because order items should not remain after their parent order is deleted.

---

## Design-Time DbContext Factory

The project contains:

```text
RestaurantReservationDbContextFactory
```

This class implements:

```csharp
IDesignTimeDbContextFactory<RestaurantReservationDbContext>
```

It allows Entity Framework Core tools to create a `DbContext` instance at design time.

It is used when running commands such as:

```powershell
dotnet ef migrations add
dotnet ef database update
```

The factory is needed because EF Core migration tools run separately from the normal console application and require a known way to create the `DbContext` with SQL Server options.

At runtime, the console application creates the `DbContext` using the connection string from `appsettings.json`.

---

## Migrations

Entity Framework Core migrations are used to manage database changes.

The project includes migrations for:

- Creating the initial database schema.
- Inserting seed data.
- Creating database views.
- Creating the scalar database function.
- Creating the stored procedure.

The initial migration creates:

- Tables.
- Columns.
- Primary keys.
- Foreign keys.
- Indexes.
- Constraints.
- Relationships.

The model snapshot stores the latest complete EF Core model and allows EF Core to detect future changes.

---

## Database Views, Functions, and Stored Procedures

EF Core automatically detects normal model changes such as tables, columns, keys, and relationships.

However, it does not automatically generate the SQL definitions for:

- Views.
- User-defined functions.
- Stored procedures.

Therefore, these database objects are created using custom SQL inside migrations:

The `Up` method creates the database object, while the `Down` method removes it.

---

## Data Seeding

Every table is seeded with at least five records using:

The seeded tables include:

- Five restaurants.
- Five customers.
- Five employees.
- Five restaurant tables.
- Five reservations.
- Five orders.
- Five menu items.
- Five order items.
---

## Repository Pattern

The project contains one repository for each entity:

- `RestaurantRepository`
- `CustomerRepository`
- `EmployeeRepository`
- `RestaurantTableRepository`
- `ReservationRepository`
- `OrderRepository`
- `MenuItemRepository`
- `OrderItemRepository`

A separate `ViewRepository` is used for:

- Database view queries.
- The database function call.
- Stored procedure execution.

Each repository receives the `DbContext` through its constructor.

---

## CRUD Operations

Each entity repository contains asynchronous CRUD methods:

```text
GetAllAsync
GetByIdAsync
CreateAsync
UpdateAsync
DeleteAsync
```
Update methods first retrieve the existing tracked entity, update the required properties, and save the changes.

---

## Asynchronous Operations

All database operations are asynchronous.

This prevents blocking the application thread while database operations are being completed.

---

## Required LINQ Methods

The project implements all required LINQ methods.

### `ListManagersAsync`

Returns all employees whose position is `Manager`.

### `GetReservationsByCustomerAsync`

Returns all reservations created by a specific customer.

### `ListOrdersAndMenuItemsAsync`

Returns orders for a reservation together with their order items and menu items.

### `ListOrderedMenuItemsAsync`

Returns only the distinct menu items ordered for a reservation.

### `CalculateAverageOrderAmountAsync`

Calculates the average order amount handled by a specific employee.

---

## Database Views

The project contains two SQL Server views.

### `dbo.vw_ReservationsWithDetails`

This view returns reservation information together with:

- Customer identifier.
- Customer first name.
- Customer last name.
- Restaurant identifier.
- Restaurant name.
- Reservation date.
- Party size.

### `dbo.vw_EmployeesWithRestaurant`

This view returns employee information together with:

- Employee name.
- Employee position.
- Restaurant identifier.
- Restaurant name.
- Restaurant address.

---

## Querying Views with EF Core

The database views are mapped to keyless entity types:

- `ReservationDetailsView`
- `EmployeeRestaurantView`

They are treated like read-only tables from the EF Core query perspective.

They can be queried using asynchronous LINQ:

```csharp
return await _dbContext.ReservationDetails
    .OrderBy(view => view.ReservationDate)
    .ToListAsync();
```
---

## Database Function

The project contains the scalar function:

```text
dbo.fn_CalculateRestaurantRevenue
```

It accepts a restaurant identifier and calculates the total revenue generated by that restaurant.

The calculation follows this relationship:

```text
Restaurant
→ Reservations
→ Orders
→ SUM(total_amount)
```
If a restaurant has no orders, the function returns zero.

---

## Stored Procedure

The project contains the stored procedure:

```text
dbo.sp_GetCustomersByMinimumPartySize
```

It accepts:

```text
@MinimumPartySize
```

It returns customers who have reservations with a party size greater than the provided value.

The result includes:

- Customer identifier.
- Customer name.
- Email.
- Phone number.
- Reservation identifier.
- Reservation date.
- Party size.
- Restaurant identifier.
- Restaurant name.

This ensures that the input value is sent as a SQL parameter instead of being concatenated into the SQL command.

---

## Console Demonstrations

The console application demonstrates all implemented methods using sample data.

The demonstrations are placed in separate files under:

```text
RepositoryDemos
```

This keeps `Program.cs` small and organized.

---
---

## REST API

The `RestaurantReservation.API` project exposes the database functionality through ASP.NET Core Minimal APIs.

### Reservation CRUD Endpoints

```text
GET    /api/reservations
GET    /api/reservations/{id}
POST   /api/reservations
PUT    /api/reservations/{id}
DELETE /api/reservations/{id}
```

### Additional Endpoints

```text
GET /api/employees/managers
GET /api/reservations/customer/{customerId}
GET /api/reservations/{reservationId}/orders
GET /api/reservations/{reservationId}/menu-items
GET /api/employees/{employeeId}/average-order-amount
```

The API reuses the existing Entity Framework Core repositories and `RestaurantReservationDbContext` from the `RestaurantReservation.Db` project.

---

## Authentication and Authorization

The API is secured using JWT Bearer authentication.

A login endpoint is provided to generate an access token:

```text
POST /api/auth/login
```

Protected endpoints require the JWT to be sent using the `Authorization` header:

```text
Authorization: Bearer <token>
```

JWT validation includes:

- Issuer validation.
- Audience validation.
- Token lifetime validation.
- Signing key validation.

The JWT signing key is stored using .NET User Secrets during development instead of being committed to the repository.

---

## Validation and Error Handling

The API uses built-in .NET validation with Data Annotations for request validation.

Validation includes:

- Positive identifiers.
- Positive party size.
- Customer existence.
- Restaurant existence.
- Table existence.
- Ensuring that the selected table belongs to the specified restaurant.
- Ensuring that the party size does not exceed the table capacity.

The API provides user-friendly responses for common errors:

```text
400 Bad Request
401 Unauthorized
404 Not Found
500 Internal Server Error
```

A global exception handler is used for unexpected errors.

---

## OpenAPI and Swagger

The API uses OpenAPI for API documentation and Swagger UI for interactive exploration and testing.

The documentation includes:

- Endpoint descriptions.
- Route parameters.
- Request bodies.
- Expected responses.
- Possible HTTP status codes.

Swagger UI is available in the Development environment at:

```text
/swagger
```

---

## API Testing

The REST API endpoints were manually tested using:

- Postman.
- Swagger UI.

Testing covered successful requests as well as common validation, authentication, and error scenarios.

---

## gRPC Bonus

The project also includes a gRPC implementation of the main reservation CRUD operations.

The gRPC contract is defined in:

```text
RestaurantReservation.API/Protos/reservation.proto
```

The gRPC service implementation is located in:

```text
RestaurantReservation.API/Grpc/ReservationGrpcService.cs
```

The service supports:

```text
GetAllReservations
GetReservationById
CreateReservation
UpdateReservation
DeleteReservation
```

The gRPC service uses the same Entity Framework Core `DbContext` and SQL Server database as the REST API.

The gRPC operations were manually tested using Postman's gRPC client.

---

## Running the Project

### 1. Configure the Database

Create the `RestaurantReservationCore` database and update the connection string in `appsettings.json`.

### 2. Apply Migrations

```powershell
dotnet ef database update
```

### 3. Run the Console Application

```powershell
dotnet run --project RestaurantReservation
```

### 4. Configure the JWT Secret

Set the JWT signing key using .NET User Secrets:

```powershell
dotnet user-secrets set "Jwt:Key" "your-secret-key" --project RestaurantReservation.API
```

### 5. Run the Web API

```powershell
dotnet run --project RestaurantReservation.API 
```

The REST API can be explored and tested using Swagger UI or Postman.

The gRPC reservation service can also be tested using Postman's gRPC .
