namespace API.Models.DTOs.User;

public class RegisterDto
{
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Currency { get; set; }

    public string Link { get; set; }
    // public double Salary { get; set; } = 0.0;
    // public double SavingInNumber { get; set; }
    // public double SavingPercentage { get; set; }
}