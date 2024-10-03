using BankTransferApi.Models;

namespace BankTransferApi.Services
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountDto>> GetAllAccountsAsync();
        Task<AccountDto?> GetAccountByIdAsync(int id);
        Task CreateAccountAsync(Account account);
        Task UpdateAccountAsync(int id, Account account);
        Task DeleteAccountAsync(int id);
    }
}