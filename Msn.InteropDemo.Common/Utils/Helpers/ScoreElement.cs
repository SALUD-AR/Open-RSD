
namespace Msn.InteropDemo.Common.Utils.Helpers
{

    //TODO: Eliminar al mover el Helper a AppService
    public class ScoreElement
    {
        public string PesoValorUI { get; set; }
        public int LevenshteinDistante { get; set; }
        public string Ingresado { get; set; }
        public string Obtenido { get; set; }
        public string ObtenidoLen { get; set; }
        public string CoeficienteParcialUI { get; set; }
        public decimal CoeficienteFinal { get; set; }
        public string CoeficienteFinalUI { get; set; }
    }
}
