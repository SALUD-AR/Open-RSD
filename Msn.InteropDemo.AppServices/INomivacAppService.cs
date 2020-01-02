using Msn.InteropDemo.Entities.Evoluciones;
using Msn.InteropDemo.Entities.Nomivac;
using Msn.InteropDemo.ViewModel.Vacunas;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Msn.InteropDemo.AppServices
{
    public interface INomivacAppService
    {
        Task<IEnumerable<NomivacEsquema>> GetEsquemasForVacunaSctIdAsync(decimal sctId);
        EvolucionVacunaAplicacion GetVacunaAplicacion(int evolucionAplicacionId);
        Task<VacunaAplicacionGridItemViewModel> GetVacunaAplicadaDetalleAsync(int evolucionVacunaAplicacionId);
        IEnumerable<VacunaAplicacionGridItemViewModel> GetVacunasAplicacion(int pacienteId);
        Task<int> RegistrarAplicacionNomivacAsync(int evolucionVacunaAplicacionId, 
                                       int nomivacEsquemaId, 
                                       DateTime fechaAplicacion);
    }
}