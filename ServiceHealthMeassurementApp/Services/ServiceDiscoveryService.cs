using ServiceHealthMeassurementApp.Utils;
using System.Net.Http.Headers;
using ServiceHealthMeassurementApp.Models;
using System.Text.RegularExpressions;

namespace ServiceHealthMeassurementApp.Services
{
    /// <summary>
    /// IHostedService for discovering Kubernetes services.
    /// </summary>
    public class ServiceDiscoveryService : IHostedService
    {
        private Timer? _timer;
        private TimeSpan _interval = TimeSpan.FromSeconds(30); // 30 seconds as default interval

        /// <summary>
        /// Startup of IHostedService.
        /// </summary>
        /// <param name="cancellationToken">cancelation token.</param>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(ExecutePeriodicTask, null, TimeSpan.Zero, _interval);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Stop of IHostedService.
        /// </summary>
        /// <param name="cancellationToken">cancelation token.</param>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Clean up
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Periodically executed method.
        /// </summary>
        /// <param name="state">state.</param>
        private async void ExecutePeriodicTask(object state)
        {
            await DiscoveryServicesAsync();
            var configeredIntervallSeconds = DiscoveredServiceHealthRepo.ServiceDiscoveryIntervallSeconds;

            if ((int)_interval.TotalSeconds != configeredIntervallSeconds)
            {
                _interval = TimeSpan.FromSeconds(configeredIntervallSeconds);
                _timer = new Timer(ExecutePeriodicTask, null, TimeSpan.Zero, _interval);
            }
        }

        private async Task DiscoveryServicesAsync()
        {
            // Set all service availabilities to false before starting new discovery
            foreach (var key in DiscoveredServiceHealthRepo.ServiceAvailabilities.Keys.ToList())
            {
                DiscoveredServiceHealthRepo.ServiceAvailabilities[key] = false;
            }

            var serviceAccesses = ServiceUrlRepo.Urls ?? new List<ServiceAccess>();
            HashSet<string> activeServiceNames = new HashSet<string>();
            foreach (var access in serviceAccesses)
            {
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Make the HTTP GET request to a Kubernetes API
                        string url = $"http://{access.Url}/api/v1/services";
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access.BearerToken);
                        HttpResponseMessage response = await client.GetAsync(url);

                        // Ensure the request was successful
                        response.EnsureSuccessStatusCode();

                        // Read the response body as a string
                        string content = await response.Content.ReadAsStringAsync();

                        // Extract service name with regex matching on Kubernetes API response
                        var serviceNamePattern = @"(?<=\{""metadata"":\{""name"":""(.*?)"",""namespace"")";
                        var serviceNameRegex = new Regex(serviceNamePattern);
                        var serviceNameMatches = serviceNameRegex.Matches(content);

                        foreach (Match match in serviceNameMatches)
                        {
                            if (match.Success)
                            {
                                var discoveredServiceName = access.Url + "_" +match.Groups[1].Value;
                                if (!activeServiceNames.Contains(discoveredServiceName))
                                {
                                    DiscoveredServiceHealthRepo.ServiceAvailabilities[discoveredServiceName] = true;
                                    Console.WriteLine("Available service named: " + discoveredServiceName);
                                    activeServiceNames.Add(discoveredServiceName);
                                }
                            }
                        }
                    }
                    catch (HttpRequestException e)
                    {
                        Console.WriteLine($"Request error: {e.Message}");
                    }
                }
            }

            foreach (var serviceName in DiscoveredServiceHealthRepo.ServiceAvailabilities.Keys)
            {
                DiscoveredServiceHealthRepo.ServiceAvailabilities[serviceName] = activeServiceNames.Contains(serviceName);
            }
        }
    }
}