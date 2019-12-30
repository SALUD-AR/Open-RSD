using Msn.InteropDemo.Entities.Nomivac;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Msn.InteropDemo.AppServices
{
    public interface INomivacAppService
    {
        Task<IEnumerable<NomivacEsquema>> GetEsquemasForVacunaSctIdAsync(decimal sctId);

        Task<int> RegistrarAplicacionNomivacAsync(int evolucionVacunaAplicacionId, 
                                       int nomivacEsquemaId, 
                                       DateTime fechaAplicacion);
    }
}