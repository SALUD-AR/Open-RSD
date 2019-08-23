using Msn.InteropDemo.Snowstorm.Expressions.ExpresionBuilders;
using System.Collections.Generic;

namespace Msn.InteropDemo.Snowstorm.Expressions.Constants
{
    public static class HeaderDefault
    {
        public static IEnumerable<HeaderParameter> Headers = new List<HeaderParameter>
        {
            { new HeaderParameter("Accept", "application/json")},
            { new HeaderParameter("Accept-Language", "es")}
        };  
    }
}
