using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantReservation.API.Models;
using RestaurantReservation.API.Services;
using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;
using System.Text;
using RestaurantReservation.API.Grpc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<RestaurantReservationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<EmployeeRepository>();
builder.Services.AddScoped<ReservationRepository>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<CustomerRepository>();
builder.Services.AddScoped<RestaurantRepository>();
builder.Services.AddScoped<RestaurantTableRepository>();

builder.Services.AddScoped<ReservationService>();
builder.Services.AddScoped<JwtTokenGenerator>();


var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("JWT key is missing.");

var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddValidation();

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<ReservationGrpcService>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Restaurant Reservation API v1");
    });
}

app.UseHttpsRedirection();

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        context.Response.StatusCode =
            StatusCodes.Status500InternalServerError;

        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(new
        {
            message = "An unexpected error occurred. Please try again later."
        });
    });
});

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
    .WithDescription("Retrieves a reservation using its unique identifier.")
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

app.MapGet("/api/employees/managers",
    async (EmployeeRepository repository) =>
    {
        var managers = await repository.ListManagersAsync();

        return Results.Ok(managers);
    }).RequireAuthorization()
       .WithTags("Employees")
.WithSummary("Get all managers")
.WithDescription("Retrieves all employees whose position is Manager.")
.Produces<List<Employee>>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status401Unauthorized)
.Produces(StatusCodes.Status500InternalServerError); ; 
;

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

app.MapPost("/api/auth/login",
    (LoginRequest request, JwtTokenGenerator tokenGenerator) =>
    {
        if (string.IsNullOrWhiteSpace(request.Username) ||
    string.IsNullOrWhiteSpace(request.Password))
        {
            return Results.BadRequest(new
            {
                message = "Username and password are required."
            });
        }

        if (request.Username != "admin" ||
            request.Password != "Admin123!")
        {
            return Results.Unauthorized();
        }

        var token = tokenGenerator.GenerateToken(request.Username);

        return Results.Ok(new
        {
            accessToken = token
        });
    }).WithTags("Authentication")
.WithSummary("Login")
.WithDescription("Authenticates the user and returns a JWT access token.")
.Accepts<LoginRequest>("application/json")
.Produces(StatusCodes.Status200OK)
.ProducesValidationProblem()
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status401Unauthorized)
.Produces(StatusCodes.Status500InternalServerError); ;

app.Run();


