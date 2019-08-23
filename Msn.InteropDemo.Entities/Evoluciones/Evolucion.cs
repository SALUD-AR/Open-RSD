using System.Collections.Generic;

namespace Msn.InteropDemo.Entities.Evoluciones
{
    public class Evolucion : Core.EntityAuditable
    {
        public int PacienteId { get; set; }

        public Pacientes.Paciente Paciente { get; set; }

        public string Observacion { get; set; }

        public ICollection<EvolucionDiagnostico> Diagnosticos { get; set; }

        public ICollection<EvolucionMedicamento> Medicamentos { get; set; }

        public ICollection<EvolucionVacunaAplicacion> Vacunas { get; set; }
    }

    
}
