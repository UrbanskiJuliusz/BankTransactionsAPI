using AutoMapper;
using BankTransferApi.Models;
using BankTransferApi.Repositories;

namespace BankTransferApi.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<AccountDto?> GetAccountByIdAsync(int id)
        {
            var account = await _accountRepository.GetAccountByIdAsync(id);
            return account != null ? _mapper.Map<AccountDto>(account) : null;
        }

        public async Task<IEnumerable<AccountDto>> GetAllAccountsAsync()
        {
            var accounts = await _accountRepository.GetAllAccountsAsync();
            return _mapper.Map<IEnumerable<AccountDto>>(accounts);
        }

        public async Task CreateAccountAsync(Account account)
        {
            await _accountRepository.AddAccountAsync(account);
        }

        public async Task UpdateAccountAsync(int id, Account account)
        {
            var existingAccount = await _accountRepository.GetAccountByIdAsync(id);
            if (existingAccount != null)
            {
                existingAccount.AccountNumber = account.AccountNumber;
                existingAccount.Balance = account.Balance;

                await _accountRepository.UpdateAccountAsync(existingAccount);
            }
        }

        public async Task DeleteAccountAsync(int id)
        {
            await _accountRepository.DeleteAccountAsync(id);
        }
    }
}