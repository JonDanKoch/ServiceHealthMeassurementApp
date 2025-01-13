namespace ServiceHealthMeassurementApp.Utils
{
    /// <summary>
    /// Stores infos about the discovered Kubernetes services and config data for service discovery.
    /// </summary>
    public static class DiscoveredServiceHealthRepo
    {
        /// <summary>
        /// Stores info about the availabilities of the service urls represented by the keys of the dict.
        /// </summary>
        public static Dictionary<string,bool> ServiceAvailabilities = new Dictionary<string,bool>();
        
        /// <summary>
        /// Intervall in which the service discovery happens.
        /// </summary>
        public static int ServiceDiscoveryIntervallSeconds = 60;
    }
}
