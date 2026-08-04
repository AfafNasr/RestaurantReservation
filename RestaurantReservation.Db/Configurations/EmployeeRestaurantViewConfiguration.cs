using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Views;

namespace RestaurantReservation.Db.Configurations;

public class EmployeeRestaurantViewConfiguration
    : IEntityTypeConfiguration<EmployeeRestaurantView>
{
    public void Configure(
        EntityTypeBuilder<EmployeeRestaurantView> builder)
    {
        builder.HasNoKey();

        builder.ToView("vw_EmployeesWithRestaurant");

        builder.Property(view => view.EmployeeId)
            .HasColumnName("employee_id");

        builder.Property(view => view.EmployeeFirstName)
            .HasColumnName("employee_first_name")
            .HasMaxLength(50);

        builder.Property(view => view.EmployeeLastName)
            .HasColumnName("employee_last_name")
            .HasMaxLength(50);

        builder.Property(view => view.Position)
            .HasColumnName("position")
            .HasMaxLength(50);

        builder.Property(view => view.RestaurantId)
            .HasColumnName("restaurant_id");

        builder.Property(view => view.RestaurantName)
            .HasColumnName("restaurant_name")
            .HasMaxLength(100);

        builder.Property(view => view.RestaurantAddress)
            .HasColumnName("restaurant_address")
            .HasMaxLength(250);
    }
}