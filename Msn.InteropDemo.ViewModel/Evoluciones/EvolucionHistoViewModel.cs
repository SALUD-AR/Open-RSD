using System;
using System.Collections.Generic;
using System.Text;

namespace Msn.InteropDemo.ViewModel.Evoluciones
{
    public class EvolucionHistoViewModel
    {
        public EvolucionHistoViewModel()
        {
            Items = new List<EvolucionHistoItemViewModel>();
        }

        public int? PredefEvolucionId { get; set; }
        public int PacienteId { get; set; }
        public string PacientePrimerNombre { get; set; }
        public string PacientePrimerApellido { get; set; }
        public string PacienteGeneroNombre { get; set; }
        public string PacienteTipoDocumentoNombre { get; set; }
        public string PacienteNroDocumento { get; set; }
        public string PacienteFechaNacimiento { get; set; }
        public List<EvolucionHistoItemViewModel> Items { get; set; }
        public int PacienteEdad { get; set; }
        public string FechaConsultaUI { get; set; }

        public string Sexo { get; set; }


    }
}
