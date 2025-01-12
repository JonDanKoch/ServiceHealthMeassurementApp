

using ServiceHealthMeassurementApp.Utils;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ServiceHealthMeassurementApp.Services
{
    public class ServiceDiscoveryService : IHostedService, IDisposable
    {
        private Timer? _timer;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(1); // 1 minutes interval


        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(ExecutePeriodicTask, null, TimeSpan.Zero, _interval);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Clean up 
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private void ExecutePeriodicTask(object state)
        {
            DiscoveryServicesAsync();
        }

        private async Task DiscoveryServicesAsync()
        {
            // Set all service availabilities to false
            foreach (var key in DiscoveredServiceHealthRepo.serviceAvailabilities.Keys.ToList())
            {
                DiscoveredServiceHealthRepo.serviceAvailabilities[key] = false;
            }

            var urls = ServiceUrlRepo.Urls ?? new List<string>();
            foreach (var url in urls)
            {
                string apiUrl = $"https://{url}/api/v1/pods"; 
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, apiUrl);

                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "<your-token>");

                // Call the Kubernetes API
                var client = new HttpClient();
                var response = await client.SendAsync(requestMessage);
                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response to a .NET object (e.g., Pods list)
                    var content = await response.Content.ReadAsStringAsync();
                    dynamic contentObject = JsonConvert.DeserializeObject(content);

                    // var services = ...
                    // foreach (var serviceName in serviceNames) DiscoveredServiceHealthRepo.serviceAvailabilities[serviceName] = true; Console.log(serviceName + "is available")
                    // Für alle nicht verfügbaren auch mal loggen

                }
            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}