namespace HarbintonApi.ResponseModels
{
    public class TransferResponse
    {
        public string responseCode { get; set; }
        public string responseMessage { get; set; }
        public TransferDetails? transferDetails { get; set; }
    }

    public class TransferDetails
    {
        public string transferSequence { get; set; }
    }
}
