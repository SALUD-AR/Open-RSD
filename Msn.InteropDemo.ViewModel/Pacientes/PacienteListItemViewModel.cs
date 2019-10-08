using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace Msn.InteropDemo.ViewModel.Pacientes
{
    public class PacienteListItemViewModel
    {
        public int Id { get; set; }

        public string ApellidosNombresCompletos { get; set; }

        public string TipoDocumentoNombreNroDocumento { get; set; }

        public string PrimerApellido { get; set; }

        public string PrimerNombre { get; set; }

        public string OtrosNombres { get; set; }

        public string OtrosApellidos { get; set; }

        public string Sexo { get; set; }

        public string SexoNombre { get; set; }

        public int TipoDocumentoId { get; set; }

        public string TipoDocumentoNombre { get; set; }

        public int? NroDocumento { get; set; }

        public string FechaNacimiento { get; set; }

        public string FechaNacimientoPlane { get; set; }

        public decimal Score { get; set; }

        public string FrienlyScore
        {
            get
            {
                return Score.ToString("P", CultureInfo.InvariantCulture);
            }
        }

        public bool PrimerNombreEsCoincidente { get; set; }
        public bool PrimerApellidoEsCoincidente { get; set; }
        public bool TipoDocumentoEsCoincidente { get; set; }
        public bool NroDocumentoEsCoincidente { get; set; }
        public bool SexoEsCoincidente { get; set; }
        public bool FechaNacimientoEsCoincidente { get; set; }

        public string PrimerApellidoCss {
            get {
                return this.PrimerApellidoEsCoincidente ? "text-success" : "";
            }
        }

        public string PrimerNombreCss
        {
            get
            {
                return this.PrimerNombreEsCoincidente ? "text-success" : "";
            }
        }

        public string TipoDocumentoCss
        {
            get
            {
                return this.TipoDocumentoEsCoincidente ? "text-success" : "";
            }
        }

        public string NroDocumentoCss
        {
            get
            {
                return this.NroDocumentoEsCoincidente ? "text-success" : "";
            }
        }

        public string SexoCss
        {
            get
            {
                return this.SexoEsCoincidente ? "text-success" : "";
            }
        }

        public string FechaNacimientoCss
        {
            get
            {
                return this.FechaNacimientoEsCoincidente ? "text-success" : "";
            }
        }



    }
}
