using System;

namespace Msn.InteropDemo.AppServices.Implementation.Mapping.Profiles
{
    public class PacienteProfile : AutoMapper.Profile
    {
        public PacienteProfile()
        {
            //TODO: Borrar cuando sea movido el Helper a AppServices
            CreateMap<Common.Utils.Helpers.ScoreElement, ViewModel.Response.CoeficienteScoreElementResponse>();

            CreateMap<Entities.Pacientes.Paciente, ViewModel.Pacientes.PacienteListItemViewModel>()
                .ForMember(dest => dest.TipoDocumentoId, orig => orig.MapFrom(x => x.TipoDocumentoId))
                .ForMember(dest => dest.TipoDocumentoNombre, orig => orig.MapFrom(x => x.TipoDocumento.Nombre))
                .ForMember(dest => dest.FechaNacimiento, orig => orig.MapFrom(x => x.FechaNacimiento.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.FechaNacimientoPlane, orig => orig.MapFrom(x => x.FechaNacimiento.ToString("yyyyMMdd")))
                .ForMember(dest => dest.ApellidosNombresCompletos, orig => orig.MapFrom(x => $"{x.PrimerApellido} {x.OtrosApellidos}, {x.PrimerNombre} {x.OtrosNombres}" ))
                .ForMember(dest => dest.TipoDocumentoNombreNroDocumento, orig => orig.MapFrom(x => $"{x.TipoDocumento.Nombre} {x.NroDocumento}"))
                .ForMember(dest => dest.SexoNombre, orig => orig.MapFrom(x => x.Sexo == "M" ? "Masculino" : "Femenino"));

            CreateMap<Entities.Pacientes.Paciente, ViewModel.Pacientes.PacienteViewModel>()
                .ForMember(dest => dest.TipoDocumentoNombre, orig => orig.MapFrom(x => x.TipoDocumento.Nombre))
                .ForMember(dest => dest.FechaNacimiento, orig => orig.MapFrom(x => x.FechaNacimiento.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.SexoNombre, orig => orig.MapFrom(x => x.Sexo == "M" ? "Masculino" : "Femenino"));

            CreateMap<ViewModel.Pacientes.PacienteViewModel, Entities.Pacientes.Paciente>()
                .ForMember(dest => dest.FechaNacimiento, orig => orig.MapFrom(x => Common.Utils.Helpers.DateTimeHelper.FromDateTimeAR(x.FechaNacimiento)??DateTime.Now));
        }
    }
}
