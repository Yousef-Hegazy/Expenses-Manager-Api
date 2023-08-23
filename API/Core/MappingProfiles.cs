using API.Models.Domain;
using API.Models.DTOs.Category;
using API.Models.DTOs.Expense;
using API.Models.DTOs.User;
using AutoMapper;

namespace API.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Expense, Expense>();
        CreateMap<Expense, ExpenseDto>().ReverseMap();
        CreateMap<PagedList<Expense>, PagedList<ExpenseDto>>();
        CreateMap<ExpensesPagedList, ExpenseDtoPagedList>();
        CreateMap<CreateExpenseDto, Expense>();
        CreateMap<EditExpenseDto, Expense>();
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<Expense, StatisticCategoryExpenseDto>();
        CreateMap<ExpenseStatistic, ExpenseStatisticDto>();
        CreateMap<AppUser, UserDto>()
            .ForMember(d => d.Username, o => o.MapFrom(s => s.UserName))
            .ForMember(d => d.EmailConfirmed, o => o.MapFrom(s => s.EmailConfirmed));
        CreateMap<RegisterDto, AppUser>().ForMember(d => d.UserName, o => o.MapFrom(s => s.Username));
        CreateMap<EditProfileDto, AppUser>()
            .ForMember(d => d.UserName, o => o.MapFrom(s => s.Username))
            .ForMember(d => d.Email, o => o.MapFrom(s => s.Email));
    }
}