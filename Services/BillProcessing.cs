using HarbintonApi.Interfaces;
using HarbintonApi.RequestModels;
using HarbintonApi.ResponseModels;

namespace HarbintonApi.Services
{
    public class BillProcessing:IBillProcessing
    {
        private readonly ISql _sqlconnection;
        private readonly IConfiguration _configuration;
        public BillProcessing(ISql sqlconnection, IConfiguration configuration)
        {
            _sqlconnection = sqlconnection;
            _configuration = configuration;
        }
        public async Task<BillPaymentResponse> ProcessBillRequest(BillPaymentRequest billPaymentDetails)
        {
            BillPaymentResponse response = new BillPaymentResponse();
            SqlTransferFunds sqlTransferDetails = new SqlTransferFunds();
            try
            {
                if (billPaymentDetails != null && billPaymentDetails.customerBankCode == _configuration.GetSection("CurrentBankCode").Value)
                {
                    string accountToCredit = await _sqlconnection.GetBillPaymentDetails(billPaymentDetails.utilityVendorCode);
                    string accountToDebit = await _sqlconnection.ConvertToOldAccount(billPaymentDetails.customerNuban);
                    if (accountToDebit.Contains("/"))
                    {
                        if (accountToCredit.Contains("/"))
                        {
                            string[] creditOldAccountNumber = accountToCredit.Split('/');
                            string[] debitOldAccountNumber = accountToDebit.Split('/');
                            sqlTransferDetails.amount = billPaymentDetails.amount;
                            sqlTransferDetails.receiverBraCode = Convert.ToInt32(creditOldAccountNumber[0]);
                            sqlTransferDetails.receiverCustNum = Convert.ToInt32(creditOldAccountNumber[1]);
                            sqlTransferDetails.receiverCurCode = Convert.ToInt32(creditOldAccountNumber[2]);
                            sqlTransferDetails.receiverLedCode = Convert.ToInt32(creditOldAccountNumber[3]);
                            sqlTransferDetails.receiverSubAcctCode = Convert.ToInt32(creditOldAccountNumber[4]);
                            sqlTransferDetails.senderBraCode = Convert.ToInt32(debitOldAccountNumber[0]);
                            sqlTransferDetails.senderCustNum = Convert.ToInt32(debitOldAccountNumber[1]);
                            sqlTransferDetails.senderCurCode = Convert.ToInt32(debitOldAccountNumber[2]);
                            sqlTransferDetails.senderLedCode = Convert.ToInt32(debitOldAccountNumber[3]);
                            sqlTransferDetails.senderSubAcctCode = Convert.ToInt32(debitOldAccountNumber[4]);
                            bool postResult = await _sqlconnection.TransferFunds(sqlTransferDetails);
                            if (postResult)
                            {
                                response.responseCode = "00";
                                response.responseMessage = "successful";
                            }
                        }
                    }
                    else
                    {
                        response.responseCode = "01";
                        response.responseMessage = "Customer Account Number does not exist, use another bank code";
                    }

                }
                else
                {
                    response.responseCode = "01";
                    response.responseMessage = "Invalid Vendor Code";
                }
            }
            catch (Exception ex)
            {
                response.responseCode = "02";
                response.responseMessage = "server error";
            }           
            
            return response;
        }
    }
}
