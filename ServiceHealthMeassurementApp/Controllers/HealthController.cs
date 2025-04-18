using Microsoft.AspNetCore.Mvc;
using ServiceHealthMeassurementApp.Models;
using ServiceHealthMeassurementApp.Utils;

namespace ServiceHealthMeassurementApp.Controllers
{
    /// <summary>
    /// Class for all routes providing info about the health of services or making modifications on health discovery.
    /// </summary>
    [ApiController]
    [Route("api/health")]
    public class HealthController : ControllerBase
    {

        /// <summary>
        /// Checks health of this API.
        /// </summary>
        /// <returns>Statuscode 200 if service is available.</returns>
        [HttpGet("own-health")]
        public IActionResult Get_Own_Health()
        {
            return new OkResult();
        }

        /// <summary>
        /// Displays health of all discovered Kubernetes services.
        /// </summary>
        /// <returns>health of discovered services.</returns>
        [HttpGet("discovered-services-health")]
        public List<DiscoveredService> Get_Discovered_Services_Health()
        {
            List<DiscoveredService> services = new List<DiscoveredService>();
            foreach (var serviceName in DiscoveredServiceHealthRepo.ServiceAvailabilities.Keys)
            {
                services.Add(new DiscoveredService(serviceName, DiscoveredServiceHealthRepo.ServiceAvailabilities[serviceName]));
            }
            return services;
        }

        /// <summary>
        /// Discoveres the services of the cluster that can be accessed by the given url.
        /// </summary>
        /// <param name="url">url for service discovery.</param>
        /// <param name="bearerToken">access token for given url.</param>
        [HttpPost("service-discovery-urls")]
        public IActionResult AddServiceDiscoveryUrl([FromQuery] string url, [FromHeader] string bearerToken) 
        {
            ServiceUrlRepo.Urls.Add(new ServiceAccess(url, bearerToken));
            return new OkResult();
        }

        /// <summary>
        /// Change intervall of service discovery.
        /// </summary>
        /// <param name="duration">new service discovery intervall duration.</param>
        [HttpPut("service-discovery-period-seconds")]
        public IActionResult ModifyServiceDiscoveryPeriod([FromQuery] int duration)
        {
            DiscoveredServiceHealthRepo.ServiceDiscoveryIntervallSeconds = duration;
            return new OkResult();
        }
    }
}