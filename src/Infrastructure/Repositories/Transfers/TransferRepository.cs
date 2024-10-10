using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Transfers
{
    public class TransferRepository : ITransferRepository
    {
        private readonly AppDbContext _context;
        public TransferRepository()
            => _context = new AppDbContext();

        public async Task<Transfer> CreateTransferAsync(Transfer transfer)
        {
            var entryEntity = await _context.Transfers.AddAsync(transfer);
            await _context.SaveChangesAsync();

            return entryEntity.Entity;
        }

        public async Task<Transfer> DeleteTransferAsync(Transfer transfer)
        {
            var entryEntity = _context.Transfers.Remove(transfer);
            await _context.SaveChangesAsync();
            
            return entryEntity.Entity;
        }

        public Task<List<Transfer>> GetAllTransfersAsync() 
            => _context.Transfers.ToListAsync();

        public async Task<List<Transfer>> GetAllTransfersAsync(int userId)
        {
            return await _context.Transfers
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Transfer>> GetAllTransfersAsync(int userId, bool isIncome)
        {
            return await _context.Transfers
                .Include(x => x.Category)
                .Where(x => x.UserId == userId && x.Category.IsIncome == isIncome)
                .ToListAsync();
        }

        public async Task<Transfer> GetTransferByIdAsync(int id) 
            => await _context.Transfers.FirstOrDefaultAsync(x => x.Id == id);
    }
}