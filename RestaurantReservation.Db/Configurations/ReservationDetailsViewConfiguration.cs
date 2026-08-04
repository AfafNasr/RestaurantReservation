using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Views;

namespace RestaurantReservation.Db.Configurations;

public class ReservationDetailsViewConfiguration
    : IEntityTypeConfiguration<ReservationDetailsView>
{
    public void Configure(
        EntityTypeBuilder<ReservationDetailsView> builder)
    {
        builder.HasNoKey();

        builder.ToView("vw_ReservationsWithDetails");

        builder.Property(view => view.ReservationId)
            .HasColumnName("reservation_id");

        builder.Property(view => view.ReservationDate)
            .HasColumnName("reservation_date");

        builder.Property(view => view.PartySize)
            .HasColumnName("party_size");

        builder.Property(view => view.CustomerId)
            .HasColumnName("customer_id");

        builder.Property(view => view.CustomerFirstName)
            .HasColumnName("customer_first_name")
            .HasMaxLength(50);

        builder.Property(view => view.CustomerLastName)
            .HasColumnName("customer_last_name")
            .HasMaxLength(50);

        builder.Property(view => view.RestaurantId)
            .HasColumnName("restaurant_id");

        builder.Property(view => view.RestaurantName)
            .HasColumnName("restaurant_name")
            .HasMaxLength(100);
    }
}