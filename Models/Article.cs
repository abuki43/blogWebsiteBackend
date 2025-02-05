using System;

namespace mediumBE.Models;

public class Article
{
    public int Id { get; set; }
    public required string Slug { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Body { get; set; }
    public List<string> TagList { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int AuthorId { get; set; }
    public User Author { get; set; }
    public ICollection<User>? FavoritedBy { get; set; }
    public ICollection<Comment>? Comments { get; set; }
}
