namespace RestaurantReservation.API.Models;

public record ReservationRequest(
    int CustomerId,
    int RestaurantId,
    int TableId,
    DateTime ReservationDate,
    int PartySize);
