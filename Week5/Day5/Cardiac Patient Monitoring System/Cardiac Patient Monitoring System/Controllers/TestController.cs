using Microsoft.AspNetCore.Mvc;

namespace Cardiac_Patient_Monitoring_System.Controllers
{
    [ApiController]
    [Route("api/v1/TestController")]
    public class TestController : ControllerBase
    {
        [HttpGet("error")]
        public IActionResult TestError()
        {
            throw new Exception("This is a test exception");
        }
    }
}