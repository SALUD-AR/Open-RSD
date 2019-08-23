using Msn.InteropDemo.Snowstorm.Expressions.ExpresionBuilders.Base;
using Msn.InteropDemo.Snowstorm.Expressions.Operators.Core;

namespace Msn.InteropDemo.Snowstorm.Expressions.ExpresionBuilders
{
    public class SearchDescendantExpBuilder : BaseExpBuilder
    {
        /// <param name="sctConceptId">ID del Concepto sobre el cual se obtendran los descendientes</param>
        public SearchDescendantExpBuilder(string sctConceptId) :
            base(EclOperatorFactory.EclOperatorType.DESCENDANTOF,
                 sctConceptId,
                 null,
                 null)
        { }

        /// <param name="sctConceptId">ID del Concepto sobre el cual se obtendran los descendientes</param>
        /// <param name="sctConceptTerm">ID del Concepto sobre el cual se obtendran los descendientes</param>
        public SearchDescendantExpBuilder(string sctConceptId,
                                     string sctConceptTerm) :
            base(EclOperatorFactory.EclOperatorType.DESCENDANTOF,
                 sctConceptId,
                 sctConceptTerm,
                 null)
        { }

        /// <param name="sctConceptId">ID del Concepto sobre el cual se obtendran los descendientes</param>
        /// <param name="sctConceptTerm">ID del Concepto sobre el cual se obtendran los descendientes</param>
        /// <param name="searchTerm">Termino a buscar combinado en la expresión</param>
        public SearchDescendantExpBuilder(string sctConceptId,
                                     string sctConceptTerm = null,
                                     string searchTerm = null) :
            base(EclOperatorFactory.EclOperatorType.DESCENDANTOF,
                 sctConceptId,
                 sctConceptTerm,
                 searchTerm)
        { }


    }
}
