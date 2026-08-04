using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation.RepositoryDemos;

public static class RestaurantRepositoryDemo
{
    public static async Task RunAsync(
        RestaurantReservationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        RestaurantRepository repository = new(dbContext);

        DemoConsoleHelper.PrintDemoTitle(
            "Restaurant Repository Demo");

        await DemonstrateGetAllAsync(repository);

        Restaurant createdRestaurant =
            await DemonstrateCreateAsync(repository);

        await DemonstrateGetByIdAsync(
            repository,
            createdRestaurant.RestaurantId);

        await DemonstrateUpdateAsync(
            repository,
            createdRestaurant);

        await DemonstrateDeleteAsync(
            repository,
            createdRestaurant.RestaurantId);

        await DemonstrateGetByIdAfterDeleteAsync(
            repository,
            createdRestaurant.RestaurantId);

        await DemonstrateGetAllAfterDeleteAsync(repository);

        DemoConsoleHelper.PrintDemoCompleted(
            "Restaurant Repository Demo");
    }

    private static async Task DemonstrateGetAllAsync(
        RestaurantRepository repository)
    {
        DemoConsoleHelper.PrintMethodTitle("GetAllAsync");

        List<Restaurant> restaurants =
            await repository.GetAllAsync();

        Console.WriteLine(
            $"Restaurants count: {restaurants.Count}");

        foreach (Restaurant restaurant in restaurants)
        {
            PrintRestaurantSummary(restaurant);
        }
    }

    private static async Task<Restaurant> DemonstrateCreateAsync(
        RestaurantRepository repository)
    {
        DemoConsoleHelper.PrintMethodTitle("CreateAsync");

        Restaurant restaurant = new()
        {
            Name = "Temporary Restaurant",
            Address = "Ramallah",
            PhoneNumber = "0599111111",
            OpeningHours = "08:00-22:00"
        };

        Restaurant createdRestaurant =
            await repository.CreateAsync(restaurant);

        DemoConsoleHelper.PrintSuccess(
            "Restaurant created successfully.");

        PrintRestaurantDetails(createdRestaurant);

        return createdRestaurant;
    }

    private static async Task DemonstrateGetByIdAsync(
        RestaurantRepository repository,
        int restaurantId)
    {
        DemoConsoleHelper.PrintMethodTitle("GetByIdAsync");

        Restaurant? restaurant =
            await repository.GetByIdAsync(restaurantId);

        if (restaurant is null)
        {
            DemoConsoleHelper.PrintNotFound("Restaurant");
            return;
        }

        DemoConsoleHelper.PrintSuccess(
            "Restaurant found successfully.");

        PrintRestaurantDetails(restaurant);
    }

    private static async Task DemonstrateUpdateAsync(
        RestaurantRepository repository,
        Restaurant restaurant)
    {
        DemoConsoleHelper.PrintMethodTitle("UpdateAsync");

        restaurant.Name = "Updated Temporary Restaurant";
        restaurant.Address = "Nablus";
        restaurant.PhoneNumber = "0599222222";
        restaurant.OpeningHours = "09:00-23:00";

        bool updateSucceeded =
            await repository.UpdateAsync(restaurant);

        if (!updateSucceeded)
        {
            DemoConsoleHelper.PrintFailure(
                "Restaurant update failed.");

            return;
        }

        DemoConsoleHelper.PrintSuccess(
            "Restaurant updated successfully.");

        Restaurant? updatedRestaurant =
            await repository.GetByIdAsync(
                restaurant.RestaurantId);

        if (updatedRestaurant is null)
        {
            DemoConsoleHelper.PrintNotFound(
                "Updated restaurant");

            return;
        }

        PrintRestaurantDetails(updatedRestaurant);
    }

    private static async Task DemonstrateDeleteAsync(
        RestaurantRepository repository,
        int restaurantId)
    {
        DemoConsoleHelper.PrintMethodTitle("DeleteAsync");

        bool deleteSucceeded =
            await repository.DeleteAsync(restaurantId);

        if (deleteSucceeded)
        {
            DemoConsoleHelper.PrintSuccess(
                "Restaurant deleted successfully.");
        }
        else
        {
            DemoConsoleHelper.PrintFailure(
                "Restaurant deletion failed.");
        }
    }

    private static async Task DemonstrateGetByIdAfterDeleteAsync(
        RestaurantRepository repository,
        int restaurantId)
    {
        DemoConsoleHelper.PrintMethodTitle(
            "GetByIdAsync After Delete");

        Restaurant? restaurant =
            await repository.GetByIdAsync(restaurantId);

        if (restaurant is null)
        {
            DemoConsoleHelper.PrintSuccess(
                "Deletion confirmed. Restaurant was not found.");

            return;
        }

        DemoConsoleHelper.PrintFailure(
            "Deletion could not be confirmed. Restaurant still exists.");

        PrintRestaurantDetails(restaurant);
    }

    private static async Task DemonstrateGetAllAfterDeleteAsync(
        RestaurantRepository repository)
    {
        DemoConsoleHelper.PrintMethodTitle(
            "GetAllAsync After Delete");

        List<Restaurant> restaurants =
            await repository.GetAllAsync();

        Console.WriteLine(
            $"Final restaurants count: {restaurants.Count}");

        foreach (Restaurant restaurant in restaurants)
        {
            PrintRestaurantSummary(restaurant);
        }
    }

    private static void PrintRestaurantSummary(
        Restaurant restaurant)
    {
        Console.WriteLine(
            $"Id: {restaurant.RestaurantId}, " +
            $"Name: {restaurant.Name}");
    }

    private static void PrintRestaurantDetails(
        Restaurant restaurant)
    {
        Console.WriteLine($"Id: {restaurant.RestaurantId}");
        Console.WriteLine($"Name: {restaurant.Name}");
        Console.WriteLine($"Address: {restaurant.Address}");
        Console.WriteLine(
            $"Phone number: {restaurant.PhoneNumber}");
        Console.WriteLine(
            $"Opening hours: {restaurant.OpeningHours}");
    }
}