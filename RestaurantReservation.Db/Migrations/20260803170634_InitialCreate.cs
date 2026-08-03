using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    customer_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    first_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    phone_number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.customer_id);
                });

            migrationBuilder.CreateTable(
                name: "Restaurants",
                columns: table => new
                {
                    restaurant_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    phone_number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    opening_hours = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurants", x => x.restaurant_id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    employee_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    restaurant_id = table.Column<int>(type: "int", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    position = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.employee_id);
                    table.ForeignKey(
                        name: "FK_Employees_Restaurants_restaurant_id",
                        column: x => x.restaurant_id,
                        principalTable: "Restaurants",
                        principalColumn: "restaurant_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MenuItems",
                columns: table => new
                {
                    item_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    restaurant_id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItems", x => x.item_id);
                    table.CheckConstraint("CK_MenuItems_Price", "[price] >= 0");
                    table.ForeignKey(
                        name: "FK_MenuItems_Restaurants_restaurant_id",
                        column: x => x.restaurant_id,
                        principalTable: "Restaurants",
                        principalColumn: "restaurant_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tables",
                columns: table => new
                {
                    table_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    restaurant_id = table.Column<int>(type: "int", nullable: false),
                    capacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tables", x => x.table_id);
                    table.CheckConstraint("CK_Tables_Capacity", "[capacity] > 0");
                    table.ForeignKey(
                        name: "FK_Tables_Restaurants_restaurant_id",
                        column: x => x.restaurant_id,
                        principalTable: "Restaurants",
                        principalColumn: "restaurant_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    reservation_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customer_id = table.Column<int>(type: "int", nullable: false),
                    restaurant_id = table.Column<int>(type: "int", nullable: false),
                    table_id = table.Column<int>(type: "int", nullable: false),
                    reservation_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    party_size = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.reservation_id);
                    table.CheckConstraint("CK_Reservations_PartySize", "[party_size] > 0");
                    table.ForeignKey(
                        name: "FK_Reservations_Customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "Customers",
                        principalColumn: "customer_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservations_Restaurants_restaurant_id",
                        column: x => x.restaurant_id,
                        principalTable: "Restaurants",
                        principalColumn: "restaurant_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservations_Tables_table_id",
                        column: x => x.table_id,
                        principalTable: "Tables",
                        principalColumn: "table_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    order_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    reservation_id = table.Column<int>(type: "int", nullable: false),
                    employee_id = table.Column<int>(type: "int", nullable: false),
                    order_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    total_amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.order_id);
                    table.CheckConstraint("CK_Orders_TotalAmount", "[total_amount] >= 0");
                    table.ForeignKey(
                        name: "FK_Orders_Employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "Employees",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Reservations_reservation_id",
                        column: x => x.reservation_id,
                        principalTable: "Reservations",
                        principalColumn: "reservation_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    order_item_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    order_id = table.Column<int>(type: "int", nullable: false),
                    item_id = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.order_item_id);
                    table.CheckConstraint("CK_OrderItems_Quantity", "[quantity] > 0");
                    table.ForeignKey(
                        name: "FK_OrderItems_MenuItems_item_id",
                        column: x => x.item_id,
                        principalTable: "MenuItems",
                        principalColumn: "item_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_order_id",
                        column: x => x.order_id,
                        principalTable: "Orders",
                        principalColumn: "order_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_email",
                table: "Customers",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_restaurant_id",
                table: "Employees",
                column: "restaurant_id");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_restaurant_id",
                table: "MenuItems",
                column: "restaurant_id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_item_id",
                table: "OrderItems",
                column: "item_id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_order_id_item_id",
                table: "OrderItems",
                columns: new[] { "order_id", "item_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_employee_id",
                table: "Orders",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_reservation_id",
                table: "Orders",
                column: "reservation_id");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_customer_id",
                table: "Reservations",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_restaurant_id",
                table: "Reservations",
                column: "restaurant_id");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_table_id",
                table: "Reservations",
                column: "table_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tables_restaurant_id",
                table: "Tables",
                column: "restaurant_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "MenuItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Tables");

            migrationBuilder.DropTable(
                name: "Restaurants");
        }
    }
}
