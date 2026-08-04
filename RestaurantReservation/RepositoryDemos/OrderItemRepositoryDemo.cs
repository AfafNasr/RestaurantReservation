using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation.RepositoryDemos;

public static class OrderItemRepositoryDemo
{
    public static async Task RunAsync(
        RestaurantReservationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        OrderItemRepository orderItemRepository =
            new(dbContext);

        MenuItemRepository menuItemRepository =
            new(dbContext);

        DemoConsoleHelper.PrintDemoTitle(
            "Order Item Repository Demo");

        await DemonstrateGetAllAsync(orderItemRepository);

        MenuItem temporaryMenuItem =
            await CreateTemporaryMenuItemAsync(
                menuItemRepository);

        OrderItem createdOrderItem =
            await DemonstrateCreateAsync(
                orderItemRepository,
                temporaryMenuItem.ItemId);

        await DemonstrateGetByIdAsync(
            orderItemRepository,
            createdOrderItem.OrderItemId);

        await DemonstrateUpdateAsync(
            orderItemRepository,
            createdOrderItem);

        await DemonstrateDeleteAsync(
            orderItemRepository,
            createdOrderItem.OrderItemId);

        await DemonstrateGetByIdAfterDeleteAsync(
            orderItemRepository,
            createdOrderItem.OrderItemId);

        await DeleteTemporaryMenuItemAsync(
            menuItemRepository,
            temporaryMenuItem.ItemId);

        DemoConsoleHelper.PrintDemoCompleted(
            "Order Item Repository Demo");
    }

    private static async Task DemonstrateGetAllAsync(
        OrderItemRepository repository)
    {
        DemoConsoleHelper.PrintMethodTitle("GetAllAsync");

        List<OrderItem> orderItems =
            await repository.GetAllAsync();

        Console.WriteLine(
            $"Order items count: {orderItems.Count}");

        foreach (OrderItem orderItem in orderItems)
        {
            PrintOrderItemSummary(orderItem);
        }
    }

    private static async Task<MenuItem>
        CreateTemporaryMenuItemAsync(
            MenuItemRepository repository)
    {
        DemoConsoleHelper.PrintMethodTitle(
            "Create Temporary Menu Item");

        MenuItem menuItem = new()
        {
            RestaurantId = 1,
            Name = "Temporary Order Item Product",
            Description =
                "Used only by OrderItem repository demo",
            Price = 5.00m
        };

        MenuItem createdMenuItem =
            await repository.CreateAsync(menuItem);

        DemoConsoleHelper.PrintSuccess(
            "Temporary menu item created.");

        Console.WriteLine(
            $"Menu Item Id: {createdMenuItem.ItemId}");

        return createdMenuItem;
    }

    private static async Task<OrderItem>
        DemonstrateCreateAsync(
            OrderItemRepository repository,
            int itemId)
    {
        DemoConsoleHelper.PrintMethodTitle("CreateAsync");

        OrderItem orderItem = new()
        {
            OrderId = 1,
            ItemId = itemId,
            Quantity = 1
        };

        OrderItem createdOrderItem =
            await repository.CreateAsync(orderItem);

        DemoConsoleHelper.PrintSuccess(
            "Order item created successfully.");

        PrintOrderItemDetails(createdOrderItem);

        return createdOrderItem;
    }

    private static async Task DemonstrateGetByIdAsync(
        OrderItemRepository repository,
        int orderItemId)
    {
        DemoConsoleHelper.PrintMethodTitle("GetByIdAsync");

        OrderItem? orderItem =
            await repository.GetByIdAsync(orderItemId);

        if (orderItem is null)
        {
            DemoConsoleHelper.PrintNotFound("Order item");
            return;
        }

        DemoConsoleHelper.PrintSuccess(
            "Order item found successfully.");

        PrintOrderItemDetails(orderItem);
    }

    private static async Task DemonstrateUpdateAsync(
        OrderItemRepository repository,
        OrderItem orderItem)
    {
        DemoConsoleHelper.PrintMethodTitle("UpdateAsync");

        orderItem.Quantity = 3;

        bool updateSucceeded =
            await repository.UpdateAsync(orderItem);

        if (!updateSucceeded)
        {
            DemoConsoleHelper.PrintFailure(
                "Order item update failed.");

            return;
        }

        DemoConsoleHelper.PrintSuccess(
            "Order item updated successfully.");

        OrderItem? updatedOrderItem =
            await repository.GetByIdAsync(
                orderItem.OrderItemId);

        if (updatedOrderItem is null)
        {
            DemoConsoleHelper.PrintNotFound(
                "Updated order item");

            return;
        }

        PrintOrderItemDetails(updatedOrderItem);
    }

    private static async Task DemonstrateDeleteAsync(
        OrderItemRepository repository,
        int orderItemId)
    {
        DemoConsoleHelper.PrintMethodTitle("DeleteAsync");

        bool deleteSucceeded =
            await repository.DeleteAsync(orderItemId);

        if (deleteSucceeded)
        {
            DemoConsoleHelper.PrintSuccess(
                "Order item deleted successfully.");
        }
        else
        {
            DemoConsoleHelper.PrintFailure(
                "Order item deletion failed.");
        }
    }

    private static async Task
        DemonstrateGetByIdAfterDeleteAsync(
            OrderItemRepository repository,
            int orderItemId)
    {
        DemoConsoleHelper.PrintMethodTitle(
            "GetByIdAsync After Delete");

        OrderItem? orderItem =
            await repository.GetByIdAsync(orderItemId);

        if (orderItem is null)
        {
            DemoConsoleHelper.PrintSuccess(
                "Deletion confirmed. Order item was not found.");

            return;
        }

        DemoConsoleHelper.PrintFailure(
            "Order item still exists.");

        PrintOrderItemDetails(orderItem);
    }

    private static async Task DeleteTemporaryMenuItemAsync(
        MenuItemRepository repository,
        int itemId)
    {
        DemoConsoleHelper.PrintMethodTitle(
            "Delete Temporary Menu Item");

        bool deleteSucceeded =
            await repository.DeleteAsync(itemId);

        if (deleteSucceeded)
        {
            DemoConsoleHelper.PrintSuccess(
                "Temporary menu item deleted.");
        }
        else
        {
            DemoConsoleHelper.PrintFailure(
                "Temporary menu item deletion failed.");
        }
    }

    private static void PrintOrderItemSummary(
        OrderItem orderItem)
    {
        Console.WriteLine(
            $"Id: {orderItem.OrderItemId}, " +
            $"Order Id: {orderItem.OrderId}, " +
            $"Item Id: {orderItem.ItemId}, " +
            $"Quantity: {orderItem.Quantity}");
    }

    private static void PrintOrderItemDetails(
        OrderItem orderItem)
    {
        Console.WriteLine(
            $"Id: {orderItem.OrderItemId}");

        Console.WriteLine(
            $"Order Id: {orderItem.OrderId}");

        Console.WriteLine(
            $"Item Id: {orderItem.ItemId}");

        Console.WriteLine(
            $"Quantity: {orderItem.Quantity}");
    }
}