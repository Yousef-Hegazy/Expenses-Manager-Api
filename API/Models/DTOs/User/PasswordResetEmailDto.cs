namespace API.Models.DTOs.User;

public class PasswordResetEmailDto
{
    public string Email { get; set; }
    public string Link { get; set; }
}