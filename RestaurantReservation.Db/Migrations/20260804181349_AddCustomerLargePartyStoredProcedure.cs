using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerLargePartyStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                CREATE PROCEDURE dbo.sp_GetCustomersByMinimumPartySize
                    @MinimumPartySize INT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT
                        c.customer_id,
                        c.first_name,
                        c.last_name,
                        c.email,
                        c.phone_number,
                        r.reservation_id,
                        r.reservation_date,
                        r.party_size,
                        res.restaurant_id,
                        res.name AS restaurant_name
                    FROM dbo.Customers AS c
                    INNER JOIN dbo.Reservations AS r
                        ON c.customer_id = r.customer_id
                    INNER JOIN dbo.Restaurants AS res
                        ON r.restaurant_id = res.restaurant_id
                    WHERE r.party_size > @MinimumPartySize
                    ORDER BY
                        r.party_size DESC,
                        r.reservation_date;
                END;
                """);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
               """
                DROP PROCEDURE IF EXISTS
                    dbo.sp_GetCustomersByMinimumPartySize;
                """);
        }
    }
}
