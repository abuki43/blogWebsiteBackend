using System.ComponentModel.DataAnnotations;

namespace mediumBE.Dtos;

public record class LoginDto
(
    [Required]string Email,
    [Required]string Password
);
