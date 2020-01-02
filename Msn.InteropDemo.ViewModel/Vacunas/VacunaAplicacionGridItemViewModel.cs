
using System;

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
        public int? NomivacEsquemaId { get; set; }
        public string NomivacEsquemaNombre { get; set; }
        public int? NomivanImmunizationId { get; set; }
        public bool EstaAplicada { get; set; }
    }
}
