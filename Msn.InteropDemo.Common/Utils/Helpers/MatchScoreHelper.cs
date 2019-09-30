namespace Msn.InteropDemo.Common.Utils.Helpers
{
    public sealed class MatchScoreHelper
    {
        public enum MatchField
        {
            PrimerApellido,
            PrimerNombre,
            TipoDocumento,
            NroDocumento,
            Sexo,
            FechaNacimiento
        }

        public static readonly MatchScoreHelper MatchApellido          = new MatchScoreHelper { MatchValue = 0.2M, MatchType = MatchField.PrimerApellido };
        public static readonly MatchScoreHelper MatchNombre            = new MatchScoreHelper { MatchValue = 0.1M, MatchType = MatchField.PrimerNombre };
        public static readonly MatchScoreHelper MatchTipoDocumento     = new MatchScoreHelper { MatchValue = 0.1M, MatchType = MatchField.TipoDocumento };
        public static readonly MatchScoreHelper MatchNroDocumento      = new MatchScoreHelper { MatchValue = 0.3M, MatchType = MatchField.NroDocumento };
        public static readonly MatchScoreHelper MatchSexo              = new MatchScoreHelper { MatchValue = 0.1M, MatchType = MatchField.Sexo };
        public static readonly MatchScoreHelper MatchFechaNacimiento   = new MatchScoreHelper { MatchValue = 0.2M, MatchType = MatchField.FechaNacimiento };

        public decimal MatchValue { get; private set; }

        public MatchField MatchType { get; private set; }

        public decimal CalculateScore(string obtainedText, string searchedText)
        {
            if(string.IsNullOrWhiteSpace(obtainedText))
            {
                return 0;
            }
            if (string.IsNullOrWhiteSpace(searchedText))
            {
                return 0;
            }

            decimal ret = 0;
            var strLen = obtainedText.Length;
            var distance = StringHelper.LevenshteinDistance(obtainedText, searchedText);

            //Si la distancia es igual al Len del string obtenido es porque no coincide absolutamente nada.
            //En este caso retorna 0. No ha coincidencia alguna. 
            if(distance >= strLen)
            {
                return 0;
            }

            //Si los textos son iguales, retonra el valor del Coeficiente completo.
            if (distance == 0)
            {
                return this.MatchValue;
            }

            //Si la distancia es menor entonces es necesario el calculo porcentual del coeficiente.
            if (distance < strLen)
            {
                var matchCoef = ((decimal)(strLen - distance) / (decimal)strLen);
                ret = (matchCoef * this.MatchValue);
            }

            return ret;
        }
    }
}
