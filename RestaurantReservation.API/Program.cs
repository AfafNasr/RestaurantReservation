using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantReservation.API.Models;
using RestaurantReservation.API.Services;
using RestaurantReservation.Db;
using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;
using System.Text;
using RestaurantReservation.API.Services;

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

var app = builder.Build();

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

app.MapGet("/api/reservations", async (RestaurantReservationDbContext db) =>
{
    var reservations = await db.Reservations
        .AsNoTracking()
        .ToListAsync();

    return Results.Ok(reservations);
})
    .RequireAuthorization(); 
;


app.MapGet("/api/reservations/{id:int}",
    async (int id, RestaurantReservationDbContext db) =>
    {
        var reservation = await db.Reservations
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.ReservationId == id);

        return reservation is null
            ? Results.NotFound(new { message = "Reservation not found." })
            : Results.Ok(reservation);
    })
    .RequireAuthorization(); 
;

app.MapPost("/api/reservations",
    async (ReservationRequest request, RestaurantReservationDbContext db) =>
    {
        var reservation = new Reservation
        {
            CustomerId = request.CustomerId,
            RestaurantId = request.RestaurantId,
            TableId = request.TableId,
            ReservationDate = request.ReservationDate,
            PartySize = request.PartySize
        };

        db.Reservations.Add(reservation);
        await db.SaveChangesAsync();

        return Results.Created(
            $"/api/reservations/{reservation.ReservationId}",
            reservation);
    }).RequireAuthorization(); 
;

app.MapPut("/api/reservations/{id:int}",
    async (
        int id,
        ReservationRequest request,
        RestaurantReservationDbContext db) =>
    {
        var reservation = await db.Reservations.FindAsync(id);

        if (reservation is null)
        {
            return Results.NotFound(
                new { message = "Reservation not found." });
        }

        reservation.CustomerId = request.CustomerId;
        reservation.RestaurantId = request.RestaurantId;
        reservation.TableId = request.TableId;
        reservation.ReservationDate = request.ReservationDate;
        reservation.PartySize = request.PartySize;

        await db.SaveChangesAsync();

        return Results.Ok(reservation);
    }).RequireAuthorization(); 
;

app.MapDelete("/api/reservations/{id:int}",
    async (int id, RestaurantReservationDbContext db) =>
    {
        var reservation = await db.Reservations.FindAsync(id);

        if (reservation is null)
        {
            return Results.NotFound(
                new { message = "Reservation not found." });
        }

        db.Reservations.Remove(reservation);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }).RequireAuthorization(); 
;

app.MapGet("/api/employees/managers",
    async (EmployeeRepository repository) =>
    {
        var managers = await repository.ListManagersAsync();

        return Results.Ok(managers);
    }).RequireAuthorization(); 
;

app.MapGet("/api/reservations/customer/{customerId:int}",
    async (int customerId, ReservationRepository repository) =>
    {
        var reservations =
            await repository.GetReservationsByCustomerAsync(customerId);

        return Results.Ok(reservations);
    }).RequireAuthorization(); 
;

app.MapGet("/api/reservations/{reservationId:int}/orders",
    async (int reservationId, OrderRepository repository) =>
    {
        var orders =
            await repository.ListOrdersAndMenuItemsAsync(reservationId);

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
    }).RequireAuthorization(); 
;

app.MapGet("/api/reservations/{reservationId:int}/menu-items",
    async (int reservationId, OrderRepository repository) =>
    {
        var menuItems =
            await repository.ListOrderedMenuItemsAsync(reservationId);

        return Results.Ok(menuItems);
    }).RequireAuthorization(); 
;

app.MapGet("/api/employees/{employeeId:int}/average-order-amount",
    async (int employeeId, OrderRepository repository) =>
    {
        var averageOrderAmount =
            await repository.CalculateAverageOrderAmountAsync(employeeId);

        return Results.Ok(new
        {
            employeeId,
            averageOrderAmount
        });
    }).RequireAuthorization(); 
;

app.MapPost("/api/auth/login",
    (LoginRequest request, JwtTokenGenerator tokenGenerator) =>
    {
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
    });

app.Run();


