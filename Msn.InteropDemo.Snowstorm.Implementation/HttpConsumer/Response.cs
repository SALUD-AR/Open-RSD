using System;
using System.Net;

namespace Msn.InteropDemo.Snowstorm.Implementation.HttpConsumer
{
    public class Response
    {
        public string Message { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public bool IsSuccessStatusCode { get; set; }

        public string Detail { get; set; }

        public string Body { get; set; }

        public string ResponseDetail { get; set; }

        public override string ToString()
        {
            return
                $"IsSuccessStatusCode:{IsSuccessStatusCode}; " +
                Environment.NewLine + Environment.NewLine +
                $"StatusCode:{StatusCode} " +
                Environment.NewLine + Environment.NewLine +
                $"Message:{Message}; " +
                Environment.NewLine + Environment.NewLine +
                $"Detail:{Detail}; " +
                Environment.NewLine + Environment.NewLine +
                $"Body:{Body}; " +
                Environment.NewLine + Environment.NewLine +
                $"ResponseDetail:{ResponseDetail}";
        }
    }
}
