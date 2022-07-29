using HarbintonApi.Interfaces;
using HarbintonApi.RequestModels;
using HarbintonApi.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HarbintonApi.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class BillsPaymentController : ControllerBase
    {
        private readonly IBillProcessing _billProcessing;
        public BillsPaymentController(IBillProcessing billProcessing)
        {
            _billProcessing = billProcessing;
        }
        [HttpPost]
        public async Task<IActionResult> PayBill(BillPaymentRequest billPaymentDetails)
        {
            if (ModelState.IsValid)
            {
                BillPaymentResponse billPaymentResponse = await _billProcessing.ProcessBillRequest(billPaymentDetails);
                return Ok(billPaymentResponse);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
