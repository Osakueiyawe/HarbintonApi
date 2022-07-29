namespace HarbintonApi.RequestModels
{
    public class BillPaymentRequest
    {
        public string utilityVendorCode { get; set; }
        public string customerNuban { get; set; }
        public decimal amount { get; set; }
        public string customerBankCode { get; set; }
    }
}
