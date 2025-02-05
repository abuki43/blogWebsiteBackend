using System;

namespace mediumBE.Models;

public class Comment
{
    public int Id { get; set; }
    public required string Body { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int AuthorId { get; set; }
    public User Author { get; set; }
    public int ArticleId { get; set; }
    public Article Article { get; set; }
}
