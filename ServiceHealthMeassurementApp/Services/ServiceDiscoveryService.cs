using ServiceHealthMeassurementApp.Utils;
using System.Net.Http.Headers;
using ServiceHealthMeassurementApp.Models;
using System.Text.RegularExpressions;

namespace ServiceHealthMeassurementApp.Services
{
    /// <summary>
    /// IHostedService for discovering kubernetes services
    /// </summary>
    public class ServiceDiscoveryService : IHostedService
    {
        private Timer? _timer;
        private TimeSpan _interval = TimeSpan.FromSeconds(30); // 30 seconds as dinterval

        /// <summary>
        /// Startup of IHostedService
        /// </summary>
        /// <param name="cancellationToken">cancelation token.</param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(ExecutePeriodicTask, null, TimeSpan.Zero, _interval);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Stop of IHostedService
        /// </summary>
        /// <param name="cancellationToken">cancelation token.</param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Clean up 
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Periodically executed method
        /// </summary>
        /// <param name="state">state.</param>
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
            List<string> activeServiceNames = new List<string>();
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
                    var content = await response.Content.ReadAsStringAsync();
                    var serviceNamePattern = @".*""name"": ""(.*?)"",";
                    var serviceNameRegex = new Regex(serviceNamePattern);
                    var serviceNameMatches = serviceNameRegex.Matches(content);
                    
                    foreach (Match match in serviceNameMatches)
                    {
                        if (match.Success)
                        {
                            var discoveredServiceName = match.Groups[1].Value;
                            Console.WriteLine("Available service named: " + discoveredServiceName);
                            activeServiceNames.Add(discoveredServiceName);
                        }
                    }
                }
            }

            foreach (var serviceName in DiscoveredServiceHealthRepo.serviceAvailabilities.Keys)
            {
                DiscoveredServiceHealthRepo.serviceAvailabilities[serviceName] = activeServiceNames.Contains(serviceName);
            }
        }
    }
}