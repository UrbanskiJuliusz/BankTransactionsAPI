using AutoMapper;
using BankTransferApi.Models;
using BankTransferApi.Repositories;
using BankTransferApi.Services;
using Moq;
using Xunit;

namespace BankTransferApi.Tests
{
    public class TransactionServiceTests
    {
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly TransactionService _transactionService;

        public TransactionServiceTests()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _mapperMock = new Mock<IMapper>();
            _transactionService = new TransactionService(_transactionRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetTransactionByIdAsync_ShouldReturnTransaction_WhenTransactionExists()
        {
            var transaction = new Transaction { Id = 1, FromAccountId = 1, ToAccountId = 2, Amount = 100 };
            var transactionDto = new TransactionDto { Id = 1, FromAccountNumber = "123456", ToAccountNumber = "654321", Amount = 100 };

            _transactionRepositoryMock.Setup(x => x.GetTransactionByIdAsync(1)).ReturnsAsync(transaction);
            _mapperMock.Setup(m => m.Map<TransactionDto>(transaction)).Returns(transactionDto);

            var result = await _transactionService.GetTransactionByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result?.Id);
        }

        [Fact]
        public async Task GetTransactionByIdAsync_ShouldReturnNull_WhenTransactionDoesNotExist()
        {
            _transactionRepositoryMock.Setup(x => x.GetTransactionByIdAsync(1)).ReturnsAsync((Transaction?)null);

            var result = await _transactionService.GetTransactionByIdAsync(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllTransactionsAsync_ShouldReturnAllTransactions()
        {
            var transactions = new List<Transaction>
            {
                new Transaction { Id = 1, FromAccountId = 1, ToAccountId = 2, Amount = 100 },
                new Transaction { Id = 2, FromAccountId = 2, ToAccountId = 3, Amount = 200 }
            };
            var transactionsDto = new List<TransactionDto>
            {
                new TransactionDto { Id = 1, FromAccountNumber = "123456", ToAccountNumber = "654321", Amount = 100 },
                new TransactionDto { Id = 2, FromAccountNumber = "654321", ToAccountNumber = "987654", Amount = 200 }
            };

            _transactionRepositoryMock.Setup(x => x.GetAllTransactionsAsync()).ReturnsAsync(transactions);
            _mapperMock.Setup(m => m.Map<IEnumerable<TransactionDto>>(transactions)).Returns(transactionsDto);

            var result = await _transactionService.GetAllTransactionsAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task CreateTransactionAsync_ShouldCallRepository_AddTransactionAsync()
        {
            var transaction = new Transaction
            {
                FromAccountId = 1,
                ToAccountId = 2,
                Amount = 100,
                Date = DateTime.UtcNow
            };

            await _transactionService.CreateTransactionAsync(transaction);

            _transactionRepositoryMock.Verify(x => x.AddTransactionAsync(transaction), Times.Once);
        }
    }
}