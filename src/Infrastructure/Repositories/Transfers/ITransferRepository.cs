using Domain.Entities;

namespace Infrastructure.Repositories.Transfers
{
    public interface ITransferRepository
    {
         Task<List<Transfer>> GetAllTransfersAsync() ;
         Task<List<Transfer>> GetAllTransfersAsync(int userId) ;
         Task<List<Transfer>> GetAllTransfersAsync(int userId, bool isIncome);
         Task<Transfer> GetTransferByIdAsync(int id);
         Task<Transfer> CreateTransferAsync(Transfer transfer);
         Task<Transfer> DeleteTransferAsync(Transfer transfer);
    }
}

