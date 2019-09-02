using Hl7.Fhir.Rest;
using Microsoft.Extensions.Logging;
using Msn.InteropDemo.AppServices.Core;
using Msn.InteropDemo.Entities.Activity;
using Msn.InteropDemo.Fhir.Model.Response;
using Msn.InteropDemo.Integration.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Msn.InteropDemo.Fhir.Implementacion
{
    public class PatientManager : IPatientManager
    {
        private readonly ILogger<PatientManager> _logger;
        private readonly IntegrationServicesConfiguration _integrationServicesConfiguration;
        private readonly ICurrentContext _currentContext;

        public PatientManager(ILogger<PatientManager> logger,
                              IntegrationServicesConfiguration integrationServicesConfiguration, 
                              AppServices.Core.ICurrentContext currentContext)
        {
            _logger = logger;
            _integrationServicesConfiguration = integrationServicesConfiguration;
            _currentContext = currentContext;
        }

        public FederarPacienteResponse FederarPaciente(Model.Request.FederarPacienteRequest request)
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

            patientName.FamilyElement.Extension.Add(
            new Hl7.Fhir.Model.Extension
            {
                Url = "http://hl7.org/fhir/StructureDefinition/humanname-fathers-family",
                Value = new Hl7.Fhir.Model.FhirString($"{request.PrimerApellido}")
            });

            var patient = new Hl7.Fhir.Model.Patient
            {
                Id = null, //INPORTANTE
                IdElement = null, //INPORTANTE
                Name = new List<Hl7.Fhir.Model.HumanName> { patientName },
                Identifier = new List<Hl7.Fhir.Model.Identifier> { patientIdentifierLocal, patientIdentifierDNI },
                BirthDate = request.FechaNacimiento.ToString("yyyy-MM-dd"),
                Gender = (request.Sexo == Common.Constants.Sexo.Femenido) ? Hl7.Fhir.Model.AdministrativeGender.Female : Hl7.Fhir.Model.AdministrativeGender.Male
            };

            var serviceUrl = _integrationServicesConfiguration.GetConfigurationService(IntegrationServicesConfiguration.ConfigurationServicesName.BUS);
            //var patientCreateUrl = serviceUrl.GetEndPoint(IntegrationService.ConfigurationEndPointName.PATIENT_POST_CREATE);

            var client = new FhirClient(serviceUrl.BaseURL)
            {
                PreferredFormat = ResourceFormat.Json
            };

            var activity = new ActivityLog
            {
                ActivityTypeDescriptorId = (int)Entities.Activity.ActivityType.FEDERAR_PACIENTE_EN_BUS
            };

            client.OnBeforeRequest +=  (object sender, BeforeRequestEventArgs e) =>
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
            
            var patientRet = client.Create(patient);

            var identifierBus = patient.Identifier.Where(x => x.System == Msn.InteropDemo.Common.Constants.DomainName.FederadorPatientDomain.Value).FirstOrDefault();

            var resp = new Model.Response.FederarPacienteResponse();

            if (identifierBus != null)
            {
                resp.Id = int.Parse(identifierBus.Value);
            }

            _currentContext.RegisterActivityLog(activity);

            return resp;
        }

        public bool ExistsPatient(Model.Request.ExistPatientRequest request)
        {
            var bundle = GetPatientsByMatch(request.Dni,
                                          request.PrimerNombre,
                                          request.PrimerApellido,
                                          request.OtrosNombres,
                                          request.Sexo,
                                          request.FechaNacimiento);

            return bundle.Total > 0;
        }

        public bool ExistsPatient(Common.Constants.DomainName system, string value)
        {
            var bundle = GetPatientByIdentifier(system.Value, value);
            return bundle.Total.HasValue && (bundle.Total > 0);
        }

        public Hl7.Fhir.Model.Bundle GetPatientsByMatch(int dni,
                                                        string primerApellido,
                                                        string primerNombre,
                                                        string otrosNombres,
                                                        Common.Constants.Sexo sexo,
                                                        DateTime fechaNacimiento)
        {

            if (dni < 10000)
            {
                throw new Exception($"Nro. de documento no válido:{dni} debe ser mayor a 10.000");
            }

            if (string.IsNullOrWhiteSpace(primerApellido))
            {
                throw new ArgumentException("message", nameof(primerApellido));
            }

            if (string.IsNullOrWhiteSpace(primerNombre))
            {
                throw new ArgumentException("message", nameof(primerNombre));
            }

            var patientIdentifier = new Hl7.Fhir.Model.Identifier();
            patientIdentifier.Use = Hl7.Fhir.Model.Identifier.IdentifierUse.Usual;
            patientIdentifier.System = Common.Constants.DomainName.RenaperDniDomain.Value;
            patientIdentifier.Value = dni.ToString();

            var patientName = new Hl7.Fhir.Model.HumanName();
            patientName.Use = Hl7.Fhir.Model.HumanName.NameUse.Official;
            patientName.Text = $"{primerNombre} {primerApellido}";
            patientName.Family = $"{primerApellido}";
            patientName.Given = new string[] { primerNombre, otrosNombres };
            patientName.FamilyElement.Extension.Add(
            new Hl7.Fhir.Model.Extension
            {
                Url = "http://hl7.org/fhir/StructureDefinition/humanname-fathers-family",
                Value = new Hl7.Fhir.Model.FhirString($"{primerApellido}")
            });

            var paramPatient = new Hl7.Fhir.Model.Patient();
            paramPatient.Name = new List<Hl7.Fhir.Model.HumanName> { patientName };
            paramPatient.Identifier = new List<Hl7.Fhir.Model.Identifier> { patientIdentifier };
            paramPatient.BirthDate = fechaNacimiento.ToString("yyyy-MM-dd");
            paramPatient.Gender = (sexo == Common.Constants.Sexo.Femenido) ? Hl7.Fhir.Model.AdministrativeGender.Female : Hl7.Fhir.Model.AdministrativeGender.Male;

            var parameters = new Hl7.Fhir.Model.Parameters();
            parameters.Id = "mymatch";

            parameters.Add("resource", paramPatient);
            parameters.Add("count", new Hl7.Fhir.Model.Integer(5));

            var serviceUrl = _integrationServicesConfiguration.GetConfigurationService(IntegrationServicesConfiguration.ConfigurationServicesName.BUS);
            var patientUrl = serviceUrl.GetEndPoint(IntegrationService.ConfigurationEndPointName.PATIENT_POST_MATCH);

            var activity = new ActivityLog
            {
              ActivityTypeDescriptorId = (int)Entities.Activity.ActivityType.GET_PACIENTE_EN_BUS_MATCH
            };

            var client = new FhirClient(serviceUrl.BaseURL)
            {
                PreferredFormat = ResourceFormat.Json
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

                    activity.ResponseIsJson= true;
                    activity.ActivityResponse = $"Status: {e.RawResponse.StatusCode}";
                    activity.ActivityResponseBody = responseBody;

                    _logger.LogInformation($"Received response with status: {e.RawResponse.StatusCode}");
                    _logger.LogInformation($"Received response with body: {responseBody}");
                }
            };

            var resp = client.Operation(new Uri(patientUrl.URL), parameters, false);

            var ret = (Hl7.Fhir.Model.Bundle)resp;

            _currentContext.RegisterActivityLog(activity);

            return ret;
        }

        public Hl7.Fhir.Model.Bundle GetPatientByIdentifier(Common.Constants.DomainName system, string value)
        {
            return GetPatientByIdentifier(system.Value, value);
        }

        public Hl7.Fhir.Model.Bundle GetPatientByIdentifier(string system, string value)
        {
            if (string.IsNullOrWhiteSpace(system))
            {
                throw new ArgumentException("message", nameof(system));
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("message", nameof(value));
            }

            var activity = new ActivityLog
            {
                ActivityTypeDescriptorId = (int)Entities.Activity.ActivityType.GET_PACIENTE_EN_BUS_BY_IDENTIFIER
            };


            //var baseUrl = "https://testapp.hospitalitaliano.org.ar/masterfile-federacion-service/fhir";
            var baseUrl = _integrationServicesConfiguration.Services
                                    .First(x => x.ServiceName == "BUS").BaseURL;

            var client = new FhirClient(baseUrl);
            client.PreferredFormat = ResourceFormat.Json;
            client.OnBeforeRequest += (object sender, BeforeRequestEventArgs e) =>
            {
                
                    var requestAdderss = e.RawRequest.Address.ToString();
                
                    activity.RequestIsURL = true;
                    activity.ActivityRequestUI = requestAdderss;

                    _logger.LogInformation($"Send Request Address:{requestAdderss}");
                
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


            Hl7.Fhir.Model.Bundle ret = null;

            try
            {
                var qp = new SearchParams();
                qp.Add("identifier", $"{system}|{value}");
                ret = client.Search<Hl7.Fhir.Model.Patient>(qp);
                _currentContext.RegisterActivityLog(activity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw ex;
            }

            return ret;
        }

        public Hl7.Fhir.Model.Patient GetPatientByBusId(string patientId)
        {
            var srvConfig = _integrationServicesConfiguration.GetConfigurationService(IntegrationServicesConfiguration.ConfigurationServicesName.BUS);
            var baseUrl = srvConfig.BaseURL;
            var getPatientUrl = srvConfig.GetEndPoint(IntegrationService.ConfigurationEndPointName.PATIENT_GET).URL + $"/{patientId}";

            var client = new Hl7.Fhir.Rest.FhirClient(baseUrl);
            client.OnBeforeRequest += OnBeforeRequestFhirServer;
            client.OnAfterResponse += OnAfterResponseFhirServer;

            var ret = client.Read<Hl7.Fhir.Model.Patient>(getPatientUrl);

            return ret;
        }

        private void OnAfterResponseFhirServer(object sender, AfterResponseEventArgs e)
        {
            if (e.Body != null)
            {
                var responseBody = Encoding.UTF8.GetString(e.Body, 0, e.Body.Length);

                //Prettify !!!
                responseBody = JToken.Parse(responseBody).ToString();

                _logger.LogInformation($"Received response with s {e.RawResponse.StatusCode}");
                _logger.LogInformation($"Received response with body: {responseBody }");
            }
        }

        private void OnBeforeRequestFhirServer(object sender, BeforeRequestEventArgs e)
        {
            if (e.Body != null)
            {
                var requestAdderss = e.RawRequest.Address.ToString();
                var requestBody = Encoding.UTF8.GetString(e.Body, 0, e.Body.Length);

                //Prettify !!!
                requestBody = JToken.Parse(requestBody).ToString();

                _logger.LogInformation($"Send Request Address:{requestAdderss}");
                _logger.LogInformation($"Send Request Body:{requestBody}");
            }
        }
    }
}
