using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(customer => customer.CustomerId);

        builder.Property(customer => customer.CustomerId)
            .HasColumnName("customer_id");

        builder.Property(customer => customer.FirstName)
            .HasColumnName("first_name")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(customer => customer.LastName)
            .HasColumnName("last_name")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(customer => customer.Email)
            .HasColumnName("email")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(customer => customer.PhoneNumber)
            .HasColumnName("phone_number")
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(customer => customer.Email)
            .IsUnique();
    }
}
