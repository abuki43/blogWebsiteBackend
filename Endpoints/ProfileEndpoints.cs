using mediumBE.Data;
using mediumBE.DTOs;
using mediumBE.Models;
using Microsoft.EntityFrameworkCore;

namespace mediumBE.Endpoints;

public static class ProfileEndpoints
{
    public static void MapProfileEndpoints(this IEndpointRouteBuilder app)
    {
        // Get user profile
        app.MapGet("/api/profiles/{username}", async (MediumContext db, string username, HttpContext context) =>
        {
            // TODO: Get actual user ID from auth
            var currentUserId = 1;

            var profile = await db.Users
                .Include(u => u.Followers)
                .FirstOrDefaultAsync(u => u.Username == username);

            if (profile == null)
                return Results.NotFound("Profile not found");

            var isFollowing = profile.Followers?.Any(f => f.Id == currentUserId) ?? false;

            return Results.Ok(new ProfileResponseDTO
            {
                Username = profile.Username,
                Bio = profile.Bio,
                Image = profile.Image,
                Following = isFollowing
            });
        });

        // Follow user
        app.MapPost("/api/profiles/{username}/follow", async (MediumContext db, string username, HttpContext context) =>
        {
            // TODO: Get actual user ID from auth
            var currentUserId = 1;

            var currentUser = await db.Users
                .Include(u => u.Following)
                .FirstOrDefaultAsync(u => u.Id == currentUserId);

            var userToFollow = await db.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            if (userToFollow == null)
                return Results.NotFound("User not found");

            if (currentUser.Following == null)
                currentUser.Following = new List<User>();

            if (!currentUser.Following.Any(u => u.Id == userToFollow.Id))
            {
                currentUser.Following.Add(userToFollow);
                await db.SaveChangesAsync();
            }

            return Results.Ok(new ProfileResponseDTO
            {
                Username = userToFollow.Username,
                Bio = userToFollow.Bio,
                Image = userToFollow.Image,
                Following = true
            });
        }).RequireAuthorization();

        // Unfollow user
        app.MapDelete("/api/profiles/{username}/follow", async (MediumContext db, string username, HttpContext context) =>
        {
            // TODO: Get actual user ID from auth
            var currentUserId = 1;

            var currentUser = await db.Users
                .Include(u => u.Following)
                .FirstOrDefaultAsync(u => u.Id == currentUserId);

            var userToUnfollow = await db.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            if (userToUnfollow == null)
                return Results.NotFound("User not found");

            if (currentUser.Following != null)
            {
                var followingUser = currentUser.Following.FirstOrDefault(u => u.Id == userToUnfollow.Id);
                if (followingUser != null)
                {
                    currentUser.Following.Remove(followingUser);
                    await db.SaveChangesAsync();
                }
            }

            return Results.Ok(new ProfileResponseDTO
            {
                Username = userToUnfollow.Username,
                Bio = userToUnfollow.Bio,
                Image = userToUnfollow.Image,
                Following = false
            });
        }).RequireAuthorization();

        // Get following list
        app.MapGet("/api/profiles/{username}/following", async (MediumContext db, string username) =>
        {
            var user = await db.Users
                .Include(u => u.Following)
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
                return Results.NotFound("User not found");

            var following = user.Following?
                .Select(f => new ProfileResponseDTO
                {
                    Username = f.Username,
                    Bio = f.Bio,
                    Image = f.Image,
                    Following = true
                })
                .ToList() ?? new List<ProfileResponseDTO>();

            return Results.Ok(following);
        });

        // Get followers list
        app.MapGet("/api/profiles/{username}/followers", async (MediumContext db, string username) =>
        {
            var user = await db.Users
                .Include(u => u.Followers)
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
                return Results.NotFound("User not found");

            var followers = user.Followers?
                .Select(f => new ProfileResponseDTO
                {
                    Username = f.Username,
                    Bio = f.Bio,
                    Image = f.Image,
                    Following = false // You might want to check if the current user follows each follower
                })
                .ToList() ?? new List<ProfileResponseDTO>();

            return Results.Ok(followers);
        });
    }
} 