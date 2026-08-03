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

        builder.HasData(
            new OrderItem
            {
                OrderItemId = 1,
                OrderId = 1,
                ItemId = 1,
                Quantity = 2
            },
             new OrderItem
             {
                 OrderItemId = 2,
                 OrderId = 2,
                 ItemId = 2,
                 Quantity = 1
             },
              new OrderItem
              {
                  OrderItemId = 3,
                  OrderId = 3,
                  ItemId = 3,
                  Quantity = 3
              },
               new OrderItem
               {
                   OrderItemId = 4,
                   OrderId = 4,
                   ItemId = 4,
                   Quantity = 2
               },
                new OrderItem
                {
                    OrderItemId = 5,
                    OrderId = 5,
                    ItemId = 5,
                    Quantity = 1
                });
    }
}