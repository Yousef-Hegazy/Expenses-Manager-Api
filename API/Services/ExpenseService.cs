using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using API.Core;
using API.Data;
using API.Interfaces;
using API.Models.Domain;
using API.Models.DTOs.Expense;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class ExpenseService : IExpenseService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public ExpenseService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ExpensesPagedList> GetAllAsync(GetExpensesDto getExpensesDto, string userId)
    {
        var total = await _context.Expenses.Where(e => e.UserId == userId).SumAsync(e => e.Amount);
        var query = _context.Expenses
            .Include(e => e.Category)
            .Where(e => e.UserId == userId)
            .OrderBy(e => e.Date)
            .AsQueryable();

        if (!string.IsNullOrEmpty(getExpensesDto.Categories))
        {
            var cats = JsonSerializer.Deserialize<List<string>>(json: getExpensesDto.Categories);
            query = query.Where(e => getExpensesDto.Categories.Contains(e.Category.Id.ToString()));
        }

        var expenses = await PagedList<Expense>.CreateAsync(query,
            new PagingParams { Page = getExpensesDto.Page, PageSize = getExpensesDto.PageSize });

        var expensesWithTotal = new ExpensesPagedList
        {
            PageSize = expenses.PageSize,
            CurrentPage = expenses.CurrentPage,
            TotalPages = expenses.TotalPages,
            Items = expenses.Items,
            TotalItems = expenses.TotalItems,
            TotalAmount = total,
        };


        return expensesWithTotal;
    }

    public async Task<Expense> GetByIdAsync(Guid id, string userId)
    {
        return await _context.Expenses
            .Include(e => e.Category)
            .Where(e => e.UserId == userId)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Guid?> CreateExpenseAsync(Expense expense)
    {
        expense.Date = expense.Date.ToUniversalTime();
        _context.Expenses.Add(expense);

        var res = await _context.SaveChangesAsync() > 0;

        if (!res) return null;

        return expense.Id;
    }

    public async Task<Expense> UpdateExpenseAsync(Expense expense)
    {
        var existingExpense =
            await _context.Expenses
                .Include(e => e.Category)
                .Where(e => e.UserId == expense.UserId)
                .FirstOrDefaultAsync(e => e.Id == expense.Id);


        if (existingExpense is null) return null;

        existingExpense.Date = expense.Date.ToUniversalTime();
        existingExpense.Amount = expense.Amount;
        existingExpense.Category = await _context.Categories.FindAsync(expense.CategoryId);
        existingExpense.Description = expense.Description;

        // existingExpense = _mapper.Map(source: expense, destination: existingExpense);

        var res = await _context.SaveChangesAsync() > 0;

        return res ? existingExpense : null;
    }

    public async Task<Expense> DeleteExpenseAsync(Guid id, string userId)
    {
        var existingExpense =
            await _context.Expenses
                .Include(e => e.Category)
                .Where(e => e.UserId == userId)
                .FirstOrDefaultAsync(e => e.Id == id);

        if (existingExpense is null) return null;

        _context.Expenses.Remove(existingExpense);

        var res = await _context.SaveChangesAsync() > 0;

        return res ? existingExpense : null;
    }

    public async Task<List<ExpenseStatistic>> GetStatistics(string userId)
    {
        var groups = await _context.Expenses
            .Where(e => e.UserId == userId)
            .GroupBy(e => e.Category)
            .Select(group => new ExpenseStatistic
                { Category = group.Key, TotalAmount = group.Sum(g => g.Amount), Expenses = group.ToList() })
            .ToListAsync();

        return groups;
    }
}