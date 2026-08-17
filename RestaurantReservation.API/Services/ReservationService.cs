using RestaurantReservation.API.Models;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation.API.Services;

public class ReservationService
{
    private readonly ReservationRepository _reservationRepository;
    private readonly CustomerRepository _customerRepository;
    private readonly RestaurantRepository _restaurantRepository;
    private readonly RestaurantTableRepository _tableRepository;

    public ReservationService(
        ReservationRepository reservationRepository,
        CustomerRepository customerRepository,
        RestaurantRepository restaurantRepository,
        RestaurantTableRepository tableRepository)
    {
        _reservationRepository = reservationRepository;
        _customerRepository = customerRepository;
        _restaurantRepository = restaurantRepository;
        _tableRepository = tableRepository;
    }

    public async Task<List<Reservation>> GetAllAsync()
    {
        return await _reservationRepository.GetAllAsync();
    }

    public async Task<Reservation?> GetByIdAsync(int id)
    {
        return await _reservationRepository.GetByIdAsync(id);
    }

    public async Task<ServiceResult<Reservation>> CreateAsync(
        ReservationRequest request)
    {
        var validationResult = await ValidateReservationRequestAsync(request);

        if (!validationResult.Success)
        {
            return new ServiceResult<Reservation>(
                false,
                Error: validationResult.Error,
                ErrorType: validationResult.ErrorType);
        }

        var reservation = new Reservation
        {
            CustomerId = request.CustomerId,
            RestaurantId = request.RestaurantId,
            TableId = request.TableId,
            ReservationDate = request.ReservationDate,
            PartySize = request.PartySize
        };

        var createdReservation =
            await _reservationRepository.CreateAsync(reservation);

        return new ServiceResult<Reservation>(
            true,
            createdReservation);
    }

    public async Task<ServiceResult<Reservation>> UpdateAsync(
        int id,
        ReservationRequest request)
    {
        var existingReservation =
            await _reservationRepository.GetByIdAsync(id);

        if (existingReservation is null)
        {
            return new ServiceResult<Reservation>(
                false,
                Error: "Reservation not found.",
                ErrorType: ServiceErrorType.NotFound);
        }

        var validationResult =
            await ValidateReservationRequestAsync(request);

        if (!validationResult.Success)
        {
            return new ServiceResult<Reservation>(
                false,
                Error: validationResult.Error,
                ErrorType: validationResult.ErrorType);
        }

        var reservation = new Reservation
        {
            ReservationId = id,
            CustomerId = request.CustomerId,
            RestaurantId = request.RestaurantId,
            TableId = request.TableId,
            ReservationDate = request.ReservationDate,
            PartySize = request.PartySize
        };

        var updated =
            await _reservationRepository.UpdateAsync(reservation);

        if (!updated)
        {
            return new ServiceResult<Reservation>(
                false,
                Error: "Reservation not found.",
                ErrorType: ServiceErrorType.NotFound);
        }

        return new ServiceResult<Reservation>(
            true,
            reservation);
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        var deleted =
            await _reservationRepository.DeleteAsync(id);

        if (!deleted)
        {
            return new ServiceResult<bool>(
                false,
                Error: "Reservation not found.",
                ErrorType: ServiceErrorType.NotFound);
        }

        return new ServiceResult<bool>(
            true,
            true);
    }

    private async Task<ServiceResult<bool>> ValidateReservationRequestAsync(
        ReservationRequest request)
    {
        if (request.ReservationDate <= DateTime.Now)
        {
            return new ServiceResult<bool>(
                false,
                Error: "Reservation date must be in the future.",
                ErrorType: ServiceErrorType.Validation);
        }

        var customer =
            await _customerRepository.GetByIdAsync(
                request.CustomerId);

        if (customer is null)
        {
            return new ServiceResult<bool>(
                false,
                Error: "The specified customer does not exist.",
                ErrorType: ServiceErrorType.Validation);
        }

        var restaurant =
            await _restaurantRepository.GetByIdAsync(
                request.RestaurantId);

        if (restaurant is null)
        {
            return new ServiceResult<bool>(
                false,
                Error: "The specified restaurant does not exist.",
                ErrorType: ServiceErrorType.Validation);
        }

        var table =
            await _tableRepository.GetByIdAsync(
                request.TableId);

        if (table is null)
        {
            return new ServiceResult<bool>(
                false,
                Error: "The specified table does not exist.",
                ErrorType: ServiceErrorType.Validation);
        }

        if (table.RestaurantId != request.RestaurantId)
        {
            return new ServiceResult<bool>(
                false,
                Error:
                    "The selected table does not belong to the specified restaurant.",
                ErrorType: ServiceErrorType.Validation);
        }

        if (request.PartySize > table.Capacity)
        {
            return new ServiceResult<bool>(
                false,
                Error:
                    "Party size exceeds the selected table capacity.",
                ErrorType: ServiceErrorType.Validation);
        }

        return new ServiceResult<bool>(
            true,
            true);
    }
}