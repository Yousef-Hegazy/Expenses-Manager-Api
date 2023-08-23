using System.Text.Json.Nodes;
using API.Core;

namespace API.Models.DTOs.Expense;

public class GetExpensesDto : PagingParams
{
    public string Categories { get; set; }
}