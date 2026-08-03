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
        builder.HasData(
            new Customer
            {
                CustomerId = 1,
                FirstName = "Ahmad",
                LastName = "Khalil",
                Email = "ahmad.khalil@example.com",
                PhoneNumber = "0569000001"
            },
             new Customer
             {
                 CustomerId = 2,
                 FirstName = "Sara",
                 LastName = "Naser",
                 Email = "sara.naser@example.com",
                 PhoneNumber = "0569000002"
             },
             new Customer
             {
                 CustomerId = 3,
                 FirstName = "Omar",
                 LastName = "Saleh",
                 Email = "omar.saleh@example.com",
                 PhoneNumber = "0569000003"
             },
             new Customer
             {
                 CustomerId = 4,
                 FirstName = "Lina",
                 LastName = "Hassan",
                 Email = "lina.hassan@example.com",
                 PhoneNumber = "0569000004"
             },
             new Customer
             {
                 CustomerId = 5,
                 FirstName = "Yousef",
                 LastName = "Ali",
                 Email = "yousef.ali@example.com",
                 PhoneNumber = "0569000005"
             });
    }
}
