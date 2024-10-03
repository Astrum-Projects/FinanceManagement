using Domain.Entities;

namespace Infrastructure.Repositories.Transfers
{
    internal interface ITransferRepository
    {
        public Task<List<Transfer>> GetAllTransfers();
        public Task<Transfer> GetTransferById(int id);
        public Task<Transfer> GetTransferById(long telegramId);
        public Task<Transfer> CreateTransfer(Transfer Transfer);
        public Task<Transfer> DeleteTransfer(Transfer Transfer);
    }
}

