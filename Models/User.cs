using System;
using System.ComponentModel.DataAnnotations;

namespace mediumBE.Models;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    [EmailAddress]
    public required string Email { get; set; }
    public string Bio { get; set; } = "";
    public string Image { get; set; } = "";
    public ICollection<Article>? Articles { get; set; } 
    public ICollection<Comment>? Comments { get; set; } 

    public ICollection<User>? Following { get; set; }
    public ICollection<User>? Followers { get; set; }
    public ICollection<Article>? Favorites { get; set; }
}
