using Microsoft.EntityFrameworkCore;
using RestaurantReservation.API.Models;
using RestaurantReservation.Db;
using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<RestaurantReservationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

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
});

app.MapGet("/api/reservations/{id:int}",
    async (int id, RestaurantReservationDbContext db) =>
    {
        var reservation = await db.Reservations
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.ReservationId == id);

        return reservation is null
            ? Results.NotFound(new { message = "Reservation not found." })
            : Results.Ok(reservation);
    });

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
    });

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
    });

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
    });


app.Run();


