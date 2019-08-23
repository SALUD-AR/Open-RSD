using Msn.InteropDemo.Snowstorm.Expressions.Operators.Core;

namespace Msn.InteropDemo.Snowstorm.Expressions.Attributes.Base
{
    public class EclAttribute : Core.IExpression
    {
        private readonly EclOperator _operatorType;

        /// <summary>
        /// Configura un Atributo para filtrado de búsqueda.
        /// </summary>
        /// <param name="attrSctId">ID de concepto del atributo por el cual filtrar</param>
        /// <param name="attrValueSctId">ID de concepto del valor del atributo por el cual filtrar</param>
        /// <param name="attrOperator">Término del valor por el cual filtrar</param>
        public EclAttribute(string attrSctId,
                            string attrValueSctId,
                            EclOperatorFactory.EclOperatorType attrOperator = Operators.Core.EclOperatorFactory.EclOperatorType.EQUAL) 
                            : this(attrSctId, null, attrValueSctId, null, attrOperator)
        {

        }

        /// <summary>
        /// Configura un Atributo para filtrado de búsqueda.
        /// </summary>
        /// <param name="attrSctId">ID de concepto del atributo por el cual filtrar</param>
        /// <param name="attrSctTerm">Término del concepto a buscar</param>
        /// <param name="attrValueSctId">ID de concepto del valor del atributo por el cual filtrar</param>
        /// <param name="attrValueSctTerm">Término del valor por el cual filtrar</param>
        /// <param name="attrOperator"></param>
        public EclAttribute(string attrSctId,
                            string attrSctTerm,
                            string attrValueSctId,
                            string attrValueSctTerm,
                            EclOperatorFactory.EclOperatorType attrOperator = EclOperatorFactory.EclOperatorType.EQUAL)
        {
            AttrSctId = attrSctId;
            AttrSctTerm = attrSctTerm;
            AttrValueSctId = attrValueSctId;
            AttrValueSctTerm = attrValueSctTerm;
            _operatorType = Operators.Core.EclOperatorFactory.CreateAttOperator(attrOperator);
        }

        public string AttrSctId { get; }
        public string AttrSctTerm { get; }
        public string AttrValueSctId { get; }
        public string AttrValueSctTerm { get; }

        public string GetExpression()
        {
            var attrTerm = AttrSctTerm != null ? $"|{AttrSctTerm}|" : string.Empty;
            var attrValueTerm = AttrValueSctTerm != null ? $"|{AttrValueSctTerm}|" : string.Empty;
            return $"{AttrSctId}{attrTerm}{_operatorType.GetOperator}{AttrValueSctId}{attrValueTerm}";
        }

        public override string ToString() => GetExpression();
    }
}
