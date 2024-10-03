using Domain.Entities;
using Infrastructure;
using Infrastructure.Repositories.Categories;
using Microsoft.EntityFrameworkCore;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository() 
        => _context = new AppDbContext();

    public async Task<Category> AddAsync(Category category)
    {
        var entryEntity = await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();

        return entryEntity.Entity;
    }

    public async Task<Category> DeleteAsync(Category category)
    {
        var entryEntity = _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return entryEntity.Entity;
    }

    public async Task<List<Category>> GetAllAsync() 
        => await _context.Categories.ToListAsync();

    public async Task<Category> GetByIdAsync(int id) 
        => await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
}
