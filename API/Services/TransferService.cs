using BankTransferApi.Models;
using BankTransferApi.Repositories;
using BankTransferApi.Data;

namespace BankTransferApi.Services
{
    public class TransferResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class TransferService : ITransferService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionService _transactionService;
        private readonly BankContext _context;

        public TransferService(IAccountRepository accountRepository, ITransactionService transactionService, BankContext context)
        {
            _accountRepository = accountRepository;
            _transactionService = transactionService;
            _context = context;
        }

        public async Task<TransferResult> TransferAsync(int fromAccountId, int toAccountId, decimal amount)
        {
            var fromAccount = await _accountRepository.GetAccountByIdAsync(fromAccountId);
            var toAccount = await _accountRepository.GetAccountByIdAsync(toAccountId);

            var validationResult = ValidateTransfer(fromAccount, toAccount, amount);
            if (!validationResult.Success)
            {
                return validationResult;
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await ProcessTransferAsync(fromAccount!, toAccount!, amount);

                var newTransaction = new Transaction
                {
                    FromAccountId = fromAccountId,
                    FromAccount = fromAccount!,
                    ToAccountId = toAccountId,
                    ToAccount = toAccount!,
                    Amount = amount,
                    Date = DateTime.UtcNow
                };

                await _transactionService.CreateTransactionAsync(newTransaction);

                await transaction.CommitAsync();

                return new TransferResult { Success = true };
            }
            catch
            {
                await transaction.RollbackAsync();
                return new TransferResult { Success = false, ErrorMessage = "An error occurred during the transfer." };
            }
        }

        private TransferResult ValidateTransfer(Account? fromAccount, Account? toAccount, decimal amount)
        {
            if (fromAccount == null || toAccount == null)
            {
                return new TransferResult { Success = false, ErrorMessage = "One or both accounts do not exist." };
            }

            if (fromAccount.Balance < amount)
            {
                return new TransferResult { Success = false, ErrorMessage = "Insufficient funds." };
            }

            return new TransferResult { Success = true };
        }

        private async Task ProcessTransferAsync(Account fromAccount, Account toAccount, decimal amount)
        {
            fromAccount.Balance -= amount;
            toAccount.Balance += amount;

            await _accountRepository.UpdateAccountAsync(fromAccount);
            await _accountRepository.UpdateAccountAsync(toAccount);
        }
    }
}