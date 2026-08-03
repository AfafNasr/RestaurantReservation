using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Configurations;

public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
{
    public void Configure(EntityTypeBuilder<MenuItem> builder)
    {
        builder.ToTable("MenuItems");

        builder.HasKey(menuItem => menuItem.ItemId);

        builder.Property(menuItem => menuItem.ItemId)
            .HasColumnName("item_id");

        builder.Property(menuItem => menuItem.RestaurantId)
            .HasColumnName("restaurant_id");

        builder.Property(menuItem => menuItem.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(menuItem => menuItem.Description)
            .HasColumnName("description")
            .HasMaxLength(500);

        builder.Property(menuItem => menuItem.Price)
            .HasColumnName("price")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.HasCheckConstraint(
            "CK_MenuItems_Price",
            "[price] >= 0");

        builder.HasOne(menuItem => menuItem.Restaurant)
            .WithMany(restaurant => restaurant.MenuItems)
            .HasForeignKey(menuItem => menuItem.RestaurantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}