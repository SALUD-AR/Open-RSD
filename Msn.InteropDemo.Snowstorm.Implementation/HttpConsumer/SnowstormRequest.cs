using Msn.InteropDemo.Snowstorm.Model.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Msn.InteropDemo.Snowstorm.Implementation.HttpConsumer
{
    public class SnowstormRequest : RequestBuilder
    {
        private readonly IEnumerable<Expressions.ExpresionBuilders.HeaderParameter> headerParameters;

        public SnowstormRequest(IEnumerable<Expressions.ExpresionBuilders.HeaderParameter> headerParameters)
        {
            this.headerParameters = headerParameters;
        }
        public TModelResponse Get<TModelResponse>(string url)
        {
            var resp = base.GetAsync<HttpConsumer.Response>(url).Result;
            if (!resp.IsSuccessStatusCode)
            {
                throw new Exception($"Error consultando Snowstorm: StatusCode={resp.StatusCode}; Message:{resp.Message}");
            }
            
            var ret = JsonConvert.DeserializeObject<TModelResponse>(resp.Body); 
            return ret;
        }

        protected override void AddDefaultHeader(HttpClient client)
        {
            foreach (var item in headerParameters)
            {
                client.DefaultRequestHeaders.Add(item.Key, item.Value);
            }
        }
    }
}
