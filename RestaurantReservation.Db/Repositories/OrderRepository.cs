using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public class OrderRepository
{
    private readonly RestaurantReservationDbContext _dbContext;

    public OrderRepository(RestaurantReservationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Order>> GetAllAsync()
    {
        return await _dbContext.Orders
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(int orderId)
    {
        return await _dbContext.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(order => order.OrderId == orderId);
    }

    public async Task<Order> CreateAsync(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        await _dbContext.Orders.AddAsync(order);
        await _dbContext.SaveChangesAsync();

        return order;
    }

    public async Task<bool> UpdateAsync(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        Order? existingOrder =
            await _dbContext.Orders.FindAsync(order.OrderId);

        if (existingOrder is null)
        {
            return false;
        }

        existingOrder.ReservationId = order.ReservationId;
        existingOrder.EmployeeId = order.EmployeeId;
        existingOrder.OrderDate = order.OrderDate;
        existingOrder.TotalAmount = order.TotalAmount;

        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int orderId)
    {
        Order? order =
            await _dbContext.Orders.FindAsync(orderId);

        if (order is null)
        {
            return false;
        }

        _dbContext.Orders.Remove(order);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<List<Order>> ListOrdersAndMenuItemsAsync(
        int reservationId)
    {
        return await _dbContext.Orders
            .AsNoTracking()
            .Where(order => order.ReservationId == reservationId)
            .Include(order => order.OrderItems)
                .ThenInclude(orderItem => orderItem.MenuItem)
            .OrderBy(order => order.OrderDate)
            .ToListAsync();
    }

    public async Task<List<MenuItem>> ListOrderedMenuItemsAsync(
        int reservationId)
    {
        return await _dbContext.OrderItems
            .AsNoTracking()
            .Where(orderItem =>
                orderItem.Order.ReservationId == reservationId)
            .Select(orderItem => orderItem.MenuItem)
            .Distinct()
            .OrderBy(menuItem => menuItem.Name)
            .ToListAsync();
    }

    public async Task<decimal?> CalculateAverageOrderAmountAsync(
        int employeeId)
    {
        return await _dbContext.Orders
            .AsNoTracking()
            .Where(order => order.EmployeeId == employeeId)
            .Select(order => (decimal?)order.TotalAmount)
            .AverageAsync();
    }
}