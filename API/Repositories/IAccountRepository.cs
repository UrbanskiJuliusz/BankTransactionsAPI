using BankTransferApi.Models;

namespace BankTransferApi.Repositories
{
    public interface IAccountRepository
    {
        Task<Account?> GetAccountByIdAsync(int id);
        Task<IEnumerable<Account>> GetAllAccountsAsync();
        Task AddAccountAsync(Account account);
        Task UpdateAccountAsync(Account account);
        Task DeleteAccountAsync(int id);
    }
}