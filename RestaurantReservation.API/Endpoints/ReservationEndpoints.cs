using RestaurantReservation.API.Models;
using RestaurantReservation.API.Services;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation.API.Endpoints;

public static class ReservationEndpoints
{
    public static IEndpointRouteBuilder MapReservationEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/reservations",
            async (ReservationService service) =>
            {
                var reservations = await service.GetAllAsync();

                return Results.Ok(reservations);
            })
            .RequireAuthorization()
            .WithTags("Reservations")
            .WithSummary("Get all reservations")
            .WithDescription("Retrieves all reservations.")
            .Produces<List<Reservation>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status500InternalServerError);

        app.MapGet("/api/reservations/{id:int}",
            async (int id, ReservationService service) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest(new
                    {
                        message = "ReservationId must be greater than 0."
                    });
                }

                var reservation = await service.GetByIdAsync(id);

                return reservation is null
                    ? Results.NotFound(new
                    {
                        message = "Reservation not found."
                    })
                    : Results.Ok(reservation);
            })
            .RequireAuthorization()
            .WithTags("Reservations")
            .WithSummary("Get reservation by ID")
            .WithDescription(
                "Retrieves a reservation using its unique identifier.")
            .Produces<Reservation>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        app.MapPost("/api/reservations",
            async (
                ReservationRequest request,
                ReservationService service) =>
            {
                var result = await service.CreateAsync(request);

                if (!result.Success)
                {
                    return Results.BadRequest(new
                    {
                        message = result.Error
                    });
                }

                return Results.Created(
                    $"/api/reservations/{result.Data!.ReservationId}",
                    result.Data);
            })
            .RequireAuthorization()
            .WithTags("Reservations")
            .WithSummary("Create a reservation")
            .WithDescription(
                "Creates a new reservation after validating the customer, restaurant, table, and party size.")
            .Accepts<ReservationRequest>("application/json")
            .Produces<Reservation>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status500InternalServerError);

        app.MapPut("/api/reservations/{id:int}",
            async (
                int id,
                ReservationRequest request,
                ReservationService service) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest(new
                    {
                        message = "ReservationId must be greater than 0."
                    });
                }

                var result = await service.UpdateAsync(id, request);

                if (!result.Success)
                {
                    if (result.ErrorType == ServiceErrorType.NotFound)
                    {
                        return Results.NotFound(new
                        {
                            message = result.Error
                        });
                    }

                    return Results.BadRequest(new
                    {
                        message = result.Error
                    });
                }

                return Results.Ok(result.Data);
            })
            .RequireAuthorization()
            .WithTags("Reservations")
            .WithSummary("Update a reservation")
            .WithDescription("Updates an existing reservation by ID.")
            .Accepts<ReservationRequest>("application/json")
            .Produces<Reservation>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        app.MapDelete("/api/reservations/{id:int}",
            async (int id, ReservationService service) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest(new
                    {
                        message = "ReservationId must be greater than 0."
                    });
                }

                var result = await service.DeleteAsync(id);

                if (!result.Success)
                {
                    return Results.NotFound(new
                    {
                        message = result.Error
                    });
                }

                return Results.NoContent();
            })
            .RequireAuthorization()
            .WithTags("Reservations")
            .WithSummary("Delete a reservation")
            .WithDescription("Deletes an existing reservation by ID.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        app.MapGet("/api/reservations/customer/{customerId:int}",
    async (
        int customerId,
        ReservationRepository reservationRepository,
        CustomerRepository customerRepository) =>
    {
        if (customerId <= 0)
        {
            return Results.BadRequest(new
            {
                message = "CustomerId must be greater than 0."
            });
        }

        var customer =
            await customerRepository.GetByIdAsync(customerId);

        if (customer is null)
        {
            return Results.NotFound(new
            {
                message = "Customer not found."
            });
        }

        var reservations =
            await reservationRepository
                .GetReservationsByCustomerAsync(customerId);

        return Results.Ok(reservations);
    })
    .RequireAuthorization()
    .WithTags("Reservations")
    .WithSummary("Get reservations by customer")
    .WithDescription(
        "Retrieves all reservations belonging to a specific customer.")
    .Produces<List<Reservation>>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status401Unauthorized)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status500InternalServerError);

        app.MapGet("/api/reservations/{reservationId:int}/orders",
    async (
        int reservationId,
        OrderRepository orderRepository,
        ReservationRepository reservationRepository) =>
    {
        if (reservationId <= 0)
        {
            return Results.BadRequest(new
            {
                message = "ReservationId must be greater than 0."
            });
        }

        var reservation =
            await reservationRepository.GetByIdAsync(reservationId);

        if (reservation is null)
        {
            return Results.NotFound(new
            {
                message = "Reservation not found."
            });
        }

        var orders =
            await orderRepository
                .ListOrdersAndMenuItemsAsync(reservationId);

        var result = orders.Select(order => new
        {
            order.OrderId,
            order.ReservationId,
            order.EmployeeId,
            order.OrderDate,
            order.TotalAmount,

            Items = order.OrderItems.Select(orderItem => new
            {
                orderItem.OrderItemId,
                orderItem.ItemId,
                orderItem.Quantity,

                MenuItem = new
                {
                    orderItem.MenuItem.ItemId,
                    orderItem.MenuItem.Name,
                    orderItem.MenuItem.Description,
                    orderItem.MenuItem.Price
                }
            })
        });

        return Results.Ok(result);
    })
    .RequireAuthorization()
    .WithTags("Reservations")
    .WithSummary("Get reservation orders")
    .WithDescription(
        "Retrieves all orders for a reservation including their ordered menu items.")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status401Unauthorized)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status500InternalServerError);

        app.MapGet("/api/reservations/{reservationId:int}/menu-items",
    async (
        int reservationId,
        OrderRepository orderRepository,
        ReservationRepository reservationRepository) =>
    {
        if (reservationId <= 0)
        {
            return Results.BadRequest(new
            {
                message = "ReservationId must be greater than 0."
            });
        }

        var reservation =
            await reservationRepository.GetByIdAsync(reservationId);

        if (reservation is null)
        {
            return Results.NotFound(new
            {
                message = "Reservation not found."
            });
        }

        var menuItems =
            await orderRepository
                .ListOrderedMenuItemsAsync(reservationId);

        var result = menuItems.Select(menuItem => new
        {
            menuItem.ItemId,
            menuItem.RestaurantId,
            menuItem.Name,
            menuItem.Description,
            menuItem.Price
        });

        return Results.Ok(result);
    })
    .RequireAuthorization()
    .WithTags("Reservations")
    .WithSummary("Get ordered menu items")
    .WithDescription(
        "Retrieves the distinct menu items ordered for a reservation.")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status401Unauthorized)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status500InternalServerError);


        return app;
    }
}