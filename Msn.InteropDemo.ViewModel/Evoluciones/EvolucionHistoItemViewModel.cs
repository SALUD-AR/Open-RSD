using System;
using System.Collections.Generic;
using System.Text;

namespace Msn.InteropDemo.ViewModel.Evoluciones
{
    public class EvolucionHistoItemViewModel
    {
        public int Id { get; set; }

        public string FechaEvolucion { get; set; }

        public int PacienteId { get; set; }
        public string ProfesionalApellido { get; set; }
    }
}
