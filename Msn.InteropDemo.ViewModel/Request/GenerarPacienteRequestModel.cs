using System;
using System.Collections.Generic;
using System.Text;

namespace Msn.InteropDemo.ViewModel.Request
{
    public class GenerarPacienteRequestModel
    {
        public string PrimerNombre { get; set; }

        public string PrimerApellido { get; set; }

        public string Sexo { get; set; }

        public int TipoDocumentoId { get; set; }

        public int NroDocumneto { get; set; }

        public string FechaNacimiento { get; set; }
    }
}
