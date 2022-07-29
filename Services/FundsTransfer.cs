using HarbintonApi.Interfaces;
using HarbintonApi.RequestModels;
using HarbintonApi.ResponseModels;

namespace HarbintonApi.Services
{
    public class FundsTransfer:IFundTransfer
    {
        private readonly IConfiguration _configuration;
        private readonly ISql _sqlconnection;
        public FundsTransfer(IConfiguration configuration, ISql sqlconnection)
        {
            _configuration = configuration;
            _sqlconnection = sqlconnection;
        }
        public async Task<TransferResponse> TransferFunds(TransferRequest transferDetails)
        {
            TransferResponse transferResponse = new TransferResponse();
            try
            {
                string currentBankCode = _configuration.GetSection("CurrentBankCode").Value;
                if (transferDetails.senderBankCode == currentBankCode && transferDetails.receiverBankCode == currentBankCode)
                {
                    //For transactions within Harbinton
                    if (await _sqlconnection.CheckForExistingCustomer(transferDetails.senderNuban) && await _sqlconnection.CheckForExistingCustomer(transferDetails.receiverNuban))
                    {
                        Task<string> senderOldAccountNumber = _sqlconnection.ConvertToOldAccount(transferDetails.senderNuban);
                        Task<string> receiverOldAccountNumber = _sqlconnection.ConvertToOldAccount(transferDetails.receiverNuban);
                        string senderAccountNumber = await senderOldAccountNumber;
                        string receiverAccountNumber = await receiverOldAccountNumber;
                        if (senderAccountNumber.Contains("/") && receiverAccountNumber.Contains("/"))
                        {
                            string[] senderAcct = senderAccountNumber.Split('/');
                            string[] receiverAcct = receiverAccountNumber.Split('/');
                            if (Convert.ToDecimal(senderAcct[5]) >= transferDetails.amount)
                            {
                                SqlTransferFunds sqlTransferDetails = new SqlTransferFunds();
                                sqlTransferDetails.senderBraCode = Convert.ToInt32(senderAcct[0]);
                                sqlTransferDetails.senderCustNum = Convert.ToInt32(senderAcct[1]);
                                sqlTransferDetails.senderCurCode = Convert.ToInt32(senderAcct[2]);
                                sqlTransferDetails.senderLedCode = Convert.ToInt32(senderAcct[3]);
                                sqlTransferDetails.senderSubAcctCode = Convert.ToInt32(senderAcct[4]);
                                sqlTransferDetails.receiverBraCode = Convert.ToInt32(receiverAcct[0]);
                                sqlTransferDetails.receiverCustNum = Convert.ToInt32(receiverAcct[1]);
                                sqlTransferDetails.receiverCurCode = Convert.ToInt32(receiverAcct[2]);
                                sqlTransferDetails.receiverLedCode = Convert.ToInt32(receiverAcct[3]);
                                sqlTransferDetails.receiverSubAcctCode = Convert.ToInt32(receiverAcct[4]);
                                sqlTransferDetails.amount = transferDetails.amount;
                                bool postResult = await _sqlconnection.TransferFunds(sqlTransferDetails);
                                if (postResult)
                                {
                                    transferResponse.responseCode = "00";
                                    transferResponse.responseMessage = "Transaction Successful";
                                }
                                else
                                {
                                    transferResponse.responseCode = "02";
                                    transferResponse.responseMessage = "transaction Unsuccessful";
                                }
                            }
                            else
                            {
                                transferResponse.responseCode = "01";
                                transferResponse.responseMessage = "Insufficient Account Balance";
                            }
                        }
                    }
                    else
                    {
                        transferResponse.responseCode = "01";
                        transferResponse.responseMessage = "Customer does not exist. invalid bank code";
                    }
                    
                }
            }
            catch (Exception ex)
            {

            }
            return transferResponse;
        }
    }
}
