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
        builder.HasData(
             new Table
             {
                 TableId = 1,
                 RestaurantId = 1,
                 Capacity = 2
             },
              new Table
              {
                  TableId = 2,
                  RestaurantId = 2,
                  Capacity = 4
              },
              new Table
              {
                  TableId = 3,
                  RestaurantId = 3,
                  Capacity = 6
              },
               new Table
               {
                   TableId = 4,
                   RestaurantId = 4,
                   Capacity = 4
               },
               new Table
               {
                   TableId = 5,
                   RestaurantId = 5,
                   Capacity = 8
               });
    }
}