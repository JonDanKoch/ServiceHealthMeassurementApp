namespace ServiceHealthMeassurementApp.Utils
{
    public class ServiceUrlRepo
    {
        public static List<string> Urls { get; set; }
        
        private static ServiceUrlRepo _instance;

        // Lock object for thread synchronization
        private static readonly object _lock = new object();

        public static ServiceUrlRepo Instance
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
                            _instance = new ServiceUrlRepo();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}
