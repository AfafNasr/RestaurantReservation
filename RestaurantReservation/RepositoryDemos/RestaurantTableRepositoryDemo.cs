using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation.RepositoryDemos;

public static class RestaurantTableRepositoryDemo
{
    public static async Task RunAsync(
        RestaurantReservationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        RestaurantTableRepository repository = new(dbContext);

        DemoConsoleHelper.PrintDemoTitle(
            "Restaurant Table Repository Demo");

        await DemonstrateGetAllAsync(repository);

        Table createdTable =
            await DemonstrateCreateAsync(repository);

        await DemonstrateGetByIdAsync(
            repository,
            createdTable.TableId);

        await DemonstrateUpdateAsync(
            repository,
            createdTable);

        await DemonstrateDeleteAsync(
            repository,
            createdTable.TableId);

        await DemonstrateGetByIdAfterDeleteAsync(
            repository,
            createdTable.TableId);

        DemoConsoleHelper.PrintDemoCompleted(
            "Restaurant Table Repository Demo");
    }

    private static async Task DemonstrateGetAllAsync(
        RestaurantTableRepository repository)
    {
        DemoConsoleHelper.PrintMethodTitle("GetAllAsync");

        List<Table> tables =
            await repository.GetAllAsync();

        Console.WriteLine($"Tables count: {tables.Count}");

        foreach (Table table in tables)
        {
            PrintTableSummary(table);
        }
    }

    private static async Task<Table> DemonstrateCreateAsync(
        RestaurantTableRepository repository)
    {
        DemoConsoleHelper.PrintMethodTitle("CreateAsync");

        Table table = new()
        {
            RestaurantId = 1,
            Capacity = 10
        };

       Table createdTable =
            await repository.CreateAsync(table);

        DemoConsoleHelper.PrintSuccess(
            "Restaurant table created successfully.");

        PrintTableDetails(createdTable);

        return createdTable;
    }

    private static async Task DemonstrateGetByIdAsync(
        RestaurantTableRepository repository,
        int tableId)
    {
        DemoConsoleHelper.PrintMethodTitle("GetByIdAsync");

        Table? table =
            await repository.GetByIdAsync(tableId);

        if (table is null)
        {
            DemoConsoleHelper.PrintNotFound("Restaurant table");
            return;
        }

        DemoConsoleHelper.PrintSuccess(
            "Restaurant table found successfully.");

        PrintTableDetails(table);
    }

    private static async Task DemonstrateUpdateAsync(
        RestaurantTableRepository repository,
        Table table)
    {
        DemoConsoleHelper.PrintMethodTitle("UpdateAsync");

        table.Capacity = 12;

        bool updateSucceeded =
            await repository.UpdateAsync(table);

        if (!updateSucceeded)
        {
            DemoConsoleHelper.PrintFailure(
                "Restaurant table update failed.");

            return;
        }

        DemoConsoleHelper.PrintSuccess(
            "Restaurant table updated successfully.");

        Table? updatedTable =
            await repository.GetByIdAsync(table.TableId);

        if (updatedTable is null)
        {
            DemoConsoleHelper.PrintNotFound(
                "Updated restaurant table");

            return;
        }

        PrintTableDetails(updatedTable);
    }

    private static async Task DemonstrateDeleteAsync(
        RestaurantTableRepository repository,
        int tableId)
    {
        DemoConsoleHelper.PrintMethodTitle("DeleteAsync");

        bool deleteSucceeded =
            await repository.DeleteAsync(tableId);

        if (deleteSucceeded)
        {
            DemoConsoleHelper.PrintSuccess(
                "Restaurant table deleted successfully.");
        }
        else
        {
            DemoConsoleHelper.PrintFailure(
                "Restaurant table deletion failed.");
        }
    }

    private static async Task DemonstrateGetByIdAfterDeleteAsync(
        RestaurantTableRepository repository,
        int tableId)
    {
        DemoConsoleHelper.PrintMethodTitle(
            "GetByIdAsync After Delete");

        Table? table =
            await repository.GetByIdAsync(tableId);

        if (table is null)
        {
            DemoConsoleHelper.PrintSuccess(
                "Deletion confirmed. Restaurant table was not found.");

            return;
        }

        DemoConsoleHelper.PrintFailure(
            "Restaurant table still exists.");

        PrintTableDetails(table);
    }

    private static void PrintTableSummary(
       Table table)
    {
        Console.WriteLine(
            $"Id: {table.TableId}, " +
            $"Restaurant Id: {table.RestaurantId}, " +
            $"Capacity: {table.Capacity}");
    }

    private static void PrintTableDetails(
        Table table)
    {
        Console.WriteLine($"Id: {table.TableId}");
        Console.WriteLine(
            $"Restaurant Id: {table.RestaurantId}");
        Console.WriteLine($"Capacity: {table.Capacity}");
    }
}