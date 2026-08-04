using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation.RepositoryDemos;

public static class EmployeeRepositoryDemo
{
    public static async Task RunAsync(
        RestaurantReservationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        EmployeeRepository repository = new(dbContext);

        DemoConsoleHelper.PrintDemoTitle(
            "Employee Repository Demo");

        await DemonstrateGetAllAsync(repository);

        Employee createdEmployee =
            await DemonstrateCreateAsync(repository);

        await DemonstrateGetByIdAsync(
            repository,
            createdEmployee.EmployeeId);

        await DemonstrateUpdateAsync(
            repository,
            createdEmployee);

        await DemonstrateListManagersAsync(repository);

        await DemonstrateDeleteAsync(
            repository,
            createdEmployee.EmployeeId);

        DemoConsoleHelper.PrintDemoCompleted(
            "Employee Repository Demo");
    }

    private static async Task DemonstrateGetAllAsync(
        EmployeeRepository repository)
    {
        DemoConsoleHelper.PrintMethodTitle("GetAllAsync");

        List<Employee> employees =
            await repository.GetAllAsync();

        Console.WriteLine($"Employees count: {employees.Count}");

        foreach (Employee employee in employees)
        {
            PrintEmployeeSummary(employee);
        }
    }

    private static async Task<Employee> DemonstrateCreateAsync(
        EmployeeRepository repository)
    {
        DemoConsoleHelper.PrintMethodTitle("CreateAsync");

        Employee employee = new()
        {
            RestaurantId = 1,
            FirstName = "Temporary",
            LastName = "Employee",
            Position = "Waiter"
        };

        Employee createdEmployee =
            await repository.CreateAsync(employee);

        DemoConsoleHelper.PrintSuccess(
            "Employee created successfully.");

        PrintEmployeeDetails(createdEmployee);

        return createdEmployee;
    }

    private static async Task DemonstrateGetByIdAsync(
        EmployeeRepository repository,
        int employeeId)
    {
        DemoConsoleHelper.PrintMethodTitle("GetByIdAsync");

        Employee? employee =
            await repository.GetByIdAsync(employeeId);

        if (employee is null)
        {
            DemoConsoleHelper.PrintNotFound("Employee");
            return;
        }

        DemoConsoleHelper.PrintSuccess(
            "Employee found successfully.");

        PrintEmployeeDetails(employee);
    }

    private static async Task DemonstrateUpdateAsync(
        EmployeeRepository repository,
        Employee employee)
    {
        DemoConsoleHelper.PrintMethodTitle("UpdateAsync");

        employee.FirstName = "Updated";
        employee.LastName = "Employee";
        employee.Position = "Manager";

        bool updateSucceeded =
            await repository.UpdateAsync(employee);

        if (!updateSucceeded)
        {
            DemoConsoleHelper.PrintFailure(
                "Employee update failed.");

            return;
        }

        DemoConsoleHelper.PrintSuccess(
            "Employee updated successfully.");

        Employee? updatedEmployee =
            await repository.GetByIdAsync(employee.EmployeeId);

        if (updatedEmployee is null)
        {
            DemoConsoleHelper.PrintNotFound("Updated employee");
            return;
        }

        PrintEmployeeDetails(updatedEmployee);
    }

    private static async Task DemonstrateListManagersAsync(
        EmployeeRepository repository)
    {
        DemoConsoleHelper.PrintMethodTitle("ListManagersAsync");

        List<Employee> managers =
            await repository.ListManagersAsync();

        Console.WriteLine($"Managers count: {managers.Count}");

        foreach (Employee manager in managers)
        {
            PrintEmployeeSummary(manager);
        }
    }

    private static async Task DemonstrateDeleteAsync(
        EmployeeRepository repository,
        int employeeId)
    {
        DemoConsoleHelper.PrintMethodTitle("DeleteAsync");

        bool deleteSucceeded =
            await repository.DeleteAsync(employeeId);

        if (deleteSucceeded)
        {
            DemoConsoleHelper.PrintSuccess(
                "Employee deleted successfully.");
        }
        else
        {
            DemoConsoleHelper.PrintFailure(
                "Employee deletion failed.");
        }
    }

    private static void PrintEmployeeSummary(Employee employee)
    {
        Console.WriteLine(
            $"Id: {employee.EmployeeId}, " +
            $"Name: {employee.FirstName} {employee.LastName}, " +
            $"Position: {employee.Position}");
    }

    private static void PrintEmployeeDetails(Employee employee)
    {
        Console.WriteLine($"Id: {employee.EmployeeId}");
        Console.WriteLine($"Restaurant Id: {employee.RestaurantId}");
        Console.WriteLine(
            $"Name: {employee.FirstName} {employee.LastName}");
        Console.WriteLine($"Position: {employee.Position}");
    }
}