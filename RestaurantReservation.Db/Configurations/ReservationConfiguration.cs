using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Configurations;

public class ReservationConfiguration :
    IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("Reservations");

        builder.HasKey(reservation => reservation.ReservationId);

        builder.Property(reservation => reservation.ReservationId)
            .HasColumnName("reservation_id");

        builder.Property(reservation => reservation.CustomerId)
            .HasColumnName("customer_id");

        builder.Property(reservation => reservation.RestaurantId)
            .HasColumnName("restaurant_id");

        builder.Property(reservation => reservation.TableId)
            .HasColumnName("table_id");

        builder.Property(reservation => reservation.ReservationDate)
            .HasColumnName("reservation_date")
            .HasColumnType("datetime2")
            .IsRequired();

        builder.Property(reservation => reservation.PartySize)
            .HasColumnName("party_size")
            .IsRequired();

        builder.HasCheckConstraint(
            "CK_Reservations_PartySize",
            "[party_size] > 0");

        builder.HasOne(reservation => reservation.Customer)
            .WithMany(customer => customer.Reservations)
            .HasForeignKey(reservation => reservation.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(reservation => reservation.Restaurant)
            .WithMany(restaurant => restaurant.Reservations)
            .HasForeignKey(reservation => reservation.RestaurantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(reservation => reservation.Table)
            .WithMany(table => table.Reservations)
            .HasForeignKey(reservation => reservation.TableId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}