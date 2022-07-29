using HarbintonApi.RequestModels;

namespace HarbintonApi.Interfaces
{
    public interface ISql
    {
        Task<bool> CheckForExistingCustomer(string nuban);
        Task<string> ConvertToOldAccount(string nuban);
        Task<bool> TransferFunds(SqlTransferFunds transferDetails);
        Task<string> GetBillPaymentDetails(string utilityId);
    }
}
