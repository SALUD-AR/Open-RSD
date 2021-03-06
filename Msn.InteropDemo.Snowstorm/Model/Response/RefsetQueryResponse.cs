﻿using System.Collections.Generic;

namespace Msn.InteropDemo.Snowstorm.Model.Response
{
    public class RefsetQueryCie10MapResponse
    {
        public List<Components.RefsetCie10MapItem> Items { get; set; }
        public int Total { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public string SearchAfter { get; set; }
    }
}
