# Restaurant Reservation System

A .NET console application built with Entity Framework Core and SQL Server to manage restaurants, customers, employees, tables, reservations, orders, menu items, and order items.

The project demonstrates Code First development, EF Core migrations, Fluent API configuration, data seeding, asynchronous repositories, LINQ queries, database views, a scalar database function, and a stored procedure.

---

## Solution Structure

The solution contains two projects:

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

## Configuration

The runtime connection string is stored in:

```text
RestaurantReservation/appsettings.json
```
The `appsettings.json` file is copied to the output directory during the build.

---

## Running the Project

1. Create the `RestaurantReservationCore` database.
2. Update the connection string in `appsettings.json`.
3. Apply the migrations:

```powershell
dotnet ef database update
```
Run the application:

```powershell
dotnet run
```
