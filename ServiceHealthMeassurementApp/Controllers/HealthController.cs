using Microsoft.AspNetCore.Mvc;
using ServiceHealthMeassurementApp.Models;
using ServiceHealthMeassurementApp.Utils;

namespace ServiceHealthMeassurementApp.Controllers
{
    [ApiController]
    [Route("api/health")]
    public class HealthController : ControllerBase
    {
        public HealthController()
        {
        }

        [HttpGet("own-health")]
        public IActionResult Get_Own_Health()
        {
            return new OkResult();
        }

        [HttpGet("discovered-services-health")]
        public List<DiscoveredService> Get_Discovered_Services_Health()
        {
            List<DiscoveredService> services = new List<DiscoveredService>();
            foreach (var serviceName in DiscoveredServiceHealthRepo.serviceAvailabilities.Keys)
            {
                services.Add(new DiscoveredService(serviceName, DiscoveredServiceHealthRepo.serviceAvailabilities[serviceName]));
            }
            return services;
        }

        [HttpPost("service-discovery-urls")]
        public IActionResult AddServiceDiscoveryUrl([FromQuery] string url) 
        {
            ServiceUrlRepo.Urls.Add(url);
            return new OkResult();
        }
    }
}