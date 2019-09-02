
namespace Msn.InteropDemo.ViewModel.Evoluciones
{
    public class EvolucionDiagnosticoViewModel
    {
        public int Id { get; set; }

        public int PacienteId { get; set; }

        public int? EvolucionId { get; set; }

        public decimal? SctConceptId { get; set; }

        public string SctDescriptionTerm { get; set; }

        public string Cie10SubcategoriaId { get; set; }

        public string Cie10SubcategoriaNombre { get; set; }
    }
}
