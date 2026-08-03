using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Configurations;

public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
{
    public void Configure(EntityTypeBuilder<Restaurant> builder)
    {
        builder.ToTable("Restaurants");

        builder.HasKey(restaurant => restaurant.RestaurantId);

        builder.Property(restaurant => restaurant.RestaurantId)
            .HasColumnName("restaurant_id");

        builder.Property(restaurant => restaurant.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(restaurant => restaurant.Address)
            .HasColumnName("address")
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(restaurant => restaurant.PhoneNumber)
            .HasColumnName("phone_number")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(restaurant => restaurant.OpeningHours)
            .HasColumnName("opening_hours")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasData(
             new Restaurant
             {
                 RestaurantId = 1,
                 Name = "Olive Garden",
                 Address = "Ramallah",
                 PhoneNumber = "0599000001",
                 OpeningHours = "09:00-23:00"
             },
              new Restaurant
              {
                  RestaurantId = 2,
                  Name = "Sea Breeze",
                  Address = "Gaza",
                  PhoneNumber = "0599000002",
                  OpeningHours = "10:00-22:00"
              },
              new Restaurant
              {
                  RestaurantId = 3,
                  Name = "Mountain View",
                  Address = "Nablus",
                  PhoneNumber = "0599000003",
                  OpeningHours = "08:00-22:00"
              },
              new Restaurant
              {
                  RestaurantId = 4,
                  Name = "City Grill",
                  Address = "Hebron",
                  PhoneNumber = "0599000004",
                  OpeningHours = "11:00-00:00"
              },
              new Restaurant
              {
                  RestaurantId = 5,
                  Name = "Garden House",
                  Address = "Jenin",
                  PhoneNumber = "0599000005",
                  OpeningHours = "09:00-21:00"
              });
    }

}
