using Hl7.Fhir.Rest;
using Microsoft.Extensions.Logging;
using Msn.InteropDemo.AppServices.Core;
using Msn.InteropDemo.Entities.Activity;
using Msn.InteropDemo.Fhir.Model.Response;
using Msn.InteropDemo.Integration.Configuration;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text;

namespace Msn.InteropDemo.Fhir.Implementacion
{
    public class ImmunizationManager : IImmunizationManager
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

        /// <summary>
        /// Registra una vacuna en Nomivac basandose en el ejemplo de: 
        /// https://simplifier.net/saluddigital.ar/immunization-example
        /// </summary>
        /// <param name="request">Model con los datos para el registro</param>
        public async System.Threading.Tasks.Task<RegistrarImmunizationResponse> RegistrarAplicacionVacunaAsync(Model.Request.RegistrarInmunizationRequest request)
        {
            var immu = GenerateImmunization(request);

            var service = _integrationServicesConfiguration.GetConfigurationService(IntegrationServicesConfiguration.ConfigurationServicesName.IMMUNIZATION);

            var client = new FhirClient(service.BaseURL)
            {
                PreferredFormat = ResourceFormat.Json
            };

            var activity = new ActivityLog
            {
                ActivityTypeDescriptorId = (int)Entities.Activity.ActivityType.IMMUNIZATION_POST_VACUNA_NOMIVAC
            };

            client.OnBeforeRequest += (object sender, BeforeRequestEventArgs e) =>
            {
                if (e.Body != null)
                {
                    var requestAdderss = e.RawRequest.Address.ToString();
                    var requestBody = Encoding.UTF8.GetString(e.Body, 0, e.Body.Length);

                    //Prettify !!!
                    requestBody = JToken.Parse(requestBody).ToString();

                    activity.RequestIsJson = true;
                    activity.ActivityRequestUI = requestAdderss;
                    activity.ActivityRequestBody = requestBody;

                    _logger.LogInformation($"Send Request Address:{requestAdderss}");
                    _logger.LogInformation($"Send Request Body:{requestBody}");
                }
            };

            client.OnAfterResponse += (object sender, AfterResponseEventArgs e) =>
            {
                if (e.Body != null)
                {
                    var responseBody = Encoding.UTF8.GetString(e.Body, 0, e.Body.Length);

                    //Prettify !!!
                    responseBody = JToken.Parse(responseBody).ToString();

                    activity.ResponseIsJson = true;
                    activity.ActivityResponse = $"Status: {e.RawResponse.StatusCode}";
                    activity.ActivityResponseBody = responseBody;

                    _logger.LogInformation($"Received response with status: {e.RawResponse.StatusCode}");
                    _logger.LogInformation($"Received response with body: {responseBody}");
                }
            };

            var immuResp = await client.CreateAsync(immu);

            var ret = new RegistrarImmunizationResponse
            {
                Id = immuResp.Id
            };

            _currentContext.RegisterActivityLog(activity);

            return ret;
        }


        private Hl7.Fhir.Model.Immunization GenerateImmunization(Model.Request.RegistrarInmunizationRequest request)
        {
            var patient = GeneratePatient(request);
            var location = GenerateLocation(request);
            var vaccineCode = GenerateVaccineCode(request);

            var immu = new Hl7.Fhir.Model.Immunization();

            //immu.Meta = new Hl7.Fhir.Model.Meta();
            //immu.Meta.Profile = new string[] { "http://fhir.msal.gov.ar/StructureDefinition/NomivacImmunization" };

            immu.Contained = new List<Hl7.Fhir.Model.Resource>
            {
                patient,
                location
            };
            immu.Status = Hl7.Fhir.Model.Immunization.ImmunizationStatusCodes.Completed;
            immu.VaccineCode = vaccineCode;
            immu.Patient = new Hl7.Fhir.Model.ResourceReference
            {
                Reference = "#Patient-01"
            };

            immu.Occurrence = new Hl7.Fhir.Model.FhirDateTime
            {              
                Value = request.AplicacionVacunaFecha.ToString("yyyy-MM-dd")
            };

            immu.PrimarySource = true;
            immu.Location = new Hl7.Fhir.Model.ResourceReference
            {
                Reference = "#Location-01"
            };
            immu.LotNumber = request.VacunaLote;
            if (request.VacunaFechaVencimiento.HasValue)
            {
                immu.ExpirationDate = request.VacunaFechaVencimiento.Value.ToString("yyyy-MM-dd");
            }
            immu.ProtocolApplied.Add(GenerateProtocolApplied(request));

            return immu;
        }

