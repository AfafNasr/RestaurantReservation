using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation.RepositoryDemos;

public static class OrderRepositoryDemo
{
    public static async Task RunAsync(
        RestaurantReservationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        OrderRepository repository = new(dbContext);

        DemoConsoleHelper.PrintDemoTitle(
            "Order Repository Demo");

        await DemonstrateGetAllAsync(repository);

        Order createdOrder =
            await DemonstrateCreateAsync(repository);

        await DemonstrateGetByIdAsync(
            repository,
            createdOrder.OrderId);

        await DemonstrateUpdateAsync(
            repository,
            createdOrder);

        await DemonstrateListOrdersAndMenuItemsAsync(
            repository,
            reservationId: 1);

        await DemonstrateListOrderedMenuItemsAsync(
            repository,
            reservationId: 1);

        await DemonstrateCalculateAverageOrderAmountAsync(
            repository,
            employeeId: 1);

        await DemonstrateDeleteAsync(
            repository,
            createdOrder.OrderId);

        await DemonstrateGetByIdAfterDeleteAsync(
            repository,
            createdOrder.OrderId);

        DemoConsoleHelper.PrintDemoCompleted(
            "Order Repository Demo");
    }

    private static async Task DemonstrateGetAllAsync(
        OrderRepository repository)
    {
        DemoConsoleHelper.PrintMethodTitle("GetAllAsync");

        List<Order> orders =
            await repository.GetAllAsync();

        Console.WriteLine($"Orders count: {orders.Count}");

        foreach (Order order in orders)
        {
            PrintOrderSummary(order);
        }
    }

    private static async Task<Order> DemonstrateCreateAsync(
        OrderRepository repository)
    {
        DemoConsoleHelper.PrintMethodTitle("CreateAsync");

        Order order = new()
        {
            ReservationId = 1,
            EmployeeId = 1,
            OrderDate = new DateTime(
                2026, 8, 10, 19, 0, 0),
            TotalAmount = 10.00m
        };

        Order createdOrder =
            await repository.CreateAsync(order);

        DemoConsoleHelper.PrintSuccess(
            "Order created successfully.");

        PrintOrderDetails(createdOrder);

        return createdOrder;
    }

    private static async Task DemonstrateGetByIdAsync(
        OrderRepository repository,
        int orderId)
    {
        DemoConsoleHelper.PrintMethodTitle("GetByIdAsync");

        Order? order =
            await repository.GetByIdAsync(orderId);

        if (order is null)
        {
            DemoConsoleHelper.PrintNotFound("Order");
            return;
        }

        DemoConsoleHelper.PrintSuccess(
            "Order found successfully.");

        PrintOrderDetails(order);
    }

    private static async Task DemonstrateUpdateAsync(
        OrderRepository repository,
        Order order)
    {
        DemoConsoleHelper.PrintMethodTitle("UpdateAsync");

        order.OrderDate =
            new DateTime(2026, 8, 10, 19, 30, 0);

        order.TotalAmount = 20.00m;

        bool updateSucceeded =
            await repository.UpdateAsync(order);

        if (!updateSucceeded)
        {
            DemoConsoleHelper.PrintFailure(
                "Order update failed.");

            return;
        }

        DemoConsoleHelper.PrintSuccess(
            "Order updated successfully.");

        Order? updatedOrder =
            await repository.GetByIdAsync(order.OrderId);

        if (updatedOrder is null)
        {
            DemoConsoleHelper.PrintNotFound("Updated order");
            return;
        }

        PrintOrderDetails(updatedOrder);
    }

    private static async Task
        DemonstrateListOrdersAndMenuItemsAsync(
            OrderRepository repository,
            int reservationId)
    {
        DemoConsoleHelper.PrintMethodTitle(
            "ListOrdersAndMenuItemsAsync");

        List<Order> orders =
            await repository.ListOrdersAndMenuItemsAsync(
                reservationId);

        Console.WriteLine(
            $"Reservation Id: {reservationId}");

        Console.WriteLine($"Orders count: {orders.Count}");

        foreach (Order order in orders)
        {
            Console.WriteLine();
            PrintOrderSummary(order);

            if (order.OrderItems.Count == 0)
            {
                Console.WriteLine("No menu items.");
                continue;
            }

            foreach (OrderItem orderItem in order.OrderItems)
            {
                Console.WriteLine(
                    $"  Item: {orderItem.MenuItem.Name}, " +
                    $"Quantity: {orderItem.Quantity}, " +
                    $"Price: {orderItem.MenuItem.Price:F2}");
            }
        }
    }

    private static async Task
        DemonstrateListOrderedMenuItemsAsync(
            OrderRepository repository,
            int reservationId)
    {
        DemoConsoleHelper.PrintMethodTitle(
            "ListOrderedMenuItemsAsync");

        List<MenuItem> menuItems =
            await repository.ListOrderedMenuItemsAsync(
                reservationId);

        Console.WriteLine(
            $"Reservation Id: {reservationId}");

        Console.WriteLine(
            $"Ordered menu items count: {menuItems.Count}");

        foreach (MenuItem menuItem in menuItems)
        {
            Console.WriteLine(
                $"Id: {menuItem.ItemId}, " +
                $"Name: {menuItem.Name}, " +
                $"Price: {menuItem.Price:F2}");
        }
    }

    private static async Task
        DemonstrateCalculateAverageOrderAmountAsync(
            OrderRepository repository,
            int employeeId)
    {
        DemoConsoleHelper.PrintMethodTitle(
            "CalculateAverageOrderAmountAsync");

        decimal? average =
            await repository.CalculateAverageOrderAmountAsync(
                employeeId);

        Console.WriteLine($"Employee Id: {employeeId}");

        if (average is null)
        {
            Console.WriteLine(
                "The employee has no orders.");

            return;
        }

        Console.WriteLine(
            $"Average order amount: {average.Value:F2}");
    }

    private static async Task DemonstrateDeleteAsync(
        OrderRepository repository,
        int orderId)
    {
        DemoConsoleHelper.PrintMethodTitle("DeleteAsync");

        bool deleteSucceeded =
            await repository.DeleteAsync(orderId);

        if (deleteSucceeded)
        {
            DemoConsoleHelper.PrintSuccess(
                "Order deleted successfully.");
        }
        else
        {
            DemoConsoleHelper.PrintFailure(
                "Order deletion failed.");
        }
    }

    private static async Task
        DemonstrateGetByIdAfterDeleteAsync(
            OrderRepository repository,
            int orderId)
    {
        DemoConsoleHelper.PrintMethodTitle(
            "GetByIdAsync After Delete");

        Order? order =
            await repository.GetByIdAsync(orderId);

        if (order is null)
        {
            DemoConsoleHelper.PrintSuccess(
                "Deletion confirmed. Order was not found.");

            return;
        }

        DemoConsoleHelper.PrintFailure(
            "Order still exists.");

        PrintOrderDetails(order);
    }

    private static void PrintOrderSummary(Order order)
    {
        Console.WriteLine(
            $"Id: {order.OrderId}, " +
            $"Reservation Id: {order.ReservationId}, " +
            $"Employee Id: {order.EmployeeId}, " +
            $"Total: {order.TotalAmount:F2}");
    }

    private static void PrintOrderDetails(Order order)
    {
        Console.WriteLine($"Id: {order.OrderId}");

        Console.WriteLine(
            $"Reservation Id: {order.ReservationId}");

        Console.WriteLine(
            $"Employee Id: {order.EmployeeId}");

        Console.WriteLine(
            $"Order date: {order.OrderDate:g}");

        Console.WriteLine(
            $"Total amount: {order.TotalAmount:F2}");
    }
}