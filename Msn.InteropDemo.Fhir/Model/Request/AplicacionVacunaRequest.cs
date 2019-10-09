using System;
using System.Collections.Generic;
using System.Text;

namespace Msn.InteropDemo.Fhir.Model.Request
{
    public class RegistrarInmunizationRequest
    {
        public string LocalId { get; set; }

        public int DNI { get; set; }

        public string PrimerNombre { get; set; }

        public string OtrosNombres { get; set; }

        public string PrimerApellido { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public Common.Constants.Sexo Sexo { get; set; }
    }
}
