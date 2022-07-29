using HarbintonApi.RequestModels;
using HarbintonApi.ResponseModels;

namespace HarbintonApi.Interfaces
{
    public interface IFundTransfer
    {
        Task<TransferResponse> TransferFunds(TransferRequest transferDetails);
    }
}
