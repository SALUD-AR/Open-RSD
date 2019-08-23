using System.Collections.Generic;

namespace Msn.InteropDemo.ViewModel.Evoluciones
{
    public class EvolucionViewModel : Core.EntityAuditableModel
    {
        public int PacienteId { get; set; }

        public Pacientes.PacienteViewModel Paciente { get; set; }

        public string Observacion { get; set; }

        public ICollection<EvolucionDiagnosticoViewModel> Diagnosticos { get; set; }

        public ICollection<EvolucionMedicamentoViewModel> Medicamentos { get; set; }

        public ICollection<EvolucionVacunaAplicacionViewModel> Vacunas { get; set; }

        public string FechaConsultaUI { get; set; }

        public string ProfesionalApellidoNombre { get; set; }
    }
}
