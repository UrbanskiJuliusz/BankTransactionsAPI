namespace BankTransferApi.Models
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; } = null!;
        public decimal Balance { get; set; }
        public List<TransactionDto> FromTransactions { get; set; } = new List<TransactionDto>();
        public List<TransactionDto> ToTransactions { get; set; } = new List<TransactionDto>();
    }
}