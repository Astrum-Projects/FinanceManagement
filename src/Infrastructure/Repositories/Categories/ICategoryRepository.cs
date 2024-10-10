using Domain.Entities;

namespace Infrastructure.Repositories.Categories
{
    public interface ICategoryRepository
    {
        Task<Category> GetByIdAsync(int id);
        Task<List<Category>> GetAllAsync();
        Task<List<Category>> GetAllAsync(int userId, bool isIncome);
        Task<Category> AddAsync(Category category);
        Task<Category> DeleteAsync(Category category);
    }
}
