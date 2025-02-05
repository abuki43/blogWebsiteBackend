namespace mediumBE.DTOs;

public class PaginationQuery
{
    public int Limit { get; set; } = 20;
    public int Offset { get; set; } = 0;
}

public class ArticlesResponse
{
    public List<ArticleResponseDTO> Articles { get; set; }
    public int ArticlesCount { get; set; }
} 