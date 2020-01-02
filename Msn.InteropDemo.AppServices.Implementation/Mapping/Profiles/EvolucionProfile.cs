
namespace Msn.InteropDemo.AppServices.Implementation.Mapping.Profiles
{
    public class EvolucionProfile : AutoMapper.Profile
    {
        public EvolucionProfile()
        {
            //ENTITIES TO VIEWMODEL
            ///////////////////////////////////////////////////////////////////////////////////////////////////////
            CreateMap<Entities.Evoluciones.Evolucion, ViewModel.Evoluciones.EvolucionViewModel>()
                .ForMember(dest => dest.ProfesionalApellidoNombre, orig => orig.MapFrom(x => $"Dr. {x.CreatedUser.Apellido}, {x.CreatedUser.Nombre}"))
                .ForMember(dest => dest.FechaConsultaUI, orig => orig.MapFrom(x => Common.Utils.Helpers.DateTimeHelper.FriendlyDateAR(x.CreatedDateTime)));

            CreateMap<Entities.Pacientes.Paciente, ViewModel.Evoluciones.EvolucionHistoViewModel>()
                .ForMember(dest => dest.PacienteId, orig => orig.MapFrom(x => x.Id))
                .ForMember(dest => dest.PacientePrimerApellido, orig => orig.MapFrom(x => x.PrimerApellido))
                .ForMember(dest => dest.PacientePrimerNombre, orig => orig.MapFrom(x => x.PrimerNombre))
                .ForMember(dest => dest.PacienteGeneroNombre, orig => orig.MapFrom(x => x.Sexo == "M" ? "Masculino" : "Femenino"))
                .ForMember(dest => dest.PacienteTipoDocumentoNombre, orig => orig.MapFrom(x => x.TipoDocumento.Nombre))
                .ForMember(dest => dest.PacienteNroDocumento, orig => orig.MapFrom(x => x.NroDocumento))
                .ForMember(dest => dest.PacienteFechaNacimiento, orig => orig.MapFrom(x => x.FechaNacimiento.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.FechaConsultaUI , orig => orig.MapFrom(x => Common.Utils.Helpers.DateTimeHelper.FriendlyDateAR(x.CreatedDateTime)))
                .ForMember(dest => dest.PacienteEdad, orig => orig.MapFrom(x => Common.Utils.Helpers.DateTimeHelper.CalculateAge(x.FechaNacimiento)));

            CreateMap<Entities.Evoluciones.Evolucion, ViewModel.Evoluciones.EvolucionHistoItemViewModel>()
                .ForMember(dest => dest.ProfesionalApellido, orig => orig.MapFrom(x => $"Dr. {x.CreatedUser.Apellido}")) 
                .ForMember(dest => dest.FechaEvolucion, orig => orig.MapFrom(x => x.CreatedDateTime.ToString("dd/MM/yyyy")));

            CreateMap<Entities.Evoluciones.EvolucionDiagnostico, ViewModel.Evoluciones.EvolucionDiagnosticoViewModel>();
            CreateMap<Entities.Evoluciones.EvolucionDiagnosticoCie10, ViewModel.Evoluciones.EvolucionDiagnosticoCie10ViewModel>();
            CreateMap<Entities.Evoluciones.EvolucionMedicamento, ViewModel.Evoluciones.EvolucionMedicamentoViewModel>();
            CreateMap<Entities.Evoluciones.EvolucionVacunaAplicacion, ViewModel.Evoluciones.EvolucionVacunaAplicacionViewModel>();

            CreateMap<Entities.Evoluciones.EvolucionVacunaAplicacion, ViewModel.Vacunas.VacunaAplicacionGridItemViewModel>()
                .ForMember(dest => dest.FechaConsultaUI, orig => orig.MapFrom(x => x.Evolucion.CreatedDateTime.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.SctConceptId, orig => orig.MapFrom(x => x.SctConceptId.HasValue ? x.SctConceptId.ToString() : ""))
                .ForMember(dest => dest.NomivacEsquemaId, orig => orig.MapFrom(x => x.AplicacionNomivacEsquemaId))
                .ForMember(dest => dest.FechaAplicacionUI, orig => orig.MapFrom(x => x.FechaAplicacion.HasValue ? x.FechaAplicacion.Value.ToString("dd/MM/yyyy") : ""))
                .ForMember(dest => dest.EstaAplicada, orig => orig.MapFrom(x => x.NomivanImmunizationId.HasValue));

            ///////////////////////////////////////////////////////////////////////////////////////////////////////

            //VIEWMODEL TO ENTITIES
            ///////////////////////////////////////////////////////////////////////////////////////////////////////
            CreateMap<ViewModel.Evoluciones.EvolucionViewModel, Entities.Evoluciones.Evolucion>();
            CreateMap<ViewModel.Evoluciones.EvolucionDiagnosticoViewModel, Entities.Evoluciones.EvolucionDiagnostico>();
            CreateMap<ViewModel.Evoluciones.EvolucionDiagnosticoCie10ViewModel, Entities.Evoluciones.EvolucionDiagnosticoCie10>();
            CreateMap<ViewModel.Evoluciones.EvolucionMedicamentoViewModel, Entities.Evoluciones.EvolucionMedicamento>();
            CreateMap<ViewModel.Evoluciones.EvolucionVacunaAplicacionViewModel, Entities.Evoluciones.EvolucionVacunaAplicacion>();
            ///////////////////////////////////////////////////////////////////////////////////////////////////////
        }
    }
}
