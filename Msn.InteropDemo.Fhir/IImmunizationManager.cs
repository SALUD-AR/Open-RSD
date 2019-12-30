using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Msn.InteropDemo.Fhir
{
    public interface IImmunizationManager
    {
        Task<Model.Response.RegistrarImmunizationResponse> RegistrarAplicacionVacunaAsync(Model.Request.RegistrarInmunizationRequest request);
    }
}
