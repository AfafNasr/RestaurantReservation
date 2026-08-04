namespace RestaurantReservation.Db.Views;

public class EmployeeRestaurantView
{
    public int EmployeeId { get; set; }

    public string EmployeeFirstName { get; set; } = null!;

    public string EmployeeLastName { get; set; } = null!;

    public string Position { get; set; } = null!;

    public int RestaurantId { get; set; }

    public string RestaurantName { get; set; } = null!;

    public string RestaurantAddress { get; set; } = null!;
}