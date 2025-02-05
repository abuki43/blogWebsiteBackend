using System.Security.Claims;
using mediumBE.Data;
using mediumBE.DTOs;
using mediumBE.Models;
using mediumBE.Services;
using Microsoft.EntityFrameworkCore;

namespace mediumBE.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/users/login", async (MediumContext db, LoginDTO loginDto, JwtService jwtService) =>
        {
            var user = await db.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null || !VerifyPassword(loginDto.Password, user.Password))
                return Results.Unauthorized();

            var token = jwtService.GenerateToken(user);

            return Results.Ok(new UserResponse
            {
                User = new UserResponseDTO
                {
                    Email = user.Email,
                    Token = token,
                    Username = user.Username,
                    Bio = user.Bio,
                    Image = user.Image
                }
            });
        });

        app.MapPost("/api/users", async (MediumContext db, RegisterDTO registerDto, JwtService jwtService) =>
        {
            if (await db.Users.AnyAsync(u => u.Email == registerDto.Email || u.Username == registerDto.Username))
                return Results.BadRequest("User with this email or username already exists");

            var user = new User
            {
                Email = registerDto.Email,
                Username = registerDto.Username,
                Password = HashPassword(registerDto.Password),
                Bio = "",
                Image = ""
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();

            var token = jwtService.GenerateToken(user);

            return Results.Ok(new UserResponse
            {
                User = new UserResponseDTO
                {
                    Email = user.Email,
                    Token = token,
                    Username = user.Username,
                    Bio = user.Bio,
                    Image = user.Image
                }
            });
        });

        // Get current user
        app.MapGet("/api/user", async (MediumContext db, HttpContext context) =>
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Results.Unauthorized();

            var user = await db.Users.FindAsync(int.Parse(userId));
            if (user == null)
                return Results.NotFound();

            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            return Results.Ok(new UserResponse
            {
                User = new UserResponseDTO
                {
                    Email = user.Email,
                    Token = token,
                    Username = user.Username,
                    Bio = user.Bio,
                    Image = user.Image
                }
            });
        }).RequireAuthorization();

        app.MapPut("/api/user", async (MediumContext db, UpdateUserDTO updateDto, HttpContext context) =>
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Results.Unauthorized();

            var user = await db.Users.FindAsync(int.Parse(userId));
            if (user == null)
                return Results.NotFound();

            user.Email = updateDto.Email;
            user.Bio = updateDto.Bio;
            user.Image = updateDto.Image;

            await db.SaveChangesAsync();

            UserResponseDTO updatedUser = new UserResponseDTO{
                Email =  updateDto.Email,
                Username = user.Username,
                Bio = updateDto.Bio,
                Image = updateDto.Image
            };

            return Results.Ok(
                updatedUser
            );
        }).RequireAuthorization();
    }

    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private static bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
} 