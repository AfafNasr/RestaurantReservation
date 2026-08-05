using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Views;
using RestaurantReservation.Db.StoredProcedure;

namespace RestaurantReservation.Db.Repositories;

public class ViewRepository
{
    private readonly RestaurantReservationDbContext _dbContext;

    public ViewRepository(
        RestaurantReservationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ReservationDetailsView>> GetReservationDetailsAsync()
    {
        return await _dbContext.ReservationDetails
            .OrderBy(view => view.ReservationDate)
            .ToListAsync();
    }

    public async Task<List<EmployeeRestaurantView>> GetEmployeeRestaurantDetailsAsync()
    {
        return await _dbContext.EmployeeRestaurantDetails
            .OrderBy(view => view.RestaurantName)
            .ThenBy(view => view.EmployeeFirstName)
            .ThenBy(view => view.EmployeeLastName)
            .ToListAsync();
    }
    public async Task<decimal?> CalculateRestaurantRevenueAsync(
    int restaurantId)
    {
        return await _dbContext.Restaurants
            .AsNoTracking()
            .Where(restaurant =>
                restaurant.RestaurantId == restaurantId)
            .Select(restaurant =>
                (decimal?)RestaurantReservationDbContext
                    .CalculateRestaurantRevenue(
                        restaurant.RestaurantId))
            .FirstOrDefaultAsync();
    }

    public async Task<List<CustomerLargePartyReservationResult>>
    GetCustomersByMinimumPartySizeAsync(int minimumPartySize)
    {
        if (minimumPartySize < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(minimumPartySize),
                "Minimum party size cannot be negative.");
        }

        return await _dbContext.CustomerLargePartyReservations
            .FromSqlInterpolated(
                $"""
             EXEC dbo.sp_GetCustomersByMinimumPartySize
                 @MinimumPartySize = {minimumPartySize}
             """)
            .AsNoTracking()
            .ToListAsync();
    }
}