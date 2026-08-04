using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public class ReservationRepository
{
    private readonly RestaurantReservationDbContext _dbContext;

    public ReservationRepository(RestaurantReservationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Reservation>> GetAllAsync()
    {
        return await _dbContext.Reservations
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Reservation?> GetByIdAsync(int reservationId)
    {
        return await _dbContext.Reservations
            .AsNoTracking()
            .FirstOrDefaultAsync(
                reservation => reservation.ReservationId == reservationId);
    }

    public async Task<Reservation> CreateAsync(Reservation reservation)
    {
        ArgumentNullException.ThrowIfNull(reservation);

        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.SaveChangesAsync();

        return reservation;
    }

    public async Task<bool> UpdateAsync(Reservation reservation)
    {
        ArgumentNullException.ThrowIfNull(reservation);

        Reservation? existingReservation =
            await _dbContext.Reservations.FindAsync(
                reservation.ReservationId);

        if (existingReservation is null)
        {
            return false;
        }

        existingReservation.CustomerId = reservation.CustomerId;
        existingReservation.RestaurantId = reservation.RestaurantId;
        existingReservation.TableId = reservation.TableId;
        existingReservation.ReservationDate = reservation.ReservationDate;
        existingReservation.PartySize = reservation.PartySize;

        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int reservationId)
    {
        Reservation? reservation =
            await _dbContext.Reservations.FindAsync(reservationId);

        if (reservation is null)
        {
            return false;
        }

        _dbContext.Reservations.Remove(reservation);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<List<Reservation>> GetReservationsByCustomerAsync(
        int customerId)
    {
        return await _dbContext.Reservations
            .AsNoTracking()
            .Where(reservation => reservation.CustomerId == customerId)
            .OrderBy(reservation => reservation.ReservationDate)
            .ToListAsync();
    }
}