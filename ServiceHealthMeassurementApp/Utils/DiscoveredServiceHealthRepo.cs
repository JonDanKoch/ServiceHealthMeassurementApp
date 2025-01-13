namespace ServiceHealthMeassurementApp.Utils
{
    public class DiscoveredServiceHealthRepo
    {
        public static Dictionary<string,bool> serviceAvailabilities = new Dictionary<string,bool>();
        public static int serviceDiscoveryIntervallSeconds = 60;
        
        private static DiscoveredServiceHealthRepo? _instance;

        // Lock object for thread synchronization
        private static readonly object _lock = new object();

        public static DiscoveredServiceHealthRepo Instance
        {
            get
            {
                // Double-checked locking to ensure thread-safety
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DiscoveredServiceHealthRepo();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}
