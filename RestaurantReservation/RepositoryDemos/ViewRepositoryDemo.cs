using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Repositories;
using RestaurantReservation.Db.Views;

namespace RestaurantReservation.RepositoryDemos;

public static class ReportingRepositoryDemo
{
    public static async Task RunAsync(
        RestaurantReservationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        ViewRepository repository = new(dbContext);

        DemoConsoleHelper.PrintDemoTitle(
            "Database Views Demo");

        await DemonstrateReservationDetailsViewAsync(repository);

        await DemonstrateEmployeeRestaurantViewAsync(repository);

        await DemonstrateCalculateRestaurantRevenueAsync( repository,restaurantId: 1);

        DemoConsoleHelper.PrintDemoCompleted(
            "Database Views Demo");
    }

    private static async Task
        DemonstrateReservationDetailsViewAsync(
            ViewRepository repository)
    {
        DemoConsoleHelper.PrintMethodTitle(
            "GetReservationDetailsAsync");

        List<ReservationDetailsView> reservations =
            await repository.GetReservationDetailsAsync();

        Console.WriteLine(
            $"Reservations count: {reservations.Count}");

        foreach (ReservationDetailsView reservation in reservations)
        {
            Console.WriteLine();

            Console.WriteLine(
                $"Reservation Id: {reservation.ReservationId}");

            Console.WriteLine(
                $"Date: {reservation.ReservationDate:g}");

            Console.WriteLine(
                $"Party size: {reservation.PartySize}");

            Console.WriteLine(
                $"Customer Id: {reservation.CustomerId}");

            Console.WriteLine(
                $"Customer: {reservation.CustomerFirstName} " +
                $"{reservation.CustomerLastName}");

            Console.WriteLine(
                $"Restaurant Id: {reservation.RestaurantId}");

            Console.WriteLine(
                $"Restaurant: {reservation.RestaurantName}");
        }
    }

    private static async Task
        DemonstrateEmployeeRestaurantViewAsync(
            ViewRepository repository)
    {
        DemoConsoleHelper.PrintMethodTitle(
            "GetEmployeeRestaurantDetailsAsync");

        List<EmployeeRestaurantView> employees =
            await repository.GetEmployeeRestaurantDetailsAsync();

        Console.WriteLine(
            $"Employees count: {employees.Count}");

        foreach (EmployeeRestaurantView employee in employees)
        {
            Console.WriteLine();

            Console.WriteLine(
                $"Employee Id: {employee.EmployeeId}");

            Console.WriteLine(
                $"Employee: {employee.EmployeeFirstName} " +
                $"{employee.EmployeeLastName}");

            Console.WriteLine(
                $"Position: {employee.Position}");

            Console.WriteLine(
                $"Restaurant Id: {employee.RestaurantId}");

            Console.WriteLine(
                $"Restaurant: {employee.RestaurantName}");

            Console.WriteLine(
                $"Restaurant address: " +
                $"{employee.RestaurantAddress}");
        }
    }

    private static async Task
    DemonstrateCalculateRestaurantRevenueAsync(
        ViewRepository repository,
        int restaurantId)
    {
        DemoConsoleHelper.PrintMethodTitle(
            "CalculateRestaurantRevenueAsync");

        decimal? revenue =
            await repository.CalculateRestaurantRevenueAsync(
                restaurantId);

        Console.WriteLine($"Restaurant Id: {restaurantId}");

        if (revenue is null)
        {
            DemoConsoleHelper.PrintNotFound("Restaurant");
            return;
        }

        Console.WriteLine(
            $"Total revenue: {revenue.Value:F2}");
    }
}