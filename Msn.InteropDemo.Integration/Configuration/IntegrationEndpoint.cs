using Newtonsoft.Json;

namespace Msn.InteropDemo.Integration.Configuration
{
    [JsonObject("integrationEndpoint")]
    public class IntegrationEndpoint
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string URL { get; set; }

        [JsonProperty("friendlyURL")]
        public string FriendlyURL { get; set; }

    }
}
