using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RestaurantReservation.Db.Data;

public class RestaurantReservationDbContextFactory
    : IDesignTimeDbContextFactory<RestaurantReservationDbContext>
{
    public RestaurantReservationDbContext CreateDbContext(string[] args)
    {
        DbContextOptions<RestaurantReservationDbContext> options =
            new DbContextOptionsBuilder<RestaurantReservationDbContext>()
                .UseSqlServer(
                    "Server=.;Database=RestaurantReservationCore;Trusted_Connection=True;TrustServerCertificate=True;")
                .Options;

        return new RestaurantReservationDbContext(options);
    }
}