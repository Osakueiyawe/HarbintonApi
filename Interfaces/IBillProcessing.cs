using HarbintonApi.RequestModels;
using HarbintonApi.ResponseModels;

namespace HarbintonApi.Interfaces
{
    public interface IBillProcessing
    {
        Task<BillPaymentResponse> ProcessBillRequest(BillPaymentRequest billPaymentDetails);
    }
}
