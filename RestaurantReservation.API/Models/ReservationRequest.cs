using System.ComponentModel.DataAnnotations;

namespace RestaurantReservation.API.Models;

public record ReservationRequest(
    [Range(1, int.MaxValue, ErrorMessage = "CustomerId must be greater than 0.")]
    int CustomerId,

    [Range(1, int.MaxValue, ErrorMessage = "RestaurantId must be greater than 0.")]
    int RestaurantId,

    [Range(1, int.MaxValue, ErrorMessage = "TableId must be greater than 0.")]
    int TableId,

    DateTime ReservationDate,

    [Range(1, int.MaxValue, ErrorMessage = "PartySize must be greater than 0.")]
    int PartySize
);