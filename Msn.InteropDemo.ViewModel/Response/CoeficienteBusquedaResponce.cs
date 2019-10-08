
namespace Msn.InteropDemo.ViewModel.Response
{
    public class CoeficienteBusquedaResponce
    {
        public CoeficienteScoreElementResponse ApellidoScoreElement { get; set; }
        public CoeficienteScoreElementResponse NombreScoreElement { get; set; }
        public CoeficienteScoreElementResponse TipoDocumentoScoreElement { get; set; }
        public CoeficienteScoreElementResponse NroDocumentoScoreElement { get; set; }
        public CoeficienteScoreElementResponse SexoScoreElement { get; set; }
        public CoeficienteScoreElementResponse FechaNacimientoScoreElement { get; set; }
        public decimal CoeficienteTotalFinal { get; set; }
        public string CoeficietneTotalFinalUI { get; set; }
    }
}
