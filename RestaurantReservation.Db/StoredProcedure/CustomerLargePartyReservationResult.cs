namespace RestaurantReservation.Db.StoredProcedure;

public class CustomerLargePartyReservationResult
{
    public int CustomerId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public int ReservationId { get; set; }

    public DateTime ReservationDate { get; set; }

    public int PartySize { get; set; }

    public int RestaurantId { get; set; }

    public string RestaurantName { get; set; } = null!;
}