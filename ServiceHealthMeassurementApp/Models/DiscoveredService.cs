using System.Text.Json.Serialization;

namespace ServiceHealthMeassurementApp.Models
{
    /// <summary>
    /// Stores information about a discovered Kubernetes service.
    /// </summary>
    public class DiscoveredService
    {
        /// <summary>
        /// Initialize field.
        /// </summary>
        /// <param name="serviceName">serviceName.</param>
        /// <param name="serviceAvailable">if service is available.</param>
        public DiscoveredService(string serviceName, bool serviceAvailable)
        {
            ServiceName = serviceName;
            ServiceAvailable = serviceAvailable;
        }

        /// <summary>
        /// Name of the service.
        /// </summary>
        [JsonPropertyName("service-name")]
        public String ServiceName { get; set; }

        /// <summary>
        /// Value indicating whether the service is available.
        /// </summary>
        [JsonPropertyName("service-available")]
        public bool ServiceAvailable { get; set; }
    }
}
