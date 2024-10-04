using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository() 
            => _context = new AppDbContext();

        public async Task<User> CreateOrModifyUser(User user)
        {
            var entityEntry = user.Id == 0 ?
                _context.Add(user):
                _context.Update(user);

            await _context.SaveChangesAsync();

            return entityEntry.Entity;
        }

        public async Task<User> DeleteUser(User user)
        {
            var entryEntity = _context.Users.Remove(user);
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
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public Task<User> GetUserById(long telegramId)
        {
            return _context.Users
                .Where(x => x.TelegramId == telegramId)
                .FirstOrDefaultAsync();
        }
    }
}
