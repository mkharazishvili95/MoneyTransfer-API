namespace MoneyTransfer_API.Models
{
    public class TransferModel
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public double Amount { get; set; }
    }
}
