namespace HarbintonApi.ResponseModels
{
    public class BillPaymentResponse
    {
        public string responseCode { get; set; }
        public string responseMessage { get; set; }
        public BillPaymentDetails billPaymentDetails { get; set; }
    }

    public class BillPaymentDetails
    {
        public string transactionSequence { get; set; }
    }
}
