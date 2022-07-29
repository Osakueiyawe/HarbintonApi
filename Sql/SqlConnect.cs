using HarbintonApi.Interfaces;
using HarbintonApi.RequestModels;
using System.Data;
using System.Data.SqlClient;

namespace HarbintonApi.Sql
{
    public class SqlConnect:ISql
    {
        private IConfiguration _configuration;
        public SqlConnect(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> CheckForExistingCustomer(string nuban)
        {
            bool result = false;
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(_configuration.GetSection("SqlConnection").Value))
                {                   
                    
                    if (sqlcon.State != ConnectionState.Open)
                    {
                        sqlcon.Open();
                    }
                    using (SqlCommand sqlcmd = new SqlCommand("checkifcustomerexists", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.Add("@nuban", SqlDbType.NVarChar).Value = nuban;
                        SqlDataAdapter sdap = new SqlDataAdapter(sqlcmd);
                        sdap.Fill(dt);
                    }                   
                    
                } 
                if (dt.Rows.Count > 0)
                {
                    result = true;
                }
            }
            catch(Exception ex)
            {

            }
            return result;
        }

        public async Task<string> ConvertToOldAccount(string nuban)
        {
            string result = "";
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(_configuration.GetSection("SqlConnection").Value))
                {

                    if (sqlcon.State != ConnectionState.Open)
                    {
                        sqlcon.Open();
                    }
                    using (SqlCommand sqlcmd = new SqlCommand("getoldaccountnumber", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.Add("@nuban", SqlDbType.NVarChar).Value = nuban;
                        SqlDataAdapter sdap = new SqlDataAdapter(sqlcmd);
                        sdap.Fill(dt);
                    }

                }
                if (dt.Rows.Count > 0)
                {
                    result = dt.Rows[0]["BranchCode"].ToString() + "/" + dt.Rows[0]["CustomerNumber"].ToString() + "/" + dt.Rows[0]["CurrencyCode"].ToString() + "/" + dt.Rows[0]["LedgerCode"].ToString() + "/" + dt.Rows[0]["SubAccountCode"].ToString() + "/" + dt.Rows[0]["AccountBalance"].ToString();
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<bool> TransferFunds(SqlTransferFunds transferDetails)
        {
            bool result = false;            
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(_configuration.GetSection("SqlConnection").Value))
                {

                    if (sqlcon.State != ConnectionState.Open)
                    {
                        sqlcon.Open();
                    }
                    using (SqlCommand sqlcmd = new SqlCommand("fundstransfer", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.Add("@senderBraCode", SqlDbType.Int).Value = transferDetails.senderBraCode;
                        sqlcmd.Parameters.Add("@senderCustNum", SqlDbType.Int).Value = transferDetails.senderCustNum;
                        sqlcmd.Parameters.Add("@senderCurCode", SqlDbType.Int).Value = transferDetails.senderCurCode;
                        sqlcmd.Parameters.Add("@senderLedCode", SqlDbType.Int).Value = transferDetails.senderLedCode;
                        sqlcmd.Parameters.Add("@senderSubAcctCode", SqlDbType.Int).Value = transferDetails.senderSubAcctCode;
                        sqlcmd.Parameters.Add("@receiverBraCode", SqlDbType.Int).Value = transferDetails.receiverBraCode;
                        sqlcmd.Parameters.Add("@receiverCustNum", SqlDbType.Int).Value = transferDetails.receiverCustNum;
                        sqlcmd.Parameters.Add("@receiverCurCode", SqlDbType.Int).Value = transferDetails.receiverCurCode;
                        sqlcmd.Parameters.Add("@receiverLedCode", SqlDbType.Int).Value = transferDetails.receiverLedCode;
                        sqlcmd.Parameters.Add("@receiverSubAcctCode", SqlDbType.Int).Value = transferDetails.receiverSubAcctCode;
                        sqlcmd.Parameters.Add("@amount", SqlDbType.Decimal).Value = transferDetails.amount;
                        sqlcmd.ExecuteNonQuery();
                        result = true;
                    }

                }                
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<string> GetBillPaymentDetails(string utilityId)
        {
            string result = "";
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(_configuration.GetSection("SqlConnection").Value))
                {

                    if (sqlcon.State != ConnectionState.Open)
                    {
                        sqlcon.Open();
                    }
                    using (SqlCommand sqlcmd = new SqlCommand("getutilitydetails", sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.Add("@utilityid", SqlDbType.Int).Value = Convert.ToInt32(utilityId);
                        SqlDataAdapter sdap = new SqlDataAdapter(sqlcmd);
                        sdap.Fill(dt);
                    }

                }
                if (dt.Rows.Count > 0)
                {
                    result = dt.Rows[0]["BranchCode"].ToString() + "/" + dt.Rows[0]["CustomerNumber"].ToString() + "/" + dt.Rows[0]["CurrencyCode"].ToString() + "/" + dt.Rows[0]["LedgerCode"].ToString() + "/" + dt.Rows[0]["SubAccountCode"].ToString();
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
    }
}
