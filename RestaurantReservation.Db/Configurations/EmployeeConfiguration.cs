using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.HasKey(employee => employee.EmployeeId);

        builder.Property(employee => employee.EmployeeId)
            .HasColumnName("employee_id");

        builder.Property(employee => employee.RestaurantId)
            .HasColumnName("restaurant_id");

        builder.Property(employee => employee.FirstName)
            .HasColumnName("first_name")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(employee => employee.LastName)
            .HasColumnName("last_name")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(employee => employee.Position)
            .HasColumnName("position")
            .HasMaxLength(50)
            .IsRequired();

        builder.HasOne(employee => employee.Restaurant)
            .WithMany(restaurant => restaurant.Employees)
            .HasForeignKey(employee => employee.RestaurantId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasData(
             new Employee
             {
                 EmployeeId = 1,
                 RestaurantId = 1,
                 FirstName = "Mohammad",
                 LastName = "Sami",
                 Position = "Manager"
             },
             new Employee
             {
                 EmployeeId = 2,
                 RestaurantId = 2,
                 FirstName = "Rami",
                 LastName = "Adel",
                 Position = "Waiter"
             },
             new Employee
             {
                 EmployeeId = 3,
                 RestaurantId = 3,
                 FirstName = "Maha",
                 LastName = "Nabil",
                 Position = "Manager"
             },
             new Employee
             {
                 EmployeeId = 4,
                 RestaurantId = 4,
                 FirstName = "Kareem",
                 LastName = "Tareq",
                 Position = "Waiter"
             },
              new Employee
              {
                  EmployeeId = 5,
                  RestaurantId = 5,
                  FirstName = "Noor",
                  LastName = "Fadi",
                  Position = "Manager"
              });
    }
}