using System;
using System.Collections.Generic;
using System.Text;

namespace Msn.InteropDemo.Fhir.Model.Request
{
    public class RegistrarInmunizationRequest
    {
        public string LocalPacienteId { get; set; }

        public int DNI { get; set; }

        public string PrimerNombre { get; set; }

        public string OtrosNombres { get; set; }

        public string PrimerApellido { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public Common.Constants.Sexo Sexo { get; set; }

        public string SctConceptId { get; set; }

        public string SctTerm { get; set; }

        public DateTime AplicacionVacunaFecha { get; set; }

        public string VacunaLote { get; set; }

        public DateTime? VacunaFechaVencimiento { get; set; }

        public string VacunaEsquemaNombre { get; set; }

        public string VacunaEsquemaId { get; set; }

        public string CurrentLocationId { get; set; }

        public string CurrentLocationName { get; set; }
    }
}
