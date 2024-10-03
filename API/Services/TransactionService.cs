using AutoMapper;
using BankTransferApi.Models;
using BankTransferApi.Repositories;

namespace BankTransferApi.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<TransactionDto?> GetTransactionByIdAsync(int id)
        {
            var transaction = await _transactionRepository.GetTransactionByIdAsync(id);
            return transaction != null ? _mapper.Map<TransactionDto>(transaction) : null;
        }

        public async Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync()
        {
            var transactions = await _transactionRepository.GetAllTransactionsAsync();
            return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
        }

        public async Task CreateTransactionAsync(Transaction transaction)
        {
            await _transactionRepository.AddTransactionAsync(transaction);
        }
    }
}