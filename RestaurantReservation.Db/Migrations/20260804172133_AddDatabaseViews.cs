using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddDatabaseViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                CREATE VIEW dbo.vw_ReservationsWithDetails
                AS
                SELECT
                    r.reservation_id,
                    r.reservation_date,
                    r.party_size,
                    c.customer_id,
                    c.first_name AS customer_first_name,
                    c.last_name AS customer_last_name,
                    res.restaurant_id,
                    res.name AS restaurant_name
                FROM dbo.Reservations AS r
                INNER JOIN dbo.Customers AS c
                    ON r.customer_id = c.customer_id
                INNER JOIN dbo.Restaurants AS res
                    ON r.restaurant_id = res.restaurant_id;
                """
            );

            migrationBuilder.Sql(
                """
                CREATE VIEW dbo.vw_EmployeesWithRestaurant
                AS
                SELECT
                    e.employee_id,
                    e.first_name AS employee_first_name,
                    e.last_name AS employee_last_name,
                    e.position,
                    res.restaurant_id,
                    res.name AS restaurant_name,
                    res.address AS restaurant_address
                FROM dbo.Employees AS e
                INNER JOIN dbo.Restaurants AS res
                    ON e.restaurant_id = res.restaurant_id;
                """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                DROP VIEW IF EXISTS dbo.vw_EmployeesWithRestaurant;
                """
            );

            migrationBuilder.Sql(
                """
                DROP VIEW IF EXISTS dbo.vw_ReservationsWithDetails;
                """
            );
        }
    }
}