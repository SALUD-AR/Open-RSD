using Microsoft.Extensions.Logging;
using Msn.InteropDemo.AppServices.Core;
using Msn.InteropDemo.Snowstorm;
using Msn.InteropDemo.ViewModel.Snomed;
using System.Collections.Generic;
using System.Linq;

namespace Msn.InteropDemo.AppServices.Implementation.AppServices
{
    public class Cie10AppService : Core.GenericServiceReadOnly<Entities.Codificacion.Cie10>, ICie10AppService
    {
        private readonly ISnowstormManager _snowstormManager;

        public Cie10AppService(ICurrentContext currentContext,
                               ILogger<Cie10AppService> logger, 
                               Snowstorm.ISnowstormManager snowstormManager) : 
                                    base(currentContext, logger)
        {
            _snowstormManager = snowstormManager;
        }

        public IEnumerable<Cie10MapResultViewModel> GetCie10MappedItems(string conceptId)
        {
            var lst = new List<Cie10MapResultViewModel>();

            var cie10lst = _snowstormManager.RunQueryCie10MapRefset(conceptId);

            //se seleccionan solo aquellos que tienen un MapTarget, es decir, un Mapeo Esistente.
            cie10lst.Items = cie10lst.Items.Where(x => !string.IsNullOrWhiteSpace(x.additionalFields.mapTarget)).ToList();

            //Customization para el formato de la codificacion de la DEIS
            foreach (var item in cie10lst.Items)
            {
                if(item.additionalFields.mapTarget.Length == 3)
                {
                    item.additionalFields.mapTarget += "X";
                }
            }

            //Mapeo  a nuestros ViewModels
            var cie10Maplst = Mapper.Map<List<Cie10MapResultViewModel>>(cie10lst.Items);

            //Se seleccionadn los distintos MApTargue para is a buscar en la DB
            var cie102search = cie10Maplst.Select(x => x.MapTarget).Distinct();

            //obtenidos de la DB
            var cie10FromDbItems =  CurrentContext.DataContext.Cie10.Where(x => cie102search.Contains(x.SubcategoriaId));

            //por cada uno encontrado se le setea el texto de la SubcategoriaNombre y CategoriaNombre de la CIE10 esn Español
            foreach (var item in cie10Maplst)
            {
                var dbitem = cie10FromDbItems.FirstOrDefault(x => x.SubcategoriaId == item.MapTarget);
                if(dbitem != null)
                {
                    item.SubcategoriaNombre = dbitem.SubcategoriaNombre;
                    item.CategoriaNombre = dbitem.CategoriaNombre;
                }
            } 

            return cie10Maplst;
        }
    }
}