        private Hl7.Fhir.Model.Patient GeneratePatient(Model.Request.RegistrarInmunizationRequest request)
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
                Value = request.LocalPacienteId
            };

            var patientName = new Hl7.Fhir.Model.HumanName
            {
                Use = Hl7.Fhir.Model.HumanName.NameUse.Official,
                Text = $"{request.PrimerNombre} {request.PrimerApellido}",
                Family = $"{request.PrimerApellido}",
                Given = new string[] { request.PrimerNombre, request.OtrosNombres }
            };

            patientName.FamilyElement.Extension.Add(
            new Hl7.Fhir.Model.Extension
            {
                Url = "http://hl7.org/fhir/StructureDefinition/humanname-fathers-family",
                Value = new Hl7.Fhir.Model.FhirString($"{request.PrimerApellido}")
            });

            var patient = new Hl7.Fhir.Model.Patient
            {
                Id = "Patient-01",
                Name = new List<Hl7.Fhir.Model.HumanName> { patientName },
                Identifier = new List<Hl7.Fhir.Model.Identifier> { patientIdentifierLocal, patientIdentifierDNI },
                BirthDate = request.FechaNacimiento.ToString("yyyy-MM-dd"),
                Gender = (request.Sexo == Common.Constants.Sexo.Femenido) ? Hl7.Fhir.Model.AdministrativeGender.Female : Hl7.Fhir.Model.AdministrativeGender.Male
            };

            return patient;
        }

        private Hl7.Fhir.Model.Location GenerateLocation(Model.Request.RegistrarInmunizationRequest request)
        {
            var location = new Hl7.Fhir.Model.Location
            {
                Id = "Location-01",
                Identifier = new List<Hl7.Fhir.Model.Identifier>
                {
                    new Hl7.Fhir.Model.Identifier
                    {
                        System = "http://fhir.msal.gov.ar/refes",
                        Value = request.CurrentLocationId
                    }
                },
                Name = request.CurrentLocationName
            };

            return location;
        }

        private Hl7.Fhir.Model.CodeableConcept GenerateVaccineCode(Model.Request.RegistrarInmunizationRequest request)
        {
            var vaccineCode = new Hl7.Fhir.Model.CodeableConcept
            {
                Coding = new List<Hl7.Fhir.Model.Coding>
                 {
                     new Hl7.Fhir.Model.Coding
                     {
                         System = "http://snomed.info/sct",
                         Code = request.SctConceptId,
                         Display = request.SctTerm
                     }
                 }
            };

            return vaccineCode;
        }

        private Hl7.Fhir.Model.Immunization.ProtocolAppliedComponent GenerateProtocolApplied(Model.Request.RegistrarInmunizationRequest request)
        {
            var protocol = new Hl7.Fhir.Model.Immunization.ProtocolAppliedComponent
            {
                SeriesElement = new Hl7.Fhir.Model.FhirString
                {
                    Value = request.VacunaEsquemaNombre,
                    Extension = new List<Hl7.Fhir.Model.Extension>
                    {
                        new Hl7.Fhir.Model.Extension
                        {
                             Url = "http://fhir.msal.gov.ar/StructureDefinition/NomivacEsquema",
                             Value = new Hl7.Fhir.Model.Coding
                             {
                                 System = "http://fhir.msal.gov.ar/CodeSystem/NOMIVAC-esquema",
                                 Code = request.VacunaEsquemaId,
                                 Display = request.VacunaEsquemaNombre
                             }
                        }
                    }
                },
                DoseNumber = new Hl7.Fhir.Model.PositiveInt
                {
                    Value = 1
                }
            };

            return protocol;
        }
    }
}
