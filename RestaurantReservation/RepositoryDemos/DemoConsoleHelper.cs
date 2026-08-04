namespace RestaurantReservation.RepositoryDemos;

public static class DemoConsoleHelper
{
    public static void PrintDemoTitle(string title)
    {
        Console.WriteLine();
        Console.WriteLine(new string('=', 60));
        Console.WriteLine(title);
        Console.WriteLine(new string('=', 60));
    }

    public static void PrintMethodTitle(string methodName)
    {
        Console.WriteLine();
        Console.WriteLine($"[{methodName}]");
        Console.WriteLine(new string('-', 60));
    }

    public static void PrintSuccess(string message)
    {
        Console.WriteLine($"SUCCESS: {message}");
    }

    public static void PrintFailure(string message)
    {
        Console.WriteLine($"FAILED: {message}");
    }

    public static void PrintNotFound(string entityName)
    {
        Console.WriteLine($"{entityName} was not found.");
    }

    public static void PrintDemoCompleted(string title)
    {
        Console.WriteLine();
        Console.WriteLine(new string('=', 60));
        Console.WriteLine($"{title} completed.");
        Console.WriteLine(new string('=', 60));
        Console.WriteLine();
    }
}