using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public class RestaurantRepository
{
    private readonly RestaurantReservationDbContext _dbContext;

    public RestaurantRepository(
        RestaurantReservationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Restaurant>> GetAllAsync()
    {
        return await _dbContext.Restaurants
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Restaurant?> GetByIdAsync(int restaurantId)
    {
        return await _dbContext.Restaurants
            .AsNoTracking()
            .FirstOrDefaultAsync(
                restaurant => restaurant.RestaurantId == restaurantId);
    }

    public async Task<Restaurant> CreateAsync(Restaurant restaurant)
    {
        ArgumentNullException.ThrowIfNull(restaurant);

        await _dbContext.Restaurants.AddAsync(restaurant);
        await _dbContext.SaveChangesAsync();

        return restaurant;
    }

    public async Task<bool> UpdateAsync(Restaurant restaurant)
    {
        ArgumentNullException.ThrowIfNull(restaurant);

        Restaurant? existingRestaurant =
            await _dbContext.Restaurants.FindAsync(
                restaurant.RestaurantId);

        if (existingRestaurant is null)
        {
            return false;
        }

        existingRestaurant.Name = restaurant.Name;
        existingRestaurant.Address = restaurant.Address;
        existingRestaurant.PhoneNumber = restaurant.PhoneNumber;
        existingRestaurant.OpeningHours = restaurant.OpeningHours;

        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int restaurantId)
    {
        Restaurant? restaurant =
            await _dbContext.Restaurants.FindAsync(restaurantId);

        if (restaurant is null)
        {
            return false;
        }

        _dbContext.Restaurants.Remove(restaurant);

        await _dbContext.SaveChangesAsync();

        return true;
    }
}