namespace API.Models.DTOs.Expense;

public class EditExpenseDto
{
    public DateTime Date { get; set; }
    public double Amount { get; set; }
    public string Description { get; set; }
    public Guid CategoryId { get; set; }
}