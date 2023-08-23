namespace API.Models.DTOs.Expense;

public class ExpensesPagedList
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }
    public int PageSize { get; set; }
    public List<Domain.Expense> Items { get; set; }
    public double TotalAmount { get; set; }
}