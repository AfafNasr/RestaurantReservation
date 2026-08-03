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

        builder.HasData(
            new Reservation
            {
                ReservationId = 1,
                CustomerId = 1,
                RestaurantId = 1,
                TableId = 1,
                ReservationDate = new DateTime(2026, 8, 10, 18, 0, 0),
                PartySize = 2
            },
            new Reservation
            {
                ReservationId = 2,
                CustomerId = 2,
                RestaurantId = 2,
                TableId = 2,
                ReservationDate = new DateTime(2026, 8, 11, 19, 0, 0),
                PartySize = 4
            },
            new Reservation
            {
                ReservationId = 3,
                CustomerId = 3,
                RestaurantId = 3,
                TableId = 3,
                ReservationDate = new DateTime(2026, 8, 12, 17, 30, 0),
                PartySize = 5
            },
            new Reservation
            {
                ReservationId = 4,
                CustomerId = 4,
                RestaurantId = 4,
                TableId = 4,
                ReservationDate = new DateTime(2026, 8, 13, 20, 0, 0),
                PartySize = 3
            },
            new Reservation
            {
                ReservationId = 5,
                CustomerId = 5,
                RestaurantId = 5,
                TableId = 5,
                ReservationDate = new DateTime(2026, 8, 14, 18, 30, 0),
                PartySize = 6
            });
    }
}