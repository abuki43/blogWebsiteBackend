namespace mediumBE.DTOs;

public class CreateCommentDTO
{
    public required string Body { get; set; }
}

public class CommentResponseDTO
{
    public int Id { get; set; }
    public string Body { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public UserResponseDTO Author { get; set; }
} 