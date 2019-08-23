using System;
using System.Collections.Generic;
using System.Text;

namespace Msn.InteropDemo.Snowstorm.Expressions.ExpresionBuilders
{
    public class HallazgoExpBuilder : SearchDescendantExpBuilder
    {
        public HallazgoExpBuilder() : base(Constants.RootConcept.Hallazgo.SctId,
                                             Constants.RootConcept.Hallazgo.SctTerm)
        {
        }

        public HallazgoExpBuilder(string searchTerm) : base(Constants.RootConcept.Hallazgo.SctId,
                                                              Constants.RootConcept.Hallazgo.SctTerm,
                                                              searchTerm)
        {
        }
    }
}
