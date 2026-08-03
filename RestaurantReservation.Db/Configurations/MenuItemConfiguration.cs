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

        builder.HasData(
             new MenuItem
             {
                 ItemId = 1,
                 RestaurantId = 1,
                 Name = "Margherita Pizza",
                 Description = "Pizza with tomato sauce and mozzarella",
                 Price = 12.50m
             },
              new MenuItem
              {
                  ItemId = 2,
                  RestaurantId = 2,
                  Name = "Grilled Salmon",
                  Description = "Salmon served with vegetables",
                  Price = 18.00m
              },
               new MenuItem
               {
                   ItemId = 3,
                   RestaurantId = 3,
                   Name = "Chicken Pasta",
                   Description = "Pasta with chicken and cream sauce",
                   Price = 9.75m
               },
               new MenuItem
               {
                   ItemId = 4,
                   RestaurantId = 4,
                   Name = "Beef Burger",
                   Description = "Beef burger served with fries",
                   Price = 15.25m
               },
                new MenuItem
                {
                    ItemId = 5,
                    RestaurantId = 5,
                    Name = "Mixed Grill",
                    Description = "Selection of grilled meats",
                    Price = 22.00m
                });
    }
}