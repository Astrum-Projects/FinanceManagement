using Domain.Entities;

namespace Infrastructure.Repositories.Transfers
{
    public interface ITransferRepository
    {
         Task<List<Transfer>> GetAllTransfersAsync() ;
         Task<Transfer> GetTransferByIdAsync(int id);
         Task<Transfer> CreateTransferAsync(Transfer Transfer);
         Task<Transfer> DeleteTransferAsync(Transfer Transfer);
    }
}

