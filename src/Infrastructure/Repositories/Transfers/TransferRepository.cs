using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Transfers
{
    public class TransferRepository : ITransferRepository
    {
        private readonly AppDbContext _context;
        public TransferRepository()
            => _context = new AppDbContext();

        public async Task<Transfer> CreateTransfer(Transfer transfer)
        {
            var entryEntity = await _context.Transfers.AddAsync(transfer);
            await _context.SaveChangesAsync();
            return entryEntity.Entity;
        }

        public async Task<Transfer> DeleteTransfer(Transfer transfer)
        {
            transfer.IsDeleted = true;
            var entryEntity = _context.Transfers.Update(transfer);
            await _context.SaveChangesAsync();
            return entryEntity.Entity;
        }

        public Task<List<Transfer>> GetAllTransfers()
        {
            return _context.Transfers
                .Where(t => t.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<Transfer> GetTransferById(int id)
        {
            return await _context.Transfers
                .Where(t => t.Id == id && t.IsDeleted == false)
                .FirstOrDefaultAsync();
        }

        public async Task<Transfer> GetTransferById(long telegramId)
        {
            return await _context.Transfers
                .Where(t => t.User.TelegramId == telegramId && t.IsDeleted == false)
                .FirstOrDefaultAsync();
        }
    }
}