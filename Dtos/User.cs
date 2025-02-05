using System.ComponentModel.DataAnnotations;

namespace mediumBE.Dtos;


public record class UserGeneral
(
    [Required] string Username,
    [Required] string Email,
    string? Bio,
    string? Image,
    [Required] string Token
);

