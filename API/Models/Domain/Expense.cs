namespace API.Models.Domain;

public class Expense
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public double Amount { get; set; }
    public string Description { get; set; }
    public string UserId { get; set; }
    public Guid CategoryId { get; set; }

    // Navigation prop
    public Category Category { get; set; }
}