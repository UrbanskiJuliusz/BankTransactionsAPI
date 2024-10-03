namespace BankTransferApi.Models
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public string FromAccountNumber { get; set; } = null!;
        public string ToAccountNumber { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
