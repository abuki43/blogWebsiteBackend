using System.ComponentModel.DataAnnotations;

namespace mediumBE.Dtos;

public record class RegisterDto
(
    [Required]string Username,
    [Required]string Email,
    [Required]string Password
);
