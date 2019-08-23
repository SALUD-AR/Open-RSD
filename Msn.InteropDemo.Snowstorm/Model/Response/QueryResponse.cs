using Msn.InteropDemo.Snowstorm.Model.Components;
using System.Collections.Generic;

namespace Msn.InteropDemo.Snowstorm.Model.Response
{
    public class QueryResponse
    {
        public List<Item> Items { get; set; }
        public int Total { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public string SearchAfter { get; set; }
        public List<long> SearchAfterArray { get; set; }
    }
}
