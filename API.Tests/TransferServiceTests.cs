using Xunit;
using BankTransferApi.Models;
using BankTransferApi.Repositories;
using BankTransferApi.Services;
using BankTransferApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;

namespace BankTransferApi.Tests
{
    public class TransferServiceTests
    {
        private readonly BankContext _context;
        private readonly Mock<ITransactionService> _transactionServiceMock;
        private readonly TransferService _transferService;

        public TransferServiceTests()
        {
            var options = new DbContextOptionsBuilder<BankContext>()
                .UseInMemoryDatabase(databaseName: "TestBankDb")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            _context = new BankContext(options);
            var accountRepository = new AccountRepository(_context);

            _transactionServiceMock = new Mock<ITransactionService>();
            _transferService = new TransferService(accountRepository, _transactionServiceMock.Object, _context);
        }

        [Fact]
        public async Task Transfer_ShouldSucceed_WhenAccountsExistAndBalanceIsSufficient()
        {
            _context.Accounts.Add(new Account { Id = 1, AccountNumber = "123456", Balance = 1000m });
            _context.Accounts.Add(new Account { Id = 2, AccountNumber = "654321", Balance = 500m });
            _context.SaveChanges();

            var transferAmount = 300m;

            var result = await _transferService.TransferAsync(1, 2, transferAmount);

            var fromAccount = await _context.Accounts.FindAsync(1);
            var toAccount = await _context.Accounts.FindAsync(2);

            Assert.True(result.Success);
            Assert.Equal(700m, fromAccount?.Balance);
            Assert.Equal(800m, toAccount?.Balance);

            _transactionServiceMock.Verify(x => x.CreateTransactionAsync(It.Is<Transaction>(t =>
                t.FromAccountId == 1 &&
                t.ToAccountId == 2 &&
                t.Amount == transferAmount
            )), Times.Once);
        }

        [Fact]
        public async Task Transfer_ShouldFail_WhenFromAccountDoesNotExist()
        {
            _context.Accounts.Add(new Account { Id = 4, AccountNumber = "123", Balance = 500m });
            _context.SaveChanges();

            var transferAmount = 300m;

            var result = await _transferService.TransferAsync(3, 4, transferAmount);

            Assert.False(result.Success);
            Assert.Equal("One or both accounts do not exist.", result.ErrorMessage);

            var toAccount = await _context.Accounts.FindAsync(4);
            Assert.Equal(500m, toAccount?.Balance);

            _transactionServiceMock.Verify(x => x.CreateTransactionAsync(It.IsAny<Transaction>()), Times.Never);
        }

        [Fact]
        public async Task Transfer_ShouldFail_WhenToAccountDoesNotExist()
        {
            _context.Accounts.Add(new Account { Id = 5, AccountNumber = "321", Balance = 1000m });
            _context.SaveChanges();

            var transferAmount = 300m;

            var result = await _transferService.TransferAsync(5, 6, transferAmount);

            Assert.False(result.Success);
            Assert.Equal("One or both accounts do not exist.", result.ErrorMessage);

            var fromAccount = await _context.Accounts.FindAsync(5);
            Assert.Equal(1000m, fromAccount?.Balance);

            _transactionServiceMock.Verify(x => x.CreateTransactionAsync(It.IsAny<Transaction>()), Times.Never);
        }

        [Fact]
        public async Task Transfer_ShouldFail_WhenInsufficientFunds()
        {
            _context.Accounts.Add(new Account { Id = 7, AccountNumber = "987", Balance = 100m });
            _context.Accounts.Add(new Account { Id = 8, AccountNumber = "789", Balance = 500m });
            _context.SaveChanges();

            var transferAmount = 300m;

            var result = await _transferService.TransferAsync(7, 8, transferAmount);

            Assert.False(result.Success);
            Assert.Equal("Insufficient funds.", result.ErrorMessage);

            var fromAccount = await _context.Accounts.FindAsync(7);
            var toAccount = await _context.Accounts.FindAsync(8);

            Assert.Equal(100m, fromAccount?.Balance);
            Assert.Equal(500m, toAccount?.Balance);

            _transactionServiceMock.Verify(x => x.CreateTransactionAsync(It.IsAny<Transaction>()), Times.Never);
        }
    }
}