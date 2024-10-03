using AutoMapper;
using BankTransferApi.Models;
using BankTransferApi.Repositories;
using BankTransferApi.Services;
using Moq;
using Xunit;

namespace BankTransferApi.Tests
{
    public class AccountServiceTests
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AccountService _accountService;

        public AccountServiceTests()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _mapperMock = new Mock<IMapper>();
            _accountService = new AccountService(_accountRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllAccountsAsync_ShouldReturnAllAccounts()
        {
            // Przykładowe dane z repozytorium
            var accounts = new List<Account>
            {
                new Account { Id = 1, AccountNumber = "123", Balance = 100 },
                new Account { Id = 2, AccountNumber = "456", Balance = 200 }
            };
            var accountsDto = new List<AccountDto>
            {
                new AccountDto { Id = 1, AccountNumber = "123", Balance = 100 },
                new AccountDto { Id = 2, AccountNumber = "456", Balance = 200 }
            };

            // Mockowanie repozytorium i AutoMapper
            _accountRepositoryMock.Setup(repo => repo.GetAllAccountsAsync())
                                  .ReturnsAsync(accounts);

            _mapperMock.Setup(m => m.Map<IEnumerable<AccountDto>>(accounts))
                       .Returns(accountsDto);

            // Wywołanie metody
            var result = await _accountService.GetAllAccountsAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _accountRepositoryMock.Verify(repo => repo.GetAllAccountsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAccountByIdAsync_ShouldReturnAccount_WhenAccountExists()
        {
            var account = new Account { Id = 1, AccountNumber = "123", Balance = 100 };
            var accountDto = new AccountDto { Id = 1, AccountNumber = "123", Balance = 100 };

            _accountRepositoryMock.Setup(repo => repo.GetAccountByIdAsync(1))
                                  .ReturnsAsync(account);

            _mapperMock.Setup(m => m.Map<AccountDto>(account))
                       .Returns(accountDto);

            var result = await _accountService.GetAccountByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result?.Id);
            Assert.Equal("123", result?.AccountNumber);
            _accountRepositoryMock.Verify(repo => repo.GetAccountByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetAccountByIdAsync_ShouldReturnNull_WhenAccountDoesNotExist()
        {
            _accountRepositoryMock.Setup(repo => repo.GetAccountByIdAsync(1))
                                  .ReturnsAsync((Account?)null);

            var result = await _accountService.GetAccountByIdAsync(1);

            Assert.Null(result);
            _accountRepositoryMock.Verify(repo => repo.GetAccountByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task CreateAccountAsync_ShouldCallRepository_AddAccountAsync()
        {
            var newAccount = new Account { Id = 1, AccountNumber = "123", Balance = 100 };

            await _accountService.CreateAccountAsync(newAccount);

            _accountRepositoryMock.Verify(repo => repo.AddAccountAsync(newAccount), Times.Once);
        }

        [Fact]
        public async Task UpdateAccountAsync_ShouldUpdateAccount_WhenAccountExists()
        {
            var existingAccount = new Account { Id = 1, AccountNumber = "123", Balance = 100 };
            var updatedAccount = new Account { Id = 1, AccountNumber = "999", Balance = 200 };

            _accountRepositoryMock.Setup(repo => repo.GetAccountByIdAsync(1))
                                  .ReturnsAsync(existingAccount);

            await _accountService.UpdateAccountAsync(1, updatedAccount);

            _accountRepositoryMock.Verify(repo => repo.GetAccountByIdAsync(1), Times.Once);
            _accountRepositoryMock.Verify(repo => repo.UpdateAccountAsync(existingAccount), Times.Once);

            Assert.Equal("999", existingAccount.AccountNumber);
            Assert.Equal(200, existingAccount.Balance);
        }

        [Fact]
        public async Task UpdateAccountAsync_ShouldNotUpdateAccount_WhenAccountDoesNotExist()
        {
            var updatedAccount = new Account { Id = 1, AccountNumber = "999", Balance = 200 };

            _accountRepositoryMock.Setup(repo => repo.GetAccountByIdAsync(1))
                                  .ReturnsAsync((Account?)null);

            await _accountService.UpdateAccountAsync(1, updatedAccount);

            _accountRepositoryMock.Verify(repo => repo.GetAccountByIdAsync(1), Times.Once);
            _accountRepositoryMock.Verify(repo => repo.UpdateAccountAsync(It.IsAny<Account>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAccountAsync_ShouldCallRepository_DeleteAccountAsync()
        {
            await _accountService.DeleteAccountAsync(1);

            _accountRepositoryMock.Verify(repo => repo.DeleteAccountAsync(1), Times.Once);
        }
    }
}