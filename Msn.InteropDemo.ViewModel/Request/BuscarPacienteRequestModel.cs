using Microsoft.AspNetCore.Mvc.Rendering;
using Msn.InteropDemo.ViewModel.Attributes.Validators;
using System.ComponentModel.DataAnnotations;

namespace Msn.InteropDemo.ViewModel.Request
{
    public class BuscarPacienteRequestModel
    {
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El {0} es requerido")]
        public string PrimerNombre { get; set; }

        [Display(Name = "Apellido")]
        [Required(ErrorMessage = "El {0} es requerido")]
        public string PrimerApellido { get; set; }

        public SelectList TipoDocumentoList { get; set; }

        [Display(Name = "Tipo Doc.")]
        [Required(ErrorMessage = "El {0} es requerido")]
        public int? TipoDocumentoId { get; set; }

        [Display(Name = "Nro. Documento")]
        [Required(ErrorMessage = "El {0} es requerido")]
        [Range(10000, 99000000, ErrorMessage = "El Nro. de documento debe estar en el rango de [10.000, 99.000.000]")]
        public int? NroDocumento { get; set; }

        public SelectList SexoList { get; set; }

        [Display(Name = "Sexo")]
        [Required(ErrorMessage = "El {0} es requerido")]
        public string Sexo { get; set; }

        [Display(Name = "Fecha de nacimiento")]
        [Required(ErrorMessage = "La {0} es requedida")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [FechaValidator(ErrorMessage = "{0} incorrecta")]
        public string FechaNacimiento { get; set; }

    }
}
