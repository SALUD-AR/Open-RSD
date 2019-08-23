using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Msn.InteropDemo.Snowstorm.Implementation.HttpConsumer
{
    public abstract class RequestBuilder
    {
        public virtual async Task<TResp> GetAsync<TResp>(string url, IDictionary<string, string> headerParameters = null, string token = null) where TResp : Response, new()
        {
            using (var client = new HttpClient())
            {
                var resp = new TResp();

                try
                {
                    AddDefaultHeader(client);

                    if (headerParameters != null)
                    {
                        foreach (var item in headerParameters)
                        {
                            client.DefaultRequestHeaders.Add(item.Key, item.Value);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    }

                    var response = await client.GetAsync(url);
                    var strResponse = await response.Content.ReadAsStringAsync();

                    resp.Body = strResponse;
                    resp.IsSuccessStatusCode = response.IsSuccessStatusCode;
                    resp.StatusCode = response.StatusCode;
                    resp.Message = response.ToString();
                }
                catch (Exception ex)
                {
                    resp.IsSuccessStatusCode = false;
                    resp.Message = $"Error en llamada a URL:{url}" + Environment.NewLine + ex.ToString();
                }

                return resp;
            }
        }

        public virtual async Task<TResp> PostAsync<TResp>(string url, string jsonBody = null, IDictionary<string, string> headerParameters = null, string token = null) where TResp : Response, new()
        {
            using (var client = new HttpClient())
            {
                HttpContent httpContent;
                HttpResponseMessage response;

                AddDefaultHeader(client);

                if (headerParameters != null)
                {
                    foreach (var item in headerParameters)
                    {
                        client.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }

                if (!string.IsNullOrWhiteSpace(token))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                if (!string.IsNullOrWhiteSpace(jsonBody))
                {
                    httpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                    response = await client.PostAsync(url, httpContent);
                }
                else
                {
                    httpContent = new StringContent(string.Empty, Encoding.UTF8, "application/json");
                    response = await client.PostAsync(url, httpContent);
                }


                var strResponse = await response.Content.ReadAsStringAsync();

                var resp = new TResp
                {
                    Body = strResponse,
                    IsSuccessStatusCode = response.IsSuccessStatusCode,
                    StatusCode = response.StatusCode,
                    Message = response.ToString(),
                };

                return resp;
            }
        }

        public virtual async Task<TResp> PostAsync<TResp>(string url, IDictionary<string, string> formParameters, IDictionary<string, string> headerParameters = null, string token = null) where TResp : Response, new()
        {
            if (formParameters == null || formParameters.Count == 0)
            {
                throw new ArgumentException("formParameters no puede ser vacío");
            }

            using (var client = new HttpClient())
            {
                AddDefaultHeader(client);

                if (headerParameters != null)
                {
                    foreach (var item in headerParameters)
                    {
                        client.DefaultRequestHeaders.Add(item.Value, item.Key);
                    }
                }

                var postData = formParameters.ToList();
                var content = new FormUrlEncodedContent(postData);


                if (!string.IsNullOrWhiteSpace(token))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                var response = await client.PostAsync(url, content);
                var strResponse = await response.Content.ReadAsStringAsync();

                var resp = new TResp
                {
                    Body = strResponse,
                    IsSuccessStatusCode = response.IsSuccessStatusCode,
                    StatusCode = response.StatusCode,
                    Message = response.ToString(),
                };

                return resp;
            }
        }

        protected abstract void AddDefaultHeader(HttpClient client);
    }
}
