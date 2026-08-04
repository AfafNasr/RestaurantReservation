using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.StoredProcedure;

namespace RestaurantReservation.Db.Configurations;

public class CustomerLargePartyReservationResultConfiguration
    : IEntityTypeConfiguration<CustomerLargePartyReservationResult>
{
    public void Configure(
        EntityTypeBuilder<CustomerLargePartyReservationResult> builder)
    {
        builder.HasNoKey();

        builder.ToView(null);

        builder.Property(result => result.CustomerId)
            .HasColumnName("customer_id");

        builder.Property(result => result.FirstName)
            .HasColumnName("first_name")
            .HasMaxLength(50);

        builder.Property(result => result.LastName)
            .HasColumnName("last_name")
            .HasMaxLength(50);

        builder.Property(result => result.Email)
            .HasColumnName("email")
            .HasMaxLength(150);

        builder.Property(result => result.PhoneNumber)
            .HasColumnName("phone_number")
            .HasMaxLength(20);

        builder.Property(result => result.ReservationId)
            .HasColumnName("reservation_id");

        builder.Property(result => result.ReservationDate)
            .HasColumnName("reservation_date");

        builder.Property(result => result.PartySize)
            .HasColumnName("party_size");

        builder.Property(result => result.RestaurantId)
            .HasColumnName("restaurant_id");

        builder.Property(result => result.RestaurantName)
            .HasColumnName("restaurant_name")
            .HasMaxLength(100);
    }
}