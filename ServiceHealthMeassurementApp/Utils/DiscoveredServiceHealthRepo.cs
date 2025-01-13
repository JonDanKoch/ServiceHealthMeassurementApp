namespace ServiceHealthMeassurementApp.Utils
{
    public static class DiscoveredServiceHealthRepo
    {
        /// <summary>
        /// Stores info about the availabilities of the service urls represented by the keys of the dict.
        /// </summary>
        public static Dictionary<string,bool> serviceAvailabilities = new Dictionary<string,bool>();
        
        /// <summary>
        /// Intervall in which the service discovery happens
        /// </summary>
        public static int serviceDiscoveryIntervallSeconds = 60;
    }
}
