using System;
using System.Collections.Generic;
using System.Text;

namespace Msn.InteropDemo.Snowstorm.Expressions.ExpresionBuilders
{
    public class HeaderParameter
    {
        public HeaderParameter(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; }
        public string Value { get; }
    }
}
