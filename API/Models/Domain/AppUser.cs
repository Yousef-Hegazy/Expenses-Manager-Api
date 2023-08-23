using Microsoft.AspNetCore.Identity;

namespace API.Models.Domain;

public class AppUser : IdentityUser
{
    public string Currency { get; set; }
    // public double Salary { get; set; } = 0.0;
    // public double SavingInNumber { get; set; }
    // public double SavingPercentage { get; set; }
    // public ICollection<Bill> Bills { get; set; } = new List<Bill>();
    // public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}