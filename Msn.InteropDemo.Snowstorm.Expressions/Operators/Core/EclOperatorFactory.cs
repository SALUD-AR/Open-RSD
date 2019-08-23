namespace Msn.InteropDemo.Snowstorm.Expressions.Operators.Core
{
    public static class EclOperatorFactory
    {
        public enum EclOperatorType
        {
            EQUAL = 0,
            DESCENDANTOF,
            ANCESTOROF,
            ANY
        }

        public static EclOperator CreateAttOperator(EclOperatorType attType = EclOperatorType.EQUAL)
        {
            if (attType == EclOperatorType.ANCESTOROF)
            {
                return new OperatorAncestorOf();
            }
            if (attType == EclOperatorType.DESCENDANTOF)
            {
                return new OperatorDescendantOf();
            }
            if (attType == EclOperatorType.ANY)
            {
                return new OperatorAny();
            }

            return new OperatorEquals();
        }
    }
}
