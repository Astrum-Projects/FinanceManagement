using Domain.Entities;
using Infrastructure.Repositories.Categories;
using Microsoft.EntityFrameworkCore;

public class CategoryRepository : ICategoryRepository
{
    private readonly DbContext _context;

    public CategoryRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<Category> AddAsync(Category category)
    {
        _context.Set<Category>().Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category> DeleteAsync(int id)
    {
        var category = await _context.Set<Category>().FirstOrDefaultAsync(c => c.Id == id);
        if (category != null)
        {
            category.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
        return category;
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return await _context.Set<Category>()
                             .Where(c => !c.IsDeleted)
                             .ToListAsync();
    }

    public async Task<Category> GetByIdAsync(int id)
    {
        return await _context.Set<Category>()
                             .Where(c => c.Id == id && !c.IsDeleted)
                             .FirstOrDefaultAsync();
    }
}
