using System;

namespace Msn.InteropDemo.Fhir
{
    public interface IPatientManager
    {
        Model.Response.FederarPacienteResponse FederarPaciente(Model.Request.FederarPacienteRequest request);
        Hl7.Fhir.Model.Patient GetPatientByBusId(string patientId);
        Hl7.Fhir.Model.Bundle GetPatientByIdentifier(Common.Constants.DomainName system, string value);
        Hl7.Fhir.Model.Bundle GetPatientByIdentifier(string system, string value);
        Hl7.Fhir.Model.Bundle GetPatientsByMatch(int dni,
                                                        string primerApellido,
                                                        string primerNombre,
                                                        string otrosNombres,
                                                        Common.Constants.Sexo sexo,
                                                        DateTime fechaNacimiento);
        bool ExistsPatient(Model.Request.ExistPatientRequest request);
        bool ExistsPatient(Common.Constants.DomainName system, string value);
    }
}
