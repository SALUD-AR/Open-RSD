using Msn.InteropDemo.Snowstorm.Model.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Msn.InteropDemo.Snowstorm.Model.Response
{
    public class RefsetQueryResponse
    {
        public List<RefsetItem> Items { get; set; }
        public int Total { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public string SearchAfter { get; set; }
    }
}
