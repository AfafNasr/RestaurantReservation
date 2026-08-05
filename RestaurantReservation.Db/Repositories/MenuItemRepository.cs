using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public class MenuItemRepository
{
    private readonly RestaurantReservationDbContext _dbContext;

    public MenuItemRepository(RestaurantReservationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<MenuItem>> GetAllAsync()
    {
        return await _dbContext.MenuItems
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<MenuItem?> GetByIdAsync(int itemId)
    {
        return await _dbContext.MenuItems
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.ItemId == itemId);
    }

    public async Task<MenuItem> CreateAsync(MenuItem menuItem)
    {
        ArgumentNullException.ThrowIfNull(menuItem);

        await _dbContext.MenuItems.AddAsync(menuItem);
        await _dbContext.SaveChangesAsync();

        return menuItem;
    }

    public async Task<bool> UpdateAsync(MenuItem menuItem)
    {
        ArgumentNullException.ThrowIfNull(menuItem);

        MenuItem? existingItem =
            await _dbContext.MenuItems.FindAsync(menuItem.ItemId);

        if (existingItem is null)
        {
            return false;
        }

        existingItem.RestaurantId = menuItem.RestaurantId;
        existingItem.Name = menuItem.Name;
        existingItem.Description = menuItem.Description;
        existingItem.Price = menuItem.Price;

        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int itemId)
    {
        MenuItem? menuItem =
            await _dbContext.MenuItems.FindAsync(itemId);

        if (menuItem is null)
        {
            return false;
        }

        _dbContext.MenuItems.Remove(menuItem);
        await _dbContext.SaveChangesAsync();

        return true;
    }
}