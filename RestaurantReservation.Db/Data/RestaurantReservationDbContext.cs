using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Views;

namespace RestaurantReservation.Db.Data;

public class RestaurantReservationDbContext : DbContext
{
    public RestaurantReservationDbContext(
        DbContextOptions<RestaurantReservationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Restaurant> Restaurants => Set<Restaurant>();

    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<Employee> Employees => Set<Employee>();

    public DbSet<Table> Tables => Set<Table>();

    public DbSet<Reservation> Reservations => Set<Reservation>();

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<MenuItem> MenuItems => Set<MenuItem>();

    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    public DbSet<ReservationDetailsView> ReservationDetails => Set<ReservationDetailsView>();

    public DbSet<EmployeeRestaurantView> EmployeeRestaurantDetails => Set<EmployeeRestaurantView>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(RestaurantReservationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}