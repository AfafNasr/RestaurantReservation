using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.API.Grpc;

public class ReservationGrpcService
    : ReservationGrpc.ReservationGrpcBase
{
    private readonly RestaurantReservationDbContext _dbContext;

    public ReservationGrpcService(
        RestaurantReservationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override async Task<ReservationListReply> GetAllReservations(
        EmptyRequest request,
        ServerCallContext context)
    {
        var reservations = await _dbContext.Reservations
            .AsNoTracking()
            .ToListAsync();

        var reply = new ReservationListReply();

        reply.Reservations.AddRange(
            reservations.Select(r => new ReservationReply
            {
                ReservationId = r.ReservationId,
                CustomerId = r.CustomerId,
                RestaurantId = r.RestaurantId,
                TableId = r.TableId,
                ReservationDate = r.ReservationDate.ToString("O"),
                PartySize = r.PartySize
            }));

        return reply;
    }
    public override async Task<ReservationReply> GetReservationById(
    GetReservationRequest request,
    ServerCallContext context)
    {
        if (request.Id <= 0)
        {
            throw new RpcException(
                new Status(
                    StatusCode.InvalidArgument,
                    "ReservationId must be greater than 0."));
        }

        var reservation = await _dbContext.Reservations
            .AsNoTracking()
            .FirstOrDefaultAsync(
                r => r.ReservationId == request.Id);

        if (reservation is null)
        {
            throw new RpcException(
                new Status(
                    StatusCode.NotFound,
                    "Reservation not found."));
        }

        return new ReservationReply
        {
            ReservationId = reservation.ReservationId,
            CustomerId = reservation.CustomerId,
            RestaurantId = reservation.RestaurantId,
            TableId = reservation.TableId,
            ReservationDate = reservation.ReservationDate.ToString("O"),
            PartySize = reservation.PartySize
        };
    }

    public override async Task<ReservationReply> CreateReservation(
    CreateReservationRequest request,
    ServerCallContext context)
    {
        if (request.CustomerId <= 0 ||
            request.RestaurantId <= 0 ||
            request.TableId <= 0 ||
            request.PartySize <= 0)
        {
            throw new RpcException(
                new Status(
                    StatusCode.InvalidArgument,
                    "Reservation data is invalid."));
        }

        if (!DateTime.TryParse(
                request.ReservationDate,
                out var reservationDate))
        {
            throw new RpcException(
                new Status(
                    StatusCode.InvalidArgument,
                    "Reservation date is invalid."));
        }

        var customerExists = await _dbContext.Customers
            .AnyAsync(c => c.CustomerId == request.CustomerId);

        if (!customerExists)
        {
            throw new RpcException(
                new Status(
                    StatusCode.NotFound,
                    "Customer not found."));
        }

        var restaurantExists = await _dbContext.Restaurants
            .AnyAsync(r => r.RestaurantId == request.RestaurantId);

        if (!restaurantExists)
        {
            throw new RpcException(
                new Status(
                    StatusCode.NotFound,
                    "Restaurant not found."));
        }

        var table = await _dbContext.Tables
            .AsNoTracking()
            .FirstOrDefaultAsync(
                t => t.TableId == request.TableId);

        if (table is null)
        {
            throw new RpcException(
                new Status(
                    StatusCode.NotFound,
                    "Table not found."));
        }

        if (table.RestaurantId != request.RestaurantId)
        {
            throw new RpcException(
                new Status(
                    StatusCode.InvalidArgument,
                    "The table does not belong to the specified restaurant."));
        }

        if (request.PartySize > table.Capacity)
        {
            throw new RpcException(
                new Status(
                    StatusCode.InvalidArgument,
                    "Party size exceeds table capacity."));
        }

        var reservation = new Reservation
        {
            CustomerId = request.CustomerId,
            RestaurantId = request.RestaurantId,
            TableId = request.TableId,
            ReservationDate = reservationDate,
            PartySize = request.PartySize
        };

        _dbContext.Reservations.Add(reservation);
        await _dbContext.SaveChangesAsync();

        return new ReservationReply
        {
            ReservationId = reservation.ReservationId,
            CustomerId = reservation.CustomerId,
            RestaurantId = reservation.RestaurantId,
            TableId = reservation.TableId,
            ReservationDate = reservation.ReservationDate.ToString("O"),
            PartySize = reservation.PartySize
        };
    }

    public override async Task<ReservationReply> UpdateReservation(
    UpdateReservationRequest request,
    ServerCallContext context)
    {
        if (request.Id <= 0)
        {
            throw new RpcException(
                new Status(
                    StatusCode.InvalidArgument,
                    "ReservationId must be greater than 0."));
        }

        var reservation = await _dbContext.Reservations
            .FindAsync(request.Id);

        if (reservation is null)
        {
            throw new RpcException(
                new Status(
                    StatusCode.NotFound,
                    "Reservation not found."));
        }

        if (!DateTime.TryParse(
                request.ReservationDate,
                out var reservationDate))
        {
            throw new RpcException(
                new Status(
                    StatusCode.InvalidArgument,
                    "Reservation date is invalid."));
        }

        var table = await _dbContext.Tables
            .AsNoTracking()
            .FirstOrDefaultAsync(
                t => t.TableId == request.TableId);

        if (table is null)
        {
            throw new RpcException(
                new Status(
                    StatusCode.NotFound,
                    "Table not found."));
        }

        if (table.RestaurantId != request.RestaurantId)
        {
            throw new RpcException(
                new Status(
                    StatusCode.InvalidArgument,
                    "The table does not belong to the specified restaurant."));
        }

        if (request.PartySize <= 0 ||
            request.PartySize > table.Capacity)
        {
            throw new RpcException(
                new Status(
                    StatusCode.InvalidArgument,
                    "Party size is invalid."));
        }

        reservation.CustomerId = request.CustomerId;
        reservation.RestaurantId = request.RestaurantId;
        reservation.TableId = request.TableId;
        reservation.ReservationDate = reservationDate;
        reservation.PartySize = request.PartySize;

        await _dbContext.SaveChangesAsync();

        return new ReservationReply
        {
            ReservationId = reservation.ReservationId,
            CustomerId = reservation.CustomerId,
            RestaurantId = reservation.RestaurantId,
            TableId = reservation.TableId,
            ReservationDate = reservation.ReservationDate.ToString("O"),
            PartySize = reservation.PartySize
        };
    }

    public override async Task<DeleteReservationReply> DeleteReservation(
    DeleteReservationRequest request,
    ServerCallContext context)
    {
        if (request.Id <= 0)
        {
            throw new RpcException(
                new Status(
                    StatusCode.InvalidArgument,
                    "ReservationId must be greater than 0."));
        }

        var reservation = await _dbContext.Reservations
            .FindAsync(request.Id);

        if (reservation is null)
        {
            throw new RpcException(
                new Status(
                    StatusCode.NotFound,
                    "Reservation not found."));
        }

        _dbContext.Reservations.Remove(reservation);
        await _dbContext.SaveChangesAsync();

        return new DeleteReservationReply
        {
            Success = true,
            Message = "Reservation deleted successfully."
        };
    }

};