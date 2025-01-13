using ServiceHealthMeassurementApp.Models;

namespace ServiceHealthMeassurementApp.Utils
{
    /// <summary>
    /// Stores urls with access tokens for accessing the different kubernetes APIs.
    /// </summary>
    public static class ServiceUrlRepo
    {
        /// <summary>
        /// Urls with access tokens that can be used to connect to the kubernetes API.
        /// </summary>
        public static List<ServiceAccess> Urls { get; set; } = new List<ServiceAccess>();
    }
}
