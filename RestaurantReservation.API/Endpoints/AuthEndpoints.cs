using RestaurantReservation.API.Models;
using RestaurantReservation.API.Services;

namespace RestaurantReservation.API.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/login",
            (LoginRequest request, JwtTokenGenerator tokenGenerator) =>
            {
                if (string.IsNullOrWhiteSpace(request.Username) ||
                    string.IsNullOrWhiteSpace(request.Password))
                {
                    return Results.BadRequest(new
                    {
                        message = "Username and password are required."
                    });
                }

                if (request.Username != "admin" ||
                    request.Password != "Admin123!")
                {
                    return Results.Unauthorized();
                }

                var token =
                    tokenGenerator.GenerateToken(request.Username);

                return Results.Ok(new
                {
                    accessToken = token
                });
            })
            .WithTags("Authentication")
            .WithSummary("Login")
            .WithDescription(
                "Authenticates the user and returns a JWT access token.")
            .Accepts<LoginRequest>("application/json")
            .Produces(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status500InternalServerError);

        return app;
    }
}