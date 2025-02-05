using System.Security.Claims;
using mediumBE.Data;
using mediumBE.DTOs;
using mediumBE.Models;
using Microsoft.EntityFrameworkCore;

namespace mediumBE.Endpoints;

public static class ArticleEndpoints
{
    public static void MapArticleEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/articles", async (MediumContext db, CreateArticleDTO articleDTO, HttpContext context) =>
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Results.Unauthorized();

            var article = new Article
            {
                Title = articleDTO.Title,
                Description = articleDTO.Description,
                Body = articleDTO.Body,
                Slug = GenerateSlug(articleDTO.Title),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                AuthorId = int.Parse(userId),
                TagList = articleDTO.TagList
            };

            db.Articles.Add(article);
            await db.SaveChangesAsync();

            // Load the author information
            var articleWithAuthor = await db.Articles
                .Include(a => a.Author)
                .FirstAsync(a => a.Id == article.Id);

            return Results.Ok(new ArticleResponseDTO
            {
                Slug = article.Slug,
                Title = article.Title,
                Description = article.Description,
                Body = article.Body,
                TagList = article.TagList,
                CreatedAt = article.CreatedAt,
                UpdatedAt = article.UpdatedAt,
                Favorited = false,
                FavoritesCount = 0,
                Author = new UserResponseDTO
                {
                    Username = articleWithAuthor.Author.Username,
                    Bio = articleWithAuthor.Author.Bio,
                    Image = articleWithAuthor.Author.Image
                }
            });
        }).RequireAuthorization();

        app.MapGet("/api/articles", async (
            MediumContext db,
            [AsParameters] PaginationQuery pagination,
            string? tag = null) =>
        {
            var query = db.Articles
                .Include(a => a.Author)
                .AsQueryable();

            // Apply tag filter if provided
            if (!string.IsNullOrEmpty(tag))
            {
                query = query.Where(a => a.TagList.Contains(tag));
            }

            // Get total articles count for pagination
            var totalArticles = await query.CountAsync();

            // Apply pagination
            var articles = await query
                .OrderByDescending(a => a.CreatedAt)
                .Skip(pagination.Offset)
                .Take(pagination.Limit)
                .Select(a => new ArticleResponseDTO
                {
                    Slug = a.Slug,
                    Title = a.Title,
                    Description = a.Description,
                    Body = a.Body,
                    TagList = a.TagList,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt,
                    Favorited = false, // TODO: Check if current user favorited
                    FavoritesCount = a.FavoritedBy.Count,
                    Author = new UserResponseDTO
                    {
                        Username = a.Author.Username,
                        Bio = a.Author.Bio,
                        Image = a.Author.Image
                    }
                })
                .ToListAsync();

            return Results.Ok(new
            {
                Articles = articles,
                ArticlesCount = totalArticles
            });
        });

        // Get all unique tags
        app.MapGet("/api/tags", async (MediumContext db) =>
        {
            var tags = await db.Articles
                .SelectMany(a => a.TagList)
                .Distinct()
                .ToListAsync();

            return Results.Ok(new { Tags = tags });
        });

        // Get article by slug
        app.MapGet("/api/articles/{slug}", async (MediumContext db, string slug) =>
        {
            var article = await db.Articles
                .Include(a => a.Author)
                .FirstOrDefaultAsync(a => a.Slug == slug);

            if (article == null)
                return Results.NotFound();

            return Results.Ok(new ArticleResponseDTO
            {
                Slug = article.Slug,
                Title = article.Title,
                Description = article.Description,
                Body = article.Body,
                TagList = article.TagList,
                CreatedAt = article.CreatedAt,
                UpdatedAt = article.UpdatedAt,
                Favorited = false, // TODO: Check if current user favorited
                FavoritesCount = article.FavoritedBy.Count,
                Author = new UserResponseDTO
                {
                    Username = article.Author.Username,
                    Bio = article.Author.Bio,
                    Image = article.Author.Image
                }
            });
        });
    }

    private static string GenerateSlug(string title)
    {
        // Convert to lowercase and replace spaces with hyphens
        var slug = title.ToLower().Replace(" ", "-");
        // Remove special characters
        slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^a-z0-9\-]", "");
        // Add timestamp to make it unique
        slug = $"{slug}-{DateTime.UtcNow.Ticks}";
        return slug;
    }
} 