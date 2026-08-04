using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation.RepositoryDemos;

public static class MenuItemRepositoryDemo
{
    public static async Task RunAsync(
        RestaurantReservationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        MenuItemRepository repository = new(dbContext);

        DemoConsoleHelper.PrintDemoTitle(
            "Menu Item Repository Demo");

        await DemonstrateGetAllAsync(repository);

        MenuItem createdMenuItem =
            await DemonstrateCreateAsync(repository);

        await DemonstrateGetByIdAsync(
            repository,
            createdMenuItem.ItemId);

        await DemonstrateUpdateAsync(
            repository,
            createdMenuItem);

        await DemonstrateDeleteAsync(
            repository,
            createdMenuItem.ItemId);

        await DemonstrateGetByIdAfterDeleteAsync(
            repository,
            createdMenuItem.ItemId);

        DemoConsoleHelper.PrintDemoCompleted(
            "Menu Item Repository Demo");
    }

    private static async Task DemonstrateGetAllAsync(
        MenuItemRepository repository)
    {
        DemoConsoleHelper.PrintMethodTitle("GetAllAsync");

        List<MenuItem> menuItems =
            await repository.GetAllAsync();

        Console.WriteLine(
            $"Menu items count: {menuItems.Count}");

        foreach (MenuItem menuItem in menuItems)
        {
            PrintMenuItemSummary(menuItem);
        }
    }

    private static async Task<MenuItem> DemonstrateCreateAsync(
        MenuItemRepository repository)
    {
        DemoConsoleHelper.PrintMethodTitle("CreateAsync");

        MenuItem menuItem = new()
        {
            RestaurantId = 1,
            Name = "Temporary Menu Item",
            Description = "Temporary item used for repository demo",
            Price = 7.50m
        };

        MenuItem createdMenuItem =
            await repository.CreateAsync(menuItem);

        DemoConsoleHelper.PrintSuccess(
            "Menu item created successfully.");

        PrintMenuItemDetails(createdMenuItem);

        return createdMenuItem;
    }

    private static async Task DemonstrateGetByIdAsync(
        MenuItemRepository repository,
        int itemId)
    {
        DemoConsoleHelper.PrintMethodTitle("GetByIdAsync");

        MenuItem? menuItem =
            await repository.GetByIdAsync(itemId);

        if (menuItem is null)
        {
            DemoConsoleHelper.PrintNotFound("Menu item");
            return;
        }

        DemoConsoleHelper.PrintSuccess(
            "Menu item found successfully.");

        PrintMenuItemDetails(menuItem);
    }

    private static async Task DemonstrateUpdateAsync(
        MenuItemRepository repository,
        MenuItem menuItem)
    {
        DemoConsoleHelper.PrintMethodTitle("UpdateAsync");

        menuItem.Name = "Updated Temporary Menu Item";
        menuItem.Description =
            "Updated item used for repository demo";
        menuItem.Price = 9.25m;

        bool updateSucceeded =
            await repository.UpdateAsync(menuItem);

        if (!updateSucceeded)
        {
            DemoConsoleHelper.PrintFailure(
                "Menu item update failed.");

            return;
        }

        DemoConsoleHelper.PrintSuccess(
            "Menu item updated successfully.");

        MenuItem? updatedMenuItem =
            await repository.GetByIdAsync(menuItem.ItemId);

        if (updatedMenuItem is null)
        {
            DemoConsoleHelper.PrintNotFound(
                "Updated menu item");

            return;
        }

        PrintMenuItemDetails(updatedMenuItem);
    }

    private static async Task DemonstrateDeleteAsync(
        MenuItemRepository repository,
        int itemId)
    {
        DemoConsoleHelper.PrintMethodTitle("DeleteAsync");

        bool deleteSucceeded =
            await repository.DeleteAsync(itemId);

        if (deleteSucceeded)
        {
            DemoConsoleHelper.PrintSuccess(
                "Menu item deleted successfully.");
        }
        else
        {
            DemoConsoleHelper.PrintFailure(
                "Menu item deletion failed.");
        }
    }

    private static async Task DemonstrateGetByIdAfterDeleteAsync(
        MenuItemRepository repository,
        int itemId)
    {
        DemoConsoleHelper.PrintMethodTitle(
            "GetByIdAsync After Delete");

        MenuItem? menuItem =
            await repository.GetByIdAsync(itemId);

        if (menuItem is null)
        {
            DemoConsoleHelper.PrintSuccess(
                "Deletion confirmed. Menu item was not found.");

            return;
        }

        DemoConsoleHelper.PrintFailure(
            "Menu item still exists.");

        PrintMenuItemDetails(menuItem);
    }

    private static void PrintMenuItemSummary(
        MenuItem menuItem)
    {
        Console.WriteLine(
            $"Id: {menuItem.ItemId}, " +
            $"Name: {menuItem.Name}, " +
            $"Price: {menuItem.Price:F2}");
    }

    private static void PrintMenuItemDetails(
        MenuItem menuItem)
    {
        Console.WriteLine($"Id: {menuItem.ItemId}");
        Console.WriteLine(
            $"Restaurant Id: {menuItem.RestaurantId}");
        Console.WriteLine($"Name: {menuItem.Name}");
        Console.WriteLine(
            $"Description: {menuItem.Description ?? "No description"}");
        Console.WriteLine(
            $"Price: {menuItem.Price:F2}");
    }
}