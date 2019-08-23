using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Msn.InteropDemo.Integration.Configuration
{
    [JsonObject("integrationService")]
    public class IntegrationService
    {
        private readonly System.Object lockObj = new System.Object();

        public enum ConfigurationEndPointName
        {
            PATIENT_GET,
            PATIENT_POST_MATCH,
            PATIENT_POST_CREATE,
            SNOWSTORM_FIND_CONCEPTS,
            SNOWSTORM_REFSET_MEMBERS
        }

        [JsonProperty("baseURL")]
        public string BaseURL { get; set; }

        [JsonProperty("friendlyBaseURL")]
        public string FriendlyBaseURL { get; set; }

        [JsonProperty("serviceName")]
        public string ServiceName { get; set; }
        [JsonProperty("endPoints")]
        public List<IntegrationEndpoint> Endpoints { get; set; }

        public IntegrationEndpoint GetEndPoint(ConfigurationEndPointName endPointName)
        {
            lock (lockObj)
            {
                var ret = Endpoints.FirstOrDefault(x => x.Name == endPointName.ToString());
                return ret;
            }
                
        }
    }
}
