using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public class EmployeeRepository
{
    private readonly RestaurantReservationDbContext _dbContext;

    public EmployeeRepository(RestaurantReservationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Employee>> GetAllAsync()
    {
        return await _dbContext.Employees
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Employee?> GetByIdAsync(int employeeId)
    {
        return await _dbContext.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(
                employee => employee.EmployeeId == employeeId);
    }

    public async Task<Employee> CreateAsync(Employee employee)
    {
        ArgumentNullException.ThrowIfNull(employee);

        await _dbContext.Employees.AddAsync(employee);
        await _dbContext.SaveChangesAsync();

        return employee;
    }

    public async Task<bool> UpdateAsync(Employee employee)
    {
        ArgumentNullException.ThrowIfNull(employee);

        Employee? existingEmployee =
            await _dbContext.Employees.FindAsync(employee.EmployeeId);

        if (existingEmployee is null)
        {
            return false;
        }

        existingEmployee.RestaurantId = employee.RestaurantId;
        existingEmployee.FirstName = employee.FirstName;
        existingEmployee.LastName = employee.LastName;
        existingEmployee.Position = employee.Position;

        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int employeeId)
    {
        Employee? employee =
            await _dbContext.Employees.FindAsync(employeeId);

        if (employee is null)
        {
            return false;
        }

        _dbContext.Employees.Remove(employee);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<List<Employee>> ListManagersAsync()
    {
        return await _dbContext.Employees
            .AsNoTracking()
            .Where(employee => employee.Position == "Manager")
            .OrderBy(employee => employee.FirstName)
            .ThenBy(employee => employee.LastName)
            .ToListAsync();
    }
}  