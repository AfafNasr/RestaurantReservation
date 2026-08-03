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
    }

}
