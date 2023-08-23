using API.Models.DTOs.Category;

namespace API.Models.DTOs.Expense;

public class ExpenseDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public double Amount { get; set; }
    public string Description { get; set; }
    public CategoryDto Category { get; set; }
}