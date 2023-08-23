using API.Models.Domain;
using API.Models.DTOs.Expense;

namespace API.Interfaces;

public interface IExpenseService
{
    public Task<ExpensesPagedList> GetAllAsync(GetExpensesDto getExpensesDto, string userId);
    public Task<Expense> GetByIdAsync(Guid id, string userId);
    public Task<Guid?> CreateExpenseAsync(Expense expense);
    public Task<Expense> UpdateExpenseAsync(Expense expense);
    public Task<Expense> DeleteExpenseAsync(Guid id, string userId);
    public Task<List<ExpenseStatistic>> GetStatistics(string userId);
}