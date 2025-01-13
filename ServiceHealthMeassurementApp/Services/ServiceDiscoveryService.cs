using ServiceHealthMeassurementApp.Utils;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using ServiceHealthMeassurementApp.Models;

namespace ServiceHealthMeassurementApp.Services
{
    public class ServiceDiscoveryService : IHostedService, IDisposable
    {
        private Timer? _timer;
        private TimeSpan _interval = TimeSpan.FromMinutes(1); // 1 minutes as dinterval


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

        private async void ExecutePeriodicTask(object state)
        {
            await DiscoveryServicesAsync();
            var configeredIntervallSeconds = DiscoveredServiceHealthRepo.serviceDiscoveryIntervallSeconds;

            if ((int)_interval.TotalSeconds != configeredIntervallSeconds)
            {
                _interval = TimeSpan.FromSeconds(configeredIntervallSeconds);
                _timer = new Timer(ExecutePeriodicTask, null, TimeSpan.Zero, _interval);
            }
        }

        private async Task DiscoveryServicesAsync()
        {
            // Set all service availabilities to false
            foreach (var key in DiscoveredServiceHealthRepo.serviceAvailabilities.Keys.ToList())
            {
                DiscoveredServiceHealthRepo.serviceAvailabilities[key] = false;
            }

            var serviceAccesses = ServiceUrlRepo.Urls ?? new List<ServiceAccess>();
            foreach (var access in serviceAccesses)
            {
                string apiUrl = $"https://{access.Url}/api/v1/services";
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, apiUrl);

                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", access.BearerToken);

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