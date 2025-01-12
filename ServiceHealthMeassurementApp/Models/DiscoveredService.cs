using System.Text.Json.Serialization;

namespace ServiceHealthMeassurementApp.Models
{
    public class DiscoveredService
    {
        public DiscoveredService(string serviceName, bool serviceAvailable)
        {
            ServiceName = serviceName;
            ServiceAvailable = serviceAvailable;
        }

        [JsonPropertyName("service-name")]
        public String ServiceName { get; set; }

        [JsonPropertyName("service-available")]
        public bool ServiceAvailable { get; set; }
    }
}
