namespace mediumBE.DTOs;

public class CreateArticleDTO
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Body { get; set; }
    public List<string> TagList { get; set; } = new();
}

public class ArticleResponseDTO
{
    public string Slug { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Body { get; set; }
    public List<string> TagList { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool Favorited { get; set; }
    public int FavoritesCount { get; set; }
    public UserResponseDTO Author { get; set; }
} 