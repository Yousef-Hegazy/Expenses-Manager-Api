using API.Models.Domain;

namespace API.Interfaces;

public interface ICategoryService
{
    public Task<List<Category>> GetAllAsync();
    public Task<Category> GetByIdAsync(Guid id);
}