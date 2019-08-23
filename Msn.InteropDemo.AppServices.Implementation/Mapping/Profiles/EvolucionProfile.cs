
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

            CreateMap<Snowstorm.Model.Components.Item, ViewModel.Snomed.SnomedItem>()
                .ForMember(dest => dest.ConceptId, orig => orig.MapFrom(x => x.ConceptId))
                .ForMember(dest => dest.Description, orig => orig.MapFrom(x => x.pt.Term))
                .ForMember(dest => dest.FSN, orig => orig.MapFrom(x => x.fsn.Term))
                .ForMember(dest => dest.Language, orig => orig.MapFrom(x => x.pt.Lang));

            CreateMap<Snowstorm.Model.Components.RefsetItem, ViewModel.Snomed.SnomedItem>()
                .ForMember(dest => dest.ConceptId, orig => orig.MapFrom(x => x.referencedComponent.conceptId))
                .ForMember(dest => dest.Description, orig => orig.MapFrom(x => x.referencedComponent.pt.Term))
                .ForMember(dest => dest.FSN, orig => orig.MapFrom(x => x.referencedComponent.fsn.Term))
                .ForMember(dest => dest.Language, orig => orig.MapFrom(x => x.referencedComponent.pt.Lang));

            CreateMap<Entities.Evoluciones.EvolucionDiagnostico, ViewModel.Evoluciones.EvolucionDiagnosticoViewModel>();
            CreateMap<Entities.Evoluciones.EvolucionMedicamento, ViewModel.Evoluciones.EvolucionMedicamentoViewModel>();
            CreateMap<Entities.Evoluciones.EvolucionVacunaAplicacion, ViewModel.Evoluciones.EvolucionVacunaAplicacionViewModel>();
            ///////////////////////////////////////////////////////////////////////////////////////////////////////

            //VIEWMODEL TO ENTITIES
            ///////////////////////////////////////////////////////////////////////////////////////////////////////
            CreateMap<ViewModel.Evoluciones.EvolucionViewModel, Entities.Evoluciones.Evolucion>();
            CreateMap<ViewModel.Evoluciones.EvolucionDiagnosticoViewModel, Entities.Evoluciones.EvolucionDiagnostico>();
            CreateMap<ViewModel.Evoluciones.EvolucionMedicamentoViewModel, Entities.Evoluciones.EvolucionMedicamento>();
            CreateMap<ViewModel.Evoluciones.EvolucionVacunaAplicacionViewModel, Entities.Evoluciones.EvolucionVacunaAplicacion>();
            ///////////////////////////////////////////////////////////////////////////////////////////////////////
        }
    }
}
