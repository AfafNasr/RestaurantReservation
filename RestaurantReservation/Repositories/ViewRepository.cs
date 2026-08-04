using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Views;

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
}