using Domain.Entities;

namespace Infrastructure.Repositories.Categories
{
    public interface ICategoryRepository
    {
        Task<Category> GetByIdAsync(int id);
        Task<List<Category>> GetAllAsync();
        Task<Category> AddAsync(Category category);
        Task<Category> DeleteAsync(Category category);
    }
}
