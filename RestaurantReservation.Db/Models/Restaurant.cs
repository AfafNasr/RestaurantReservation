namespace RestaurantReservation.Db.Models;

public class Restaurant
{
    public int RestaurantId { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string OpeningHours { get; set; } = null!;

    public ICollection<Employee> Employees { get; set; } = [];

    public ICollection<Table> Tables { get; set; } = [];

    public ICollection<Reservation> Reservations { get; set; } = [];

    public ICollection<MenuItem> MenuItems { get; set; } = [];
}
