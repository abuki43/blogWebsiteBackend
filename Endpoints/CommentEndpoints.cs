using mediumBE.Data;
using mediumBE.DTOs;
using mediumBE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace mediumBE.Endpoints;

public static class CommentEndpoints
{
    public static void MapCommentEndpoints(this IEndpointRouteBuilder app)
    {
        // Add comment to an article
        app.MapPost("/api/articles/{slug}/comments", async (MediumContext db, string slug, CreateCommentDTO commentDTO, HttpContext context) =>
        {
            var currentUserId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId))
                return Results.Unauthorized();

            var article = await db.Articles
                .FirstOrDefaultAsync(a => a.Slug == slug);

            if (article == null)
                return Results.NotFound("Article not found");

            var comment = new Comment
            {
                Body = commentDTO.Body,
                ArticleId = article.Id,
                AuthorId = int.Parse(currentUserId),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            db.Comments.Add(comment);
            await db.SaveChangesAsync();

            // Load the author information
            var commentWithAuthor = await db.Comments
                .Include(c => c.Author)
                .FirstAsync(c => c.Id == comment.Id);

            return Results.Ok(new { Comment = new CommentResponseDTO
            {
                Id = comment.Id,
                Body = comment.Body,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt,
                Author = new UserResponseDTO
                {
                    Username = commentWithAuthor.Author.Username,
                    Bio = commentWithAuthor.Author.Bio,
                    Image = commentWithAuthor.Author.Image
                }
            }});
        }).RequireAuthorization();

        // Get comments for an article
        app.MapGet("/api/articles/{slug}/comments", async (MediumContext db, string slug) =>
        {
            var article = await db.Articles
                .FirstOrDefaultAsync(a => a.Slug == slug);

            if (article == null)
                return Results.NotFound("Article not found");

            var comments = await db.Comments
                .Include(c => c.Author)
                .Where(c => c.ArticleId == article.Id)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new CommentResponseDTO
                {
                    Id = c.Id,
                    Body = c.Body,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,
                    Author = new UserResponseDTO
                    {
                        Username = c.Author.Username,
                        Bio = c.Author.Bio,
                        Image = c.Author.Image
                    }
                })
                .ToListAsync();

            return Results.Ok(comments);
        });

        // Delete a comment
        app.MapDelete("/api/articles/{slug}/comments/{id}", async (MediumContext db, string slug, int id, HttpContext context) =>
        {
            // TODO: Get actual user ID from auth
            var userId = 1;

            var comment = await db.Comments
                .FirstOrDefaultAsync(c => c.Id == id && c.Article.Slug == slug);

            if (comment == null)
                return Results.NotFound("Comment not found");

            if (comment.AuthorId != userId)
                return Results.Forbid();

            db.Comments.Remove(comment);
            await db.SaveChangesAsync();

            return Results.Ok();
        });
    }
} 