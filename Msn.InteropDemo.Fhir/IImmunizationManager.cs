using System;
using System.Collections.Generic;
using System.Text;

namespace Msn.InteropDemo.Fhir
{
    public interface IImmunizationManager
    {
        Model.Response.RegistrarImmunizationResponse RegistrarAplicacionVacuna(Model.Request.RegistrarInmunizationRequest request);
    }
}
