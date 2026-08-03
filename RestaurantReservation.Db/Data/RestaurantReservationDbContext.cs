using Microsoft.EntityFrameworkCore;

namespace RestaurantReservation.Db.Data;

public class RestaurantReservationDbContext : DbContext
{
    public RestaurantReservationDbContext(
        DbContextOptions<RestaurantReservationDbContext> options)
        : base(options)
    {
    }
}