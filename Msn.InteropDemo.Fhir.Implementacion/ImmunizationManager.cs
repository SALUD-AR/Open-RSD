using Microsoft.Extensions.Logging;
using Msn.InteropDemo.AppServices.Core;
using Msn.InteropDemo.Integration.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Msn.InteropDemo.Fhir.Implementacion
{
    public class ImmunizationManager
    {
        private readonly ILogger<ImmunizationManager> _logger;
        private readonly IntegrationServicesConfiguration _integrationServicesConfiguration;
        private readonly ICurrentContext _currentContext;

        public ImmunizationManager(ILogger<ImmunizationManager> logger,
                                   IntegrationServicesConfiguration integrationServicesConfiguration,
                                   ICurrentContext currentContext)
        {
            _logger = logger;
            _integrationServicesConfiguration = integrationServicesConfiguration;
            _currentContext = currentContext;
        }

        public void RegistrarAplicacionVacuna(Msn.InteropDemo.Fhir.Model.Request.RegistrarInmunizationRequest request)
        {
            var patientIdentifierDNI = new Hl7.Fhir.Model.Identifier
            {
                Use = Hl7.Fhir.Model.Identifier.IdentifierUse.Usual,
                System = Common.Constants.DomainName.RenaperDniDomain.Value,
                Value = request.DNI.ToString()
            };

            var patientIdentifierLocal = new Hl7.Fhir.Model.Identifier
            {
                Use = Hl7.Fhir.Model.Identifier.IdentifierUse.Usual,
                System = Common.Constants.DomainName.LocalDomain.Value,
                Value = request.LocalId
            };

            var patientName = new Hl7.Fhir.Model.HumanName
            {
                Use = Hl7.Fhir.Model.HumanName.NameUse.Official,
                Text = $"{request.PrimerNombre} {request.PrimerApellido}",
                Family = $"{request.PrimerApellido}",
                Given = new string[] { request.PrimerNombre, request.OtrosNombres },
            };

            var patient = new Hl7.Fhir.Model.Patient
            {
                Id = request.LocalId, 
                Name = new List<Hl7.Fhir.Model.HumanName> { patientName },
                Identifier = new List<Hl7.Fhir.Model.Identifier> { patientIdentifierLocal, patientIdentifierDNI },
                BirthDate = request.FechaNacimiento.ToString("yyyy-MM-dd"),
                Gender = (request.Sexo == Common.Constants.Sexo.Femenido) ? Hl7.Fhir.Model.AdministrativeGender.Female : Hl7.Fhir.Model.AdministrativeGender.Male
            };

            var immu = new Hl7.Fhir.Model.Immunization();

            immu.Contained = new List<Hl7.Fhir.Model.Resource>
            {
                patient
            };

            immu.Status = Hl7.Fhir.Model.Immunization.ImmunizationStatusCodes.Completed;

            immu.VaccineCode = new Hl7.Fhir.Model.CodeableConcept
            {
                 Coding = new List<Hl7.Fhir.Model.Coding>
                 {
                     new Hl7.Fhir.Model.Coding
                     {
                         System = Common.Constants.DomainName.FederadorPatientDomain.Value,
                         Code = "XXXX" //codigo de vacuna de SISA ??????
                     }
                 }
            };

            

        }

    }
}
