using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Msn.InteropDemo.Integration.Configuration
{
    public class IntegrationServicesConfiguration
    {
        private readonly System.Object lockObj = new System.Object();
        public enum ConfigurationServicesName
        {
            BUS,
            SNOWSTORM,
            IMMUNIZATION
        }

        [JsonProperty("services")]
        public IList<IntegrationService> Services { get; set; }

        public IntegrationService GetConfigurationService(ConfigurationServicesName servicesName)
        {
            lock(lockObj)
            {
                var name = servicesName.ToString();
                var ret = Services.FirstOrDefault(x => x.ServiceName == name);
                return ret;
            }
        }
    }
}
