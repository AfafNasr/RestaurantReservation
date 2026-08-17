namespace RestaurantReservation.API.Models;

public record ServiceResult<T>(
    bool Success,
    T? Data = default,
    string? Error = null,
    ServiceErrorType ErrorType = ServiceErrorType.None);

public enum ServiceErrorType
{
    None,
    NotFound,
    Validation
}