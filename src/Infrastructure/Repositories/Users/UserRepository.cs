using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository() 
            => _context = new AppDbContext();

        public async Task<User> CreateUser(User user)
        {
            var entryEntity = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return entryEntity.Entity;
        }

        public async Task<User> DeleteUser(User user)
        {
            user.IsDeleted = true;
            var entryEntity = _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return entryEntity.Entity;
        }

        public Task<List<User>> GetAllUsers()
        {
            return _context.Users
                .Where(x => x.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users
                .Where(x => x.Id == id && x.IsDeleted == false)
                .FirstOrDefaultAsync();
        }

        public Task<User> GetUserById(long telegramId)
        {
            return _context.Users
                .Where(x => x.TelegramId == telegramId && x.IsDeleted == false)
                .FirstOrDefaultAsync();
        }
    }
}
