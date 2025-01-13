namespace ServiceHealthMeassurementApp.Models
{
    /// <summary>
    /// Class to store service url and access token for the corresponding Kubernetes service.
    /// </summary>
    public class ServiceAccess
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="url">url.</param>
        /// <param name="bearerToken">token.</param>
        public ServiceAccess(string url, string bearerToken)
        {
            Url = url;
            BearerToken = bearerToken;
        }

        /// <summary>
        /// Url of the service.
        /// </summary>
        public string Url { get; set; } 

        /// <summary>
        /// Token to access the service.
        /// </summary>
        public string BearerToken { get; set; }
    }
}