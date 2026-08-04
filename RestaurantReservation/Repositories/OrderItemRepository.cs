using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public class OrderItemRepository
{
    private readonly RestaurantReservationDbContext _dbContext;

    public OrderItemRepository(RestaurantReservationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<OrderItem>> GetAllAsync()
    {
        return await _dbContext.OrderItems
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<OrderItem?> GetByIdAsync(int orderItemId)
    {
        return await _dbContext.OrderItems
            .AsNoTracking()
            .FirstOrDefaultAsync(orderItem =>
                orderItem.OrderItemId == orderItemId);
    }

    public async Task<OrderItem> CreateAsync(OrderItem orderItem)
    {
        ArgumentNullException.ThrowIfNull(orderItem);

        await _dbContext.OrderItems.AddAsync(orderItem);
        await _dbContext.SaveChangesAsync();

        return orderItem;
    }

    public async Task<bool> UpdateAsync(OrderItem orderItem)
    {
        ArgumentNullException.ThrowIfNull(orderItem);

        OrderItem? existingOrderItem =
            await _dbContext.OrderItems.FindAsync(orderItem.OrderItemId);

        if (existingOrderItem is null)
        {
            return false;
        }

        existingOrderItem.OrderId = orderItem.OrderId;
        existingOrderItem.ItemId = orderItem.ItemId;
        existingOrderItem.Quantity = orderItem.Quantity;

        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int orderItemId)
    {
        OrderItem? orderItem =
            await _dbContext.OrderItems.FindAsync(orderItemId);

        if (orderItem is null)
        {
            return false;
        }

        _dbContext.OrderItems.Remove(orderItem);
        await _dbContext.SaveChangesAsync();

        return true;
    }
}