using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddRestaurantRevenueFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                CREATE FUNCTION dbo.fn_CalculateRestaurantRevenue
                (
                    @RestaurantId INT
                )
                RETURNS DECIMAL(18, 2)
                AS
                BEGIN
                    DECLARE @TotalRevenue DECIMAL(18, 2);

                    SELECT
                        @TotalRevenue = COALESCE(SUM(o.total_amount), 0)
                    FROM dbo.Orders AS o
                    INNER JOIN dbo.Reservations AS r
                        ON o.reservation_id = r.reservation_id
                    WHERE r.restaurant_id = @RestaurantId;

                    RETURN @TotalRevenue;
                END;
                """);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
               """
                DROP FUNCTION IF EXISTS
                    dbo.fn_CalculateRestaurantRevenue;
                """);

        }
    }
}
