using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public class CustomerRepository
{
    private readonly RestaurantReservationDbContext _dbContext;

    public CustomerRepository(RestaurantReservationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        return await _dbContext.Customers
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Customer?> GetByIdAsync(int customerId)
    {
        return await _dbContext.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(
                customer => customer.CustomerId == customerId);
    }

    public async Task<Customer> CreateAsync(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);

        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.SaveChangesAsync();

        return customer;
    }

    public async Task<bool> UpdateAsync(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);

        Customer? existingCustomer =
            await _dbContext.Customers.FindAsync(customer.CustomerId);

        if (existingCustomer is null)
        {
            return false;
        }

        existingCustomer.FirstName = customer.FirstName;
        existingCustomer.LastName = customer.LastName;
        existingCustomer.Email = customer.Email;
        existingCustomer.PhoneNumber = customer.PhoneNumber;

        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int customerId)
    {
        Customer? customer =
            await _dbContext.Customers.FindAsync(customerId);

        if (customer is null)
        {
            return false;
        }

        _dbContext.Customers.Remove(customer);
        await _dbContext.SaveChangesAsync();

        return true;
    }
}