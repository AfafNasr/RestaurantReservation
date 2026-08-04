using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RestaurantReservation.Db.Data;
using RestaurantReservation.RepositoryDemos;

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

string connectionString =
    configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "The connection string 'DefaultConnection' was not found.");

DbContextOptions<RestaurantReservationDbContext> options =
    new DbContextOptionsBuilder<RestaurantReservationDbContext>()
        .UseSqlServer(connectionString)
        .Options;

await using RestaurantReservationDbContext dbContext =
    new(options);

Console.WriteLine("Entity Framework Core configuration completed successfully.");

await RestaurantRepositoryDemo.RunAsync(dbContext);
await CustomerRepositoryDemo.RunAsync(dbContext);
await EmployeeRepositoryDemo.RunAsync(dbContext);
await RestaurantTableRepositoryDemo.RunAsync(dbContext);
await MenuItemRepositoryDemo.RunAsync(dbContext);
await ReservationRepositoryDemo.RunAsync(dbContext);
await OrderRepositoryDemo.RunAsync(dbContext);
await OrderItemRepositoryDemo.RunAsync(dbContext);

await ViewRepositoryDemo.RunAsync(dbContext);
