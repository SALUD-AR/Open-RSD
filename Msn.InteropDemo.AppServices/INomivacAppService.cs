using Msn.InteropDemo.Entities.Nomivac;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Msn.InteropDemo.AppServices
{
    public interface INomivacAppService
    {
        Task<IEnumerable<NomivacEsquema>> GetEsquemasForVacunaSctIdAsync(decimal sctId);
        int RegistrarAplicacionNomivac(int pacienteId, int evolucionVacunaAplicacionId, decimal vacunaSctId, int nomivacEsquemaId, DateTime fechaAplicacion);
    }
}