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
    }
}