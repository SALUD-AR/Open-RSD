using Msn.InteropDemo.Entities.Evoluciones;
using System;
using System.Collections.Generic;
using System.Text;

namespace Msn.InteropDemo.AppServices
{
    public interface IEvolucionAppService : Core.IGenericService<Entities.Evoluciones.Evolucion>
    {
        IEnumerable<ViewModel.Evoluciones.EvolucionDiagnosticoViewModel> GetDiagnosticos(int evolucionId);
        ViewModel.Evoluciones.EvolucionHistoViewModel GetEvolucionesHisto(int pacienteId);
        IEnumerable<ViewModel.Snomed.SnomedItem> SearchSnowstormHallazgos(string term);
        IEnumerable<ViewModel.Snomed.SnomedItem> SearchSnowstormByExpressionTerm(string expression, string term);
        ViewModel.Evoluciones.EvolucionViewModel GetById(int evolucionId);
        List<ViewModel.Evoluciones.EvolucionHistoItemViewModel> GetEvolucionHistoDates(int pacienteId);
    }
}
