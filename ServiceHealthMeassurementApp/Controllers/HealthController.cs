using Microsoft.AspNetCore.Mvc;

namespace ServiceHealthMeassurementApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        public HealthController()
        {
        }

        [HttpGet(Name = "Health")]
        public IActionResult Get()
        {
            return new OkResult();
        }
    }
}