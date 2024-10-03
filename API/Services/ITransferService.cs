namespace BankTransferApi.Services
{
    public interface ITransferService
    {
        Task<TransferResult> TransferAsync(int fromAccountId, int toAccountId, decimal amount);
    }
}