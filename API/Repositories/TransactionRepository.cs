using BankTransferApi.Data;
using BankTransferApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BankTransferApi.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly BankContext _context;

        public TransactionRepository(BankContext context)
        {
            _context = context;
        }

        public async Task<Transaction?> GetTransactionByIdAsync(int id)
        {
            return await _context.Transactions
                .Include(t => t.FromAccount)
                .Include(t => t.ToAccount)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            return await _context.Transactions
                .Include(t => t.FromAccount)
                .Include(t => t.ToAccount)
                .ToListAsync();
        }

        public async Task AddTransactionAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }
    }
}