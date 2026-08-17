using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation.API.Endpoints;

public static class EmployeeEndpoints
{
    public static IEndpointRouteBuilder MapEmployeeEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/employees/managers",
            async (EmployeeRepository repository) =>
            {
                var managers = await repository.ListManagersAsync();

                return Results.Ok(managers);
            })
            .RequireAuthorization()
            .WithTags("Employees")
            .WithSummary("Get all managers")
            .WithDescription(
                "Retrieves all employees whose position is Manager.")
            .Produces<List<Employee>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status500InternalServerError);

        app.MapGet("/api/employees/{employeeId:int}/average-order-amount",
            async (
                int employeeId,
                OrderRepository orderRepository,
                EmployeeRepository employeeRepository) =>
            {
                if (employeeId <= 0)
                {
                    return Results.BadRequest(new
                    {
                        message = "EmployeeId must be greater than 0."
                    });
                }

                var employee =
                    await employeeRepository.GetByIdAsync(employeeId);

                if (employee is null)
                {
                    return Results.NotFound(new
                    {
                        message = "Employee not found."
                    });
                }

                var averageOrderAmount =
                    await orderRepository
                        .CalculateAverageOrderAmountAsync(employeeId);

                return Results.Ok(new
                {
                    employeeId,
                    averageOrderAmount
                });
            })
            .RequireAuthorization()
            .WithTags("Employees")
            .WithSummary("Get employee average order amount")
            .WithDescription(
                "Calculates the average order amount for a specific employee.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        return app;
    }
}