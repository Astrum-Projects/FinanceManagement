using Domain.Entities;

namespace Infrastructure.Repositories.Transfers
{
    public interface ITransferRepository
    {
         Task<List<Transfer>> GetAllTransfersAsync() ;
         Task<Transfer> GetTransferByIdAsync(int id);
         Task<Transfer> CreateTransferAsync(Transfer transfer);
         Task<Transfer> DeleteTransferAsync(Transfer transfer);
    }
}

