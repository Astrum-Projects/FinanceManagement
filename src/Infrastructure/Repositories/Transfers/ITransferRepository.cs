using Domain.Entities;

namespace Infrastructure.Repositories.Transfers
{
    internal interface ITransferRepository
    {
        public Task<List<Transfer>> GetAllTransfersAsync() ;
        public Task<Transfer> GetTransferByIdAsync(int id);
        public Task<Transfer> CreateTransferAsync(Transfer Transfer);
        public Task<Transfer> DeleteTransferAsync(Transfer Transfer);
    }
}

