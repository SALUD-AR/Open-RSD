
namespace Msn.InteropDemo.ViewModel.Vacunas
{
    public class VacunaAplicacionGridItemViewModel
    {
        public int Id { get; set; }
        public string SctConceptId { get; set; }
        public string SctDescriptionTerm { get; set; }
        public string FechaConsultaUI { get; set; }
        public string FechaAplicacionUI { get; set; }
        public int PacienteId { get; set; }
        public bool EstaAplicada { get; set; }
    }
}
