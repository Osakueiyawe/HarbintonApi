using HarbintonApi.Interfaces;
using HarbintonApi.RequestModels;
using HarbintonApi.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HarbintonApi.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class TransferServiceController : ControllerBase
    {
        private readonly IFundTransfer _fundtransfer;
        public TransferServiceController(IFundTransfer fundtransfer)
        {
            _fundtransfer = fundtransfer;
        }
        [HttpPost]
        public async Task<IActionResult> TransferFunds(TransferRequest transferDetails)
        {
            if (ModelState.IsValid)
            {
                TransferResponse transferResponse = await _fundtransfer.TransferFunds(transferDetails);
                return Ok(transferResponse);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
