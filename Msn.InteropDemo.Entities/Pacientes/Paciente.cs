using Msn.InteropDemo.Entities.Core;
using System;

namespace Msn.InteropDemo.Entities.Pacientes
{
    public class Paciente : EntityAuditable
    {
        public string PrimerNombre { get; set; }

        public string OtrosNombres { get; set; }

        public string PrimerApellido { get; set; }

        public string OtrosApellidos { get; set; }

        public int TipoDocumentoId { get; set; }

        public TipoDocumento TipoDocumento { get; set; }

        public int NroDocumento { get; set; }

        public string Sexo { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public string DomicilioCalle { get; set; }

        public string DomicilioCalleAltura { get; set; }

        public string DomicilioPiso { get; set; }

        public string DomicilioDepto { get; set; }

        public string DomicilioCodPostal { get; set; }

        public int? FederadorId { get; set; }

        public DateTime? FederadoDateTime { get; set; }

        public override string ToString()
        {
            return $"PrimerApellido:{PrimerApellido}, PrimerNombre:{PrimerNombre}, Tipo Doc.:{TipoDocumentoId}, Nro. Documento:{NroDocumento}, Sexo:{Sexo}";
        }
    }
}
