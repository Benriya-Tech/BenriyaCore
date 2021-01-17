using Benriya.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Benriya.App
{
    [Route("/errors")]
    [ApiController]
    [Produces("application/json")]
    public class ErrorsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index(int code)
        {
            return BadRequest(new ApiResultError(code));
        }

        [HttpGet]
        [Route("{code}")]
        public IActionResult Error(int code)
        {
            return BadRequest(new ApiResultError(code));
        }
    }
}