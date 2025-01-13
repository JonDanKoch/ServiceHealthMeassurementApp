namespace ServiceHealthMeassurementApp.Models
{
    public class ServiceAccess
    {
        public ServiceAccess(string url, string bearerToken)
        {
            Url = url;
            BearerToken = bearerToken;
        }

        public string Url { get; set; } 
        public string BearerToken { get; set; }
    }
}