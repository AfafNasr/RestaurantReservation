using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public class RestaurantTableRepository
{
    private readonly RestaurantReservationDbContext _dbContext;

    public RestaurantTableRepository(RestaurantReservationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Table>> GetAllAsync()
    {
        return await _dbContext.Tables
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Table?> GetByIdAsync(int tableId)
    {
        return await _dbContext.Tables
            .AsNoTracking()
            .FirstOrDefaultAsync(table => table.TableId == tableId);
    }

    public async Task<Table> CreateAsync(Table table)
    {
        ArgumentNullException.ThrowIfNull(table);

        await _dbContext.Tables.AddAsync(table);
        await _dbContext.SaveChangesAsync();

        return table;
    }

    public async Task<bool> UpdateAsync(Table table)
    {
        ArgumentNullException.ThrowIfNull(table);

        Table? existingTable =
            await _dbContext.Tables.FindAsync(table.TableId);

        if (existingTable is null)
        {
            return false;
        }

        existingTable.RestaurantId = table.RestaurantId;
        existingTable.Capacity = table.Capacity;

        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int tableId)
    {
        Table? table =
            await _dbContext.Tables.FindAsync(tableId);

        if (table is null)
        {
            return false;
        }

        _dbContext.Tables.Remove(table);
        await _dbContext.SaveChangesAsync();

        return true;
    }
}