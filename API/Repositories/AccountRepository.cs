using BankTransferApi.Data;
using BankTransferApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BankTransferApi.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly BankContext _context;

        public AccountRepository(BankContext context)
        {
            _context = context;
        }

        public async Task<Account?> GetAccountByIdAsync(int id)
        {
            return await _context.Accounts
                .Include(a => a.FromTransactions)
                    .ThenInclude(t => t.ToAccount)
                .Include(a => a.ToTransactions)
                    .ThenInclude(t => t.FromAccount)
                .FirstOrDefaultAsync(a => a.Id == id);
        }    

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            return await _context.Accounts
                .Include(a => a.FromTransactions)
                .Include(a => a.ToTransactions)
                .ToListAsync();
        }

        public async Task AddAccountAsync(Account account)
        {
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAccountAsync(Account account)
        {
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAccountAsync(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
            }
        }
    }
}