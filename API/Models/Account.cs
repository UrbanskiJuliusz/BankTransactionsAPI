namespace BankTransferApi.Models
{
    public class Account
    {
        public int Id { get; set; }
        public required string AccountNumber { get; set; }
        public decimal Balance { get; set; }

        public List<Transaction> FromTransactions { get; set; } = new List<Transaction>();
        public List<Transaction> ToTransactions { get; set; } = new List<Transaction>();
    }
}