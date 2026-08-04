namespace RestaurantReservation.Db.Views;

public class ReservationDetailsView
{
    public int ReservationId { get; set; }

    public DateTime ReservationDate { get; set; }

    public int PartySize { get; set; }

    public int CustomerId { get; set; }

    public string CustomerFirstName { get; set; } = null!;

    public string CustomerLastName { get; set; } = null!;

    public int RestaurantId { get; set; }

    public string RestaurantName { get; set; } = null!;
}