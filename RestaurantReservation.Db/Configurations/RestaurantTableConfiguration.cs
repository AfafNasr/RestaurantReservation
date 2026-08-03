using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Configurations;

public class RestaurantTableConfiguration :
    IEntityTypeConfiguration<Table>
{
    public void Configure(EntityTypeBuilder<Table> builder)
    {
        builder.ToTable("Tables");

        builder.HasKey(table => table.TableId);

        builder.Property(table => table.TableId)
            .HasColumnName("table_id");

        builder.Property(table => table.RestaurantId)
            .HasColumnName("restaurant_id");

        builder.Property(table => table.Capacity)
            .HasColumnName("capacity")
            .IsRequired();

        builder.HasCheckConstraint(
            "CK_Tables_Capacity",
            "[capacity] > 0");

        builder.HasOne(table => table.Restaurant)
            .WithMany(restaurant => restaurant.Tables)
            .HasForeignKey(table => table.RestaurantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}