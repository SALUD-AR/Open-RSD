using Microsoft.AspNetCore.Mvc.Rendering;
using Msn.InteropDemo.ViewModel.Attributes.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace Msn.InteropDemo.ViewModel.Pacientes
{
    public class PacienteViewModel : Core.EntityAuditableModel
    {
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El {0} es requerido")]
        [StringLength(50, ErrorMessage = "El {0} debe contener entre {1} y {2} caracteres", MinimumLength = 2)]
        public string PrimerNombre { get; set; }

        [Display(Name = "Otros Nombres")]
        [StringLength(50, ErrorMessage = "{0} debe contener entre {1} y {2} caracteres", MinimumLength = 2)]
        public string OtrosNombres { get; set; }

        [Display(Name = "Apellido")]
        [Required(ErrorMessage = "El {0} es requerido")]
        [StringLength(50, ErrorMessage = "El {0} debe contener entre {1} y {2} caracteres", MinimumLength = 2)]
        public string PrimerApellido { get; set; }

        [Display(Name = "Otros Apellidos")]
        [StringLength(50, ErrorMessage = "{0} debe contener entre {1} y {2} caracteres", MinimumLength = 2)]
        public string OtrosApellidos { get; set; }

        [Display(Name = "Tipo Documento")]
        [Required(ErrorMessage = "El {0} es requerido")]
        public int? TipoDocumentoId { get; set; }

        public string TipoDocumentoNombre { get; set; }

        public SelectList TipoDocumentoList { get; set; }

        [Display(Name = "Nro. Documento")]
        [Required(ErrorMessage = "El {0} es requerido")]
        public int? NroDocumento { get; set; }

        [Display(Name = "Sexo")]
        [Required(ErrorMessage = "El {0} es requerido")]
        public string Sexo { get; set; }

        public SelectList SexoList { get; set; }

        public string SexoNombre { get; set; }

        [Display(Name = "Fecha de nacimiento")]
        [Required(ErrorMessage = "La {0} es requedida")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [FechaValidator(ErrorMessage = "{0} incorrecta")]
        public string FechaNacimiento { get; set; }

        [Display(Name = "Calle")]
        public string DomicilioCalle { get; set; }

        [Display(Name = "Altura")]
        public string DomicilioCalleAltura { get; set; }

        [Display(Name = "Piso")]
        public string DomicilioPiso { get; set; }

        [Display(Name = "Departamento")]
        public string DomicilioDepto { get; set; }

        [Display(Name = "Cod. Postal")]
        public string DomicilioCodPostal { get; set; }

        public int? FederadorId { get; set; }

        public DateTime? FederadoDateTime { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

    }
}
