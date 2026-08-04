using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Views;
using RestaurantReservation.Db.StoredProcedure;

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

    public static decimal CalculateRestaurantRevenue(int restaurantId)
    {
        throw new NotSupportedException(
            "This method can only be used inside an EF Core LINQ query.");
    }

    public DbSet<CustomerLargePartyReservationResult>
    CustomerLargePartyReservations =>
        Set<CustomerLargePartyReservationResult>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(RestaurantReservationDbContext).Assembly);

        modelBuilder
            .HasDbFunction(
                typeof(RestaurantReservationDbContext)
                    .GetMethod(
                           nameof(CalculateRestaurantRevenue),
                           new[] { typeof(int) })!)
            .HasName("fn_CalculateRestaurantRevenue")
            .HasSchema("dbo");

        base.OnModelCreating(modelBuilder);
    }
}