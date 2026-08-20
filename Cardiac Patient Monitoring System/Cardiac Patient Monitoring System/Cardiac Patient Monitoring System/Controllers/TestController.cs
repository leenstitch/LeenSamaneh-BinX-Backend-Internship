// This controller provides a simple test endpoint for verifying
// the application's global exception-handling middleware.
//For Week5 - Day 4 

using Microsoft.AspNetCore.Mvc;

namespace Cardiac_Patient_Monitoring_System.Controllers
{
    [ApiController]
    [Route("api/v1/TestController")]
    public class TestController : ControllerBase
    {
        // Throws a test exception to verify that the global
        // exception-handling middleware catches and handles it.
        [HttpGet("error")]
        public IActionResult TestError()
        {
            throw new Exception("This is a test exception");
        }
    }
}