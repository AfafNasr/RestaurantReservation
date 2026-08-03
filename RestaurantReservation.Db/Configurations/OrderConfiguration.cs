using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(order => order.OrderId);

        builder.Property(order => order.OrderId)
            .HasColumnName("order_id");

        builder.Property(order => order.ReservationId)
            .HasColumnName("reservation_id");

        builder.Property(order => order.EmployeeId)
            .HasColumnName("employee_id");

        builder.Property(order => order.OrderDate)
            .HasColumnName("order_date")
            .HasColumnType("datetime2")
            .IsRequired();

        builder.Property(order => order.TotalAmount)
            .HasColumnName("total_amount")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.HasCheckConstraint(
            "CK_Orders_TotalAmount",
            "[total_amount] >= 0");

        builder.HasOne(order => order.Reservation)
            .WithMany(reservation => reservation.Orders)
            .HasForeignKey(order => order.ReservationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(order => order.Employee)
            .WithMany(employee => employee.Orders)
            .HasForeignKey(order => order.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new Order
            {
                OrderId = 1,
                ReservationId = 1,
                EmployeeId = 1,
                OrderDate = new DateTime(2026, 8, 10, 18, 15, 0),
                TotalAmount = 25.00m
            },
            new Order
            {
                OrderId = 2,
                ReservationId = 2,
                EmployeeId = 2,
                OrderDate = new DateTime(2026, 8, 11, 19, 20, 0),
                TotalAmount = 18.00m
            },
            new Order
            {
                OrderId = 3,
                ReservationId = 3,
                EmployeeId = 3,
                OrderDate = new DateTime(2026, 8, 12, 17, 45, 0),
                TotalAmount = 29.25m
            },
            new Order
            {
                OrderId = 4,
                ReservationId = 4,
                EmployeeId = 4,
                OrderDate = new DateTime(2026, 8, 13, 20, 15, 0),
                TotalAmount = 30.50m
            },
             new Order
             {
                 OrderId = 5,
                 ReservationId = 5,
                 EmployeeId = 5,
                 OrderDate = new DateTime(2026, 8, 14, 18, 45, 0),
                 TotalAmount = 22.00m
             });
    }
}