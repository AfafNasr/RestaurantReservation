using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation.RepositoryDemos;

public static class ReservationRepositoryDemo
{
    public static async Task RunAsync(
        RestaurantReservationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        ReservationRepository repository = new(dbContext);

        DemoConsoleHelper.PrintDemoTitle(
            "Reservation Repository Demo");

        await DemonstrateGetAllAsync(repository);

        Reservation createdReservation =
            await DemonstrateCreateAsync(repository);

        await DemonstrateGetByIdAsync(
            repository,
            createdReservation.ReservationId);

        await DemonstrateUpdateAsync(
            repository,
            createdReservation);

        await DemonstrateGetReservationsByCustomerAsync(
            repository,
            createdReservation.CustomerId);

        await DemonstrateDeleteAsync(
            repository,
            createdReservation.ReservationId);

        await DemonstrateGetByIdAfterDeleteAsync(
            repository,
            createdReservation.ReservationId);

        DemoConsoleHelper.PrintDemoCompleted(
            "Reservation Repository Demo");
    }

    private static async Task DemonstrateGetAllAsync(
        ReservationRepository repository)
    {
        DemoConsoleHelper.PrintMethodTitle("GetAllAsync");

        List<Reservation> reservations =
            await repository.GetAllAsync();

        Console.WriteLine(
            $"Reservations count: {reservations.Count}");

        foreach (Reservation reservation in reservations)
        {
            PrintReservationSummary(reservation);
        }
    }

    private static async Task<Reservation> DemonstrateCreateAsync(
        ReservationRepository repository)
    {
        DemoConsoleHelper.PrintMethodTitle("CreateAsync");

        Reservation reservation = new()
        {
            CustomerId = 1,
            RestaurantId = 1,
            TableId = 1,
            ReservationDate = new DateTime(
                2026, 9, 1, 18, 0, 0),
            PartySize = 2
        };

        Reservation createdReservation =
            await repository.CreateAsync(reservation);

        DemoConsoleHelper.PrintSuccess(
            "Reservation created successfully.");

        PrintReservationDetails(createdReservation);

        return createdReservation;
    }

    private static async Task DemonstrateGetByIdAsync(
        ReservationRepository repository,
        int reservationId)
    {
        DemoConsoleHelper.PrintMethodTitle("GetByIdAsync");

        Reservation? reservation =
            await repository.GetByIdAsync(reservationId);

        if (reservation is null)
        {
            DemoConsoleHelper.PrintNotFound("Reservation");
            return;
        }

        DemoConsoleHelper.PrintSuccess(
            "Reservation found successfully.");

        PrintReservationDetails(reservation);
    }

    private static async Task DemonstrateUpdateAsync(
        ReservationRepository repository,
        Reservation reservation)
    {
        DemoConsoleHelper.PrintMethodTitle("UpdateAsync");

        reservation.ReservationDate =
            new DateTime(2026, 9, 1, 20, 0, 0);

        reservation.PartySize = 1;

        bool updateSucceeded =
            await repository.UpdateAsync(reservation);

        if (!updateSucceeded)
        {
            DemoConsoleHelper.PrintFailure(
                "Reservation update failed.");

            return;
        }

        DemoConsoleHelper.PrintSuccess(
            "Reservation updated successfully.");

        Reservation? updatedReservation =
            await repository.GetByIdAsync(
                reservation.ReservationId);

        if (updatedReservation is null)
        {
            DemoConsoleHelper.PrintNotFound(
                "Updated reservation");

            return;
        }

        PrintReservationDetails(updatedReservation);
    }

    private static async Task
        DemonstrateGetReservationsByCustomerAsync(
            ReservationRepository repository,
            int customerId)
    {
        DemoConsoleHelper.PrintMethodTitle(
            "GetReservationsByCustomerAsync");

        List<Reservation> reservations =
            await repository.GetReservationsByCustomerAsync(
                customerId);

        Console.WriteLine($"Customer Id: {customerId}");
        Console.WriteLine(
            $"Reservations count: {reservations.Count}");

        foreach (Reservation reservation in reservations)
        {
            PrintReservationSummary(reservation);
        }
    }

    private static async Task DemonstrateDeleteAsync(
        ReservationRepository repository,
        int reservationId)
    {
        DemoConsoleHelper.PrintMethodTitle("DeleteAsync");

        bool deleteSucceeded =
            await repository.DeleteAsync(reservationId);

        if (deleteSucceeded)
        {
            DemoConsoleHelper.PrintSuccess(
                "Reservation deleted successfully.");
        }
        else
        {
            DemoConsoleHelper.PrintFailure(
                "Reservation deletion failed.");
        }
    }

    private static async Task
        DemonstrateGetByIdAfterDeleteAsync(
            ReservationRepository repository,
            int reservationId)
    {
        DemoConsoleHelper.PrintMethodTitle(
            "GetByIdAsync After Delete");

        Reservation? reservation =
            await repository.GetByIdAsync(reservationId);

        if (reservation is null)
        {
            DemoConsoleHelper.PrintSuccess(
                "Deletion confirmed. Reservation was not found.");

            return;
        }

        DemoConsoleHelper.PrintFailure(
            "Reservation still exists.");

        PrintReservationDetails(reservation);
    }

    private static void PrintReservationSummary(
        Reservation reservation)
    {
        Console.WriteLine(
            $"Id: {reservation.ReservationId}, " +
            $"Customer Id: {reservation.CustomerId}, " +
            $"Date: {reservation.ReservationDate:g}, " +
            $"Party Size: {reservation.PartySize}");
    }

    private static void PrintReservationDetails(
        Reservation reservation)
    {
        Console.WriteLine(
            $"Id: {reservation.ReservationId}");

        Console.WriteLine(
            $"Customer Id: {reservation.CustomerId}");

        Console.WriteLine(
            $"Restaurant Id: {reservation.RestaurantId}");

        Console.WriteLine(
            $"Table Id: {reservation.TableId}");

        Console.WriteLine(
            $"Reservation date: {reservation.ReservationDate:g}");

        Console.WriteLine(
            $"Party size: {reservation.PartySize}");
    }
}