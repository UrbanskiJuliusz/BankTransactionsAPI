using BankTransferApi.Models;

namespace BankTransferApi.Repositories
{
    public interface ITransactionRepository
    {
        Task<Transaction?> GetTransactionByIdAsync(int id);
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
        Task AddTransactionAsync(Transaction transaction);
    }
}