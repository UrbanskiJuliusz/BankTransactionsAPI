using Microsoft.AspNetCore.Mvc;
using BankTransferApi.Models;
using BankTransferApi.Services;

namespace BankTransferApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransferController : ControllerBase
    {
        private readonly ITransferService _transferService;

        public TransferController(ITransferService transferService)
        {
            _transferService = transferService;
        }

        [HttpPost]
        public async Task<IActionResult> Transfer([FromBody] TransferRequest request)
        {
            var result = await _transferService.TransferAsync(request.FromAccountId, request.ToAccountId, request.Amount);
            if (result.Success)
            {
                return Ok("Transfer completed successfully");
            }
            return BadRequest(result.ErrorMessage ?? "Transfer failed");
        }
    }
}