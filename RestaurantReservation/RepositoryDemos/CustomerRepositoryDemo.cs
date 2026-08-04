using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation.RepositoryDemos;

public static class CustomerRepositoryDemo
{
    public static async Task RunAsync(
        RestaurantReservationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        CustomerRepository repository = new(dbContext);

        DemoConsoleHelper.PrintDemoTitle(
            "Customer Repository Demo");

        await DemonstrateGetAllAsync(repository);

        Customer createdCustomer =
            await DemonstrateCreateAsync(repository);

        await DemonstrateGetByIdAsync(
            repository,
            createdCustomer.CustomerId);

        await DemonstrateUpdateAsync(
            repository,
            createdCustomer);

        await DemonstrateDeleteAsync(
            repository,
            createdCustomer.CustomerId);

        await DemonstrateGetByIdAfterDeleteAsync(
            repository,
            createdCustomer.CustomerId);

        DemoConsoleHelper.PrintDemoCompleted(
            "Customer Repository Demo");
    }

    private static async Task DemonstrateGetAllAsync(
        CustomerRepository repository)
    {
        DemoConsoleHelper.PrintMethodTitle("GetAllAsync");

        List<Customer> customers =
            await repository.GetAllAsync();

        Console.WriteLine($"Customers count: {customers.Count}");

        foreach (Customer customer in customers)
        {
            Console.WriteLine(
                $"Id: {customer.CustomerId}, " +
                $"Name: {customer.FirstName} {customer.LastName}");
        }
    }

    private static async Task<Customer> DemonstrateCreateAsync(
        CustomerRepository repository)
    {
        DemoConsoleHelper.PrintMethodTitle("CreateAsync");

        Customer customer = new()
        {
            FirstName = "Temporary",
            LastName = "Customer",
            Email = $"temporary.{Guid.NewGuid():N}@example.com",
            PhoneNumber = "0569111111"
        };

        Customer createdCustomer =
            await repository.CreateAsync(customer);

        DemoConsoleHelper.PrintSuccess(
            "Customer created successfully.");

        PrintCustomerDetails(createdCustomer);

        return createdCustomer;
    }

    private static async Task DemonstrateGetByIdAsync(
        CustomerRepository repository,
        int customerId)
    {
        DemoConsoleHelper.PrintMethodTitle("GetByIdAsync");

        Customer? customer =
            await repository.GetByIdAsync(customerId);

        if (customer is null)
        {
            DemoConsoleHelper.PrintNotFound("Customer");
            return;
        }

        DemoConsoleHelper.PrintSuccess(
            "Customer found successfully.");

        PrintCustomerDetails(customer);
    }

    private static async Task DemonstrateUpdateAsync(
        CustomerRepository repository,
        Customer customer)
    {
        DemoConsoleHelper.PrintMethodTitle("UpdateAsync");

        customer.FirstName = "Updated";
        customer.LastName = "Customer";
        customer.PhoneNumber = "0569222222";

        bool updateSucceeded =
            await repository.UpdateAsync(customer);

        if (!updateSucceeded)
        {
            DemoConsoleHelper.PrintFailure(
                "Customer update failed.");

            return;
        }

        DemoConsoleHelper.PrintSuccess(
            "Customer updated successfully.");

        Customer? updatedCustomer =
            await repository.GetByIdAsync(customer.CustomerId);

        if (updatedCustomer is null)
        {
            DemoConsoleHelper.PrintNotFound("Updated customer");
            return;
        }

        PrintCustomerDetails(updatedCustomer);
    }

    private static async Task DemonstrateDeleteAsync(
        CustomerRepository repository,
        int customerId)
    {
        DemoConsoleHelper.PrintMethodTitle("DeleteAsync");

        bool deleteSucceeded =
            await repository.DeleteAsync(customerId);

        if (deleteSucceeded)
        {
            DemoConsoleHelper.PrintSuccess(
                "Customer deleted successfully.");
        }
        else
        {
            DemoConsoleHelper.PrintFailure(
                "Customer deletion failed.");
        }
    }

    private static async Task DemonstrateGetByIdAfterDeleteAsync(
        CustomerRepository repository,
        int customerId)
    {
        DemoConsoleHelper.PrintMethodTitle(
            "GetByIdAsync After Delete");

        Customer? customer =
            await repository.GetByIdAsync(customerId);

        if (customer is null)
        {
            DemoConsoleHelper.PrintSuccess(
                "Deletion confirmed. Customer was not found.");

            return;
        }

        DemoConsoleHelper.PrintFailure(
            "Customer still exists.");
    }

    private static void PrintCustomerDetails(Customer customer)
    {
        Console.WriteLine($"Id: {customer.CustomerId}");
        Console.WriteLine(
            $"Name: {customer.FirstName} {customer.LastName}");
        Console.WriteLine($"Email: {customer.Email}");
        Console.WriteLine(
            $"Phone number: {customer.PhoneNumber}");
    }
}