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
            transfer.IsDeleted = true;
            var entryEntity = _context.Transfers.Update(transfer);
            await _context.SaveChangesAsync();
            
            return entryEntity.Entity;
        }

        public Task<List<Transfer>> GetAllTransfersAsync()
        {
            return _context.Transfers
                .Where(t => t.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<Transfer> GetTransferByIdAsync(int id)
        {
            return await _context.Transfers
                .Where(t => t.Id == id && t.IsDeleted == false)
                .FirstOrDefaultAsync();
        }

    }
}