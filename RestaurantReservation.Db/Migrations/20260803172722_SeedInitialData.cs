using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "customer_id", "email", "first_name", "last_name", "phone_number" },
                values: new object[,]
                {
                    { 1, "ahmad.khalil@example.com", "Ahmad", "Khalil", "0569000001" },
                    { 2, "sara.naser@example.com", "Sara", "Naser", "0569000002" },
                    { 3, "omar.saleh@example.com", "Omar", "Saleh", "0569000003" },
                    { 4, "lina.hassan@example.com", "Lina", "Hassan", "0569000004" },
                    { 5, "yousef.ali@example.com", "Yousef", "Ali", "0569000005" }
                });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "restaurant_id", "address", "name", "opening_hours", "phone_number" },
                values: new object[,]
                {
                    { 1, "Ramallah", "Olive Garden", "09:00-23:00", "0599000001" },
                    { 2, "Gaza", "Sea Breeze", "10:00-22:00", "0599000002" },
                    { 3, "Nablus", "Mountain View", "08:00-22:00", "0599000003" },
                    { 4, "Hebron", "City Grill", "11:00-00:00", "0599000004" },
                    { 5, "Jenin", "Garden House", "09:00-21:00", "0599000005" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "employee_id", "first_name", "last_name", "position", "restaurant_id" },
                values: new object[,]
                {
                    { 1, "Mohammad", "Sami", "Manager", 1 },
                    { 2, "Rami", "Adel", "Waiter", 2 },
                    { 3, "Maha", "Nabil", "Manager", 3 },
                    { 4, "Kareem", "Tareq", "Waiter", 4 },
                    { 5, "Noor", "Fadi", "Manager", 5 }
                });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "item_id", "description", "name", "price", "restaurant_id" },
                values: new object[,]
                {
                    { 1, "Pizza with tomato sauce and mozzarella", "Margherita Pizza", 12.50m, 1 },
                    { 2, "Salmon served with vegetables", "Grilled Salmon", 18.00m, 2 },
                    { 3, "Pasta with chicken and cream sauce", "Chicken Pasta", 9.75m, 3 },
                    { 4, "Beef burger served with fries", "Beef Burger", 15.25m, 4 },
                    { 5, "Selection of grilled meats", "Mixed Grill", 22.00m, 5 }
                });

            migrationBuilder.InsertData(
                table: "Tables",
                columns: new[] { "table_id", "capacity", "restaurant_id" },
                values: new object[,]
                {
                    { 1, 2, 1 },
                    { 2, 4, 2 },
                    { 3, 6, 3 },
                    { 4, 4, 4 },
                    { 5, 8, 5 }
                });

            migrationBuilder.InsertData(
                table: "Reservations",
                columns: new[] { "reservation_id", "customer_id", "party_size", "reservation_date", "restaurant_id", "table_id" },
                values: new object[,]
                {
                    { 1, 1, 2, new DateTime(2026, 8, 10, 18, 0, 0, 0, DateTimeKind.Unspecified), 1, 1 },
                    { 2, 2, 4, new DateTime(2026, 8, 11, 19, 0, 0, 0, DateTimeKind.Unspecified), 2, 2 },
                    { 3, 3, 5, new DateTime(2026, 8, 12, 17, 30, 0, 0, DateTimeKind.Unspecified), 3, 3 },
                    { 4, 4, 3, new DateTime(2026, 8, 13, 20, 0, 0, 0, DateTimeKind.Unspecified), 4, 4 },
                    { 5, 5, 6, new DateTime(2026, 8, 14, 18, 30, 0, 0, DateTimeKind.Unspecified), 5, 5 }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "order_id", "employee_id", "order_date", "reservation_id", "total_amount" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 8, 10, 18, 15, 0, 0, DateTimeKind.Unspecified), 1, 25.00m },
                    { 2, 2, new DateTime(2026, 8, 11, 19, 20, 0, 0, DateTimeKind.Unspecified), 2, 18.00m },
                    { 3, 3, new DateTime(2026, 8, 12, 17, 45, 0, 0, DateTimeKind.Unspecified), 3, 29.25m },
                    { 4, 4, new DateTime(2026, 8, 13, 20, 15, 0, 0, DateTimeKind.Unspecified), 4, 30.50m },
                    { 5, 5, new DateTime(2026, 8, 14, 18, 45, 0, 0, DateTimeKind.Unspecified), 5, 22.00m }
                });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "order_item_id", "item_id", "order_id", "quantity" },
                values: new object[,]
                {
                    { 1, 1, 1, 2 },
                    { 2, 2, 2, 1 },
                    { 3, 3, 3, 3 },
                    { 4, 4, 4, 2 },
                    { 5, 5, 5, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "order_item_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "order_item_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "order_item_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "order_item_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "order_item_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "item_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "item_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "item_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "item_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "item_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "order_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "order_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "order_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "order_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "order_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "employee_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "employee_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "employee_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "employee_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "employee_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "reservation_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "reservation_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "reservation_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "reservation_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "reservation_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "customer_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "customer_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "customer_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "customer_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "customer_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "table_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "table_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "table_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "table_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "table_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "restaurant_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "restaurant_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "restaurant_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "restaurant_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "restaurant_id",
                keyValue: 5);
        }
    }
}
