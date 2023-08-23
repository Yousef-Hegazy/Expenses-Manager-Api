using API.Core;
using API.Interfaces;
using API.Models.DTOs.Category;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[AllowAnonymous]
public class CategoriesController : BaseApiController
{
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;

    public CategoriesController(ICategoryService categoryService, IMapper mapper)
    {
        _categoryService = categoryService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var catsToReturn = _mapper.Map<List<CategoryDto>>(await _categoryService.GetAllAsync());
        
        return HandleResult(Result<List<CategoryDto>>.Success(catsToReturn));
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var cat = await _categoryService.GetByIdAsync(id);

        return HandleResult(Result<CategoryDto>.Success(_mapper.Map<CategoryDto>(cat)));
    }
}