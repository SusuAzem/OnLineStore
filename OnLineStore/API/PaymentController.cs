using Core;

using Microsoft.AspNetCore.Mvc;

namespace OnLineStore.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        [HttpPost("callback")]
        public IActionResult HandleCallback([FromBody] Payment result)
        {
            // 1. Validate the signature/token if provided by ShamCash
            if (result.Status == "success")
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
