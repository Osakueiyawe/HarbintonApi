using System.ComponentModel.DataAnnotations;

namespace HarbintonApi.RequestModels
{
    public class TransferRequest
    {
        [Required]
        public string senderNuban { get; set; }
        [Required]
        public decimal amount { get; set; }
        [Required]
        public string receiverNuban { get; set; }
        [Required]
        public string senderBankCode { get; set; }
        [Required]
        public string receiverBankCode { get; set; }
    }
}
