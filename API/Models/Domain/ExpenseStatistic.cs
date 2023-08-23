using API.Models.DTOs.Category;

namespace API.Models.Domain;

public class ExpenseStatistic
{
    public Domain.Category Category { get; set; }
    public double TotalAmount { get; set; }
    public List<Domain.Expense> Expenses { get; set; }
}