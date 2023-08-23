using API.Models.Domain;

namespace API.Models.DTOs.User;

public class UserDto
{
    public string Email { get; set; }
    public string Username { get; set; }
    public string Currency { get; set; }
    // public double Salary { get; set; }
    public bool EmailConfirmed { get; set; }
    // public double SavingInNumber { get; set; }
    // public double SavingPercentage { get; set; }
    // public ICollection<Bill> Bills { get; set; }
    // public ICollection<Loan> Loans { get; set; }
    public string Token { get; set; }
}