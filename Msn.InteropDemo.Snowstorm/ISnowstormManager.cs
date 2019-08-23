using Msn.InteropDemo.Snowstorm.Model.Response;
using System.Collections.Generic;

namespace Msn.InteropDemo.Snowstorm
{
    public interface ISnowstormManager
    {
        QueryResponse RunQuery(Expressions.ExpresionBuilders.Base.BaseExpBuilder builder);
        QueryResponse RunQuery(string expression, string term);

        RefsetQueryResponse RunQueryRefset(string conceptId, string term);
    }
}
