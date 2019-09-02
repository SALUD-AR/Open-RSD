using Msn.InteropDemo.ViewModel.Snomed;

namespace Msn.InteropDemo.AppServices.Implementation.Mapping.Profiles
{
    public class SnowstormProfile : AutoMapper.Profile
    {
        public SnowstormProfile()
        {
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

            CreateMap<Snowstorm.Model.Components.RefsetCie10MapItem, Cie10MapResultViewModel>()
                .ForMember(dest => dest.ConceptId, orig => orig.MapFrom(x => x.referencedComponent.conceptId))
                .ForMember(dest => dest.Description, orig => orig.MapFrom(x => x.referencedComponent.pt.Term))
                .ForMember(dest => dest.FSN, orig => orig.MapFrom(x => x.referencedComponent.fsn.Term))
                .ForMember(dest => dest.Language, orig => orig.MapFrom(x => x.referencedComponent.pt.Lang))
                .ForMember(dest => dest.MapGroup, orig => orig.MapFrom(x => x.additionalFields.mapGroup))
                .ForMember(dest => dest.MapPriority, orig => orig.MapFrom(x => x.additionalFields.mapPriority))
                .ForMember(dest => dest.MapRule, orig => orig.MapFrom(x => x.additionalFields.mapRule))
                .ForMember(dest => dest.MapAdvice, orig => orig.MapFrom(x => x.additionalFields.mapAdvice))
                .ForMember(dest => dest.MapTarget, orig => orig.MapFrom(x => x.additionalFields.mapTarget.Replace(".", "")))
                .ForMember(dest => dest.SubcategoriaId, orig => orig.MapFrom(x => x.additionalFields.mapTarget.Replace(".", "")));
        }
    }
}
