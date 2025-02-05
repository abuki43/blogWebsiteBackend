namespace mediumBE.DTOs;

public class LoginDTO
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class RegisterDTO
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class UserResponseDTO
{
    public string Email { get; set; }
    public string Token { get; set; } 
    public string Username { get; set; }
    public string Bio { get; set; }
    public string Image { get; set; }
}

// Wrapper for consistent API response
public class UserResponse
{
    public UserResponseDTO User { get; set; }
}

public class UpdateUserDTO
{
    public string? Email { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Bio { get; set; }
    public string? Image { get; set; }
} 