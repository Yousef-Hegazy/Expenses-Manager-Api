using System.Security.Claims;
using API.Core;
using API.Interfaces;
using API.Models.Domain;
using API.Models.DTOs.Expense;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ExpensesController : BaseApiController
{
    private readonly IExpenseService _expenseService;
    private readonly IMapper _mapper;

    public ExpensesController(IExpenseService expenseService, IMapper mapper)
    {
        _expenseService = expenseService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetExpensesDto getExpensesDto)
    {
        var expenses = await _expenseService.GetAllAsync(getExpensesDto,
            HttpContext.User.FindFirstValue(claimType: ClaimTypes.NameIdentifier));

        var expToReturn = _mapper.Map<ExpenseDtoPagedList>(expenses);

        return HandleResult(Result<ExpenseDtoPagedList>.Success(expToReturn));
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var exp = _mapper.Map<ExpenseDto>(await _expenseService.GetByIdAsync(id,
            HttpContext.User.FindFirstValue(claimType: ClaimTypes.NameIdentifier)));

        return HandleResult(Result<ExpenseDto>.Success(exp));
    }

    [HttpPost]
    public async Task<IActionResult> CreateExpense(CreateExpenseDto createExpenseDto)
    {
        var expense = _mapper.Map<Expense>(createExpenseDto);

        expense.UserId = HttpContext.User.FindFirstValue(claimType: ClaimTypes.NameIdentifier);

        var res = await _expenseService.CreateExpenseAsync(expense);

        return res is null
            ? HandleResult(Result<Guid?>.Failure("Failed to create expense"))
            : CreatedAtAction(nameof(GetById), new { id = res }, Result<Guid?>.Success(res));
    }

    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> EditExpense([FromRoute] Guid id, [FromBody] EditExpenseDto editExpenseDto)
    {
        var expenseDomain = _mapper.Map<Expense>(editExpenseDto);

        expenseDomain.Id = id;

        expenseDomain.UserId = HttpContext.User.FindFirstValue(claimType: ClaimTypes.NameIdentifier);

        expenseDomain = await _expenseService.UpdateExpenseAsync(expenseDomain);

        if (expenseDomain is null)
            return HandleResult(Result<ExpenseDto>.Failure($"Failed to update expense with id: {id}"));

        var expenseDto = _mapper.Map<ExpenseDto>(expenseDomain);

        return Ok(expenseDto);
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteExpense(Guid id)
    {
        var deletedExpense = await _expenseService.DeleteExpenseAsync(id,
            HttpContext.User.FindFirstValue(claimType: ClaimTypes.NameIdentifier));

        return deletedExpense is null
            ? HandleResult(Result<ExpenseDto>.Failure($"Failed to delete expense with id: {id}"))
            : Ok(_mapper.Map<ExpenseDto>(deletedExpense));
    }

    [AllowAnonymous]
    [HttpGet("statistics")]
    public async Task<IActionResult> GetStatistics()
    {
        var res = await _expenseService.GetStatistics(User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (res is null) return Ok();

        var mappedRes = _mapper.Map<List<ExpenseStatisticDto>>(res);

        return Ok(mappedRes);
    }
}