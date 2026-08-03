using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");

        builder.HasKey(orderItem => orderItem.OrderItemId);

        builder.Property(orderItem => orderItem.OrderItemId)
            .HasColumnName("order_item_id");

        builder.Property(orderItem => orderItem.OrderId)
            .HasColumnName("order_id");

        builder.Property(orderItem => orderItem.ItemId)
            .HasColumnName("item_id");

        builder.Property(orderItem => orderItem.Quantity)
            .HasColumnName("quantity")
            .IsRequired();

        builder.HasCheckConstraint(
            "CK_OrderItems_Quantity",
            "[quantity] > 0");

        builder.HasIndex(orderItem => new
        {
            orderItem.OrderId,
            orderItem.ItemId
        }).IsUnique();

        builder.HasOne(orderItem => orderItem.Order)
            .WithMany(order => order.OrderItems)
            .HasForeignKey(orderItem => orderItem.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(orderItem => orderItem.MenuItem)
            .WithMany(menuItem => menuItem.OrderItems)
            .HasForeignKey(orderItem => orderItem.ItemId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}