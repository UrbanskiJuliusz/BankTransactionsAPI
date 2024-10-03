using BankTransferApi.Models;

namespace BankTransferApi.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync();
        Task<TransactionDto?> GetTransactionByIdAsync(int id);
        Task CreateTransactionAsync(Transaction transaction);
    }
}