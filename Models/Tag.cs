using System;

namespace mediumBE.Models;

public class Tag
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public  ICollection<Article>? Articles { get; set; }
}
