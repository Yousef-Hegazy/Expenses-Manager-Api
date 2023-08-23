using API.Models.DTOs.Category;

namespace API.Models.DTOs.Expense;

public class ExpenseStatisticDto
{
    public CategoryDto Category { get; set; }
    public double TotalAmount { get; set; }
    public List<StatisticCategoryExpenseDto> Expenses { get; set; }
}