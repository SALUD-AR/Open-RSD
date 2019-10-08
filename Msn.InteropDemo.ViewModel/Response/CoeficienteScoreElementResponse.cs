using System;
using System.Collections.Generic;
using System.Text;

namespace Msn.InteropDemo.ViewModel.Response
{
    public class CoeficienteScoreElementResponse
    {
        public decimal PesoValor { get; set; }
        public string PesoValorUI { get; set; }
        public string LevenshteinDistante { get; set; }
        public string Ingresado { get; set; }
        public string Obtenido { get; set; }
        public string ObtenidoLen { get; set; }
        public string CoeficienteParcialUI { get; set; }
        public decimal CoeficienteFinal { get; set; }
        public string CoeficienteFinalUI { get; set; }
    }
}
