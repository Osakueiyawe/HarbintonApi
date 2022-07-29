namespace HarbintonApi.RequestModels
{
    public class SqlTransferFunds
    {
        public int senderBraCode { get; set; }
        public int senderCustNum { get; set; }
        public int senderCurCode { get; set; }
        public int senderLedCode { get; set; }
        public int senderSubAcctCode { get; set; }
        public int receiverBraCode { get; set; }
        public int receiverCustNum { get; set; }
        public int receiverCurCode { get; set; }
        public int receiverLedCode { get; set; }
        public int receiverSubAcctCode { get; set; }
        public decimal amount { get; set; }
    }
}
