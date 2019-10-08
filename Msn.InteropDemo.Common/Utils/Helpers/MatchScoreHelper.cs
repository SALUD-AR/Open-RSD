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

        public ScoreElement GenerateScoreElement(string obtainedText, string searchedText)
        {
            if (string.IsNullOrWhiteSpace(obtainedText))
            {
                throw new System.ArgumentException("message", nameof(obtainedText));
            }

            if (string.IsNullOrWhiteSpace(searchedText))
            {
                throw new System.ArgumentException("message", nameof(searchedText));
            }

            obtainedText = obtainedText.ToLower();
            searchedText = searchedText.ToLower();

            var strLen = obtainedText.Length;
            var distance = StringHelper.LevenshteinDistance(obtainedText, searchedText);

            var element = new ScoreElement
            {             
                Ingresado = searchedText,
                Obtenido = obtainedText,
                PesoValor = this.MatchValue,
                PesoValorUI = this.MatchValue.ToString("P2"),
                ObtenidoLen = strLen.ToString(),
                LevenshteinDistante = distance
            };

            //Si la distancia es igual al Len del string obtenido es porque no coincide absolutamente nada.
            //En este caso retorna 0. No ha coincidencia alguna. 
            if (distance >= strLen)
            {
                element.CoeficienteParcialUI = (0M).ToString("#0.00");
                element.CoeficienteFinal = 0;
                element.CoeficienteFinalUI = (0M).ToString("P2");
                return element;
            }

            //Si los textos son iguales, retonra el valor del Coeficiente completo.
            if (distance == 0)
            {
                element.CoeficienteParcialUI = (1M).ToString("#0.00");
                element.CoeficienteFinal = this.MatchValue;
                element.CoeficienteFinalUI = this.MatchValue.ToString("P2");
                return element;
            }

            //Si la distancia es menor entonces es necesario el calculo porcentual del coeficiente.
            if (distance < strLen)
            {
                var matchCoef = ((decimal)(strLen - distance) / (decimal)strLen);
                var finalCoef = (matchCoef * this.MatchValue);

                element.CoeficienteParcialUI = matchCoef.ToString("#0.00");
                element.CoeficienteFinal = finalCoef;
                element.CoeficienteFinalUI = finalCoef.ToString("P2");
            }

            return element;
        }

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
