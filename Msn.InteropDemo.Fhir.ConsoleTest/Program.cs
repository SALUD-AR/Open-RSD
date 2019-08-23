using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Msn.InteropDemo.Fhir.ConsoleTest.Mock;
using Msn.InteropDemo.Integration.Configuration;
using Serilog;
using System;
using System.Collections.Generic;

namespace Msn.InteropDemo.Fhir.ConsoleTest
{
    class Program
    {
        private static IServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            //CreatePatient();
            //ConfigureLogger();
            //var bunble = GetPatient();
            //ShowPatients();
            //ShowPatient("1982708");
            //Log.CloseAndFlush();
            try
            {
                RegisteServices();
                Log.Information("Servicios registrados");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar servicios:\n{ex.ToString()}");
                Console.ReadKey();
            }

            try
            {

                //var service = _serviceProvider.GetService<IPatientManager>();
                //service.GetPatientsByMatch(17287412,
                //                           "amorese",
                //                           "miguel",
                //                           "angel",
                //                           Common.Constants.Sexo.Masculino,
                //                           new DateTime(1964, 6, 30));

                //TestExpression();

                TestExpressionAppService();

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Test ERROR");
            }

            Log.CloseAndFlush();
            DisposeServices();
        }

       private static void RegisteServices()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(configuration)
                            .CreateLogger();

            IServiceCollection services = new ServiceCollection();

            services.AddDbContext<Data.Context.DataContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            services.AddLogging(config => config.AddSerilog());

            services.Configure<IntegrationServicesConfiguration>(configuration.GetSection("IntegrationServices"));
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<IntegrationServicesConfiguration>>().Value);
            services.AddTransient(typeof(Data.Context.DataContext));
            services.AddTransient<AppServices.Core.ICurrentContext, CurrentContext>();
            services.AddTransient<IPatientManager, Implementacion.PatientManager>();
            services.AddTransient<Snowstorm.ISnowstormManager, Snowstorm.Implementation.SnowstormManager>();
            services.AddTransient<AppServices.IEvolucionAppService, AppServices.Implementation.AppServices.EvolucionAppService>();


            _serviceProvider = services.BuildServiceProvider();
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }

        static void TestExpression()
        {

            var service = _serviceProvider.GetService<Snowstorm.ISnowstormManager>();

            var builder = new Snowstorm.Expressions.ExpresionBuilders.HallazgoExpBuilder("embarazo");
            builder.Attributes.Add(new Snowstorm.Expressions.Attributes.Base.EclAttribute("116676008", "morfologia asociada (atributo)", "23583003", "inflamacion (anomalia morfologica)"));
            builder.Attributes.Add(new Snowstorm.Expressions.Attributes.Base.EclAttribute("246075003", "agente causal (atributo)", "49872002", "virus (organismo)"));

            Log.Information($"Expression:\n{builder.GetExpression()}");
            Log.Information($"QueryString:\n{builder.GetQueryString()}");

            var resp = service.RunQuery(builder);

        }

        static void TestExpressionAppService()
        {
            var service = _serviceProvider.GetService<AppServices.IEvolucionAppService>();
            var resp = service.SearchSnowstormHallazgos("embarazo");
        }


        static void ShowPatients()
        {
            try
            {
                var resources = GetPatient().Entry;
                Console.WriteLine($"Cantidad de pacientes: {resources.Count}\n");

                
                foreach (var item in resources)
                {
                    Console.WriteLine($"Tipo recurso:{item.Resource.ResourceType} Id:{item.Resource.Id}");
                    Console.WriteLine($"FullUrl:{item.FullUrl}");

                    Console.WriteLine("Identifiers:");
                    var patient = (Hl7.Fhir.Model.Patient)item.Resource;
                    foreach (var patientId in patient.Identifier)
                    {
                        Console.WriteLine($"System:{patientId.System}, Value:{patientId.Value}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.ReadKey();
        }

        private static void ShowPatient(string id)
        {
            try
            {
                //var rsc = GetPatient();
                var rsc = GetPatient2(id);

                Console.WriteLine($"Paciente ID:{rsc.Id}");
                Console.WriteLine($"Meta VersionId:{rsc.Meta.VersionId} LastUpdated:{rsc.Meta.LastUpdated}");
                Console.WriteLine("Mostrando nombres");
                foreach (var name in rsc.Name)
                {
                    Console.WriteLine($"TypeName:{name.TypeName} Use:{(name.Use != null ? name.Use.ToString() : "")} Family:{name.Family}");
                }
                Console.WriteLine("---------------------------------");

                Console.WriteLine("Mostrando identificaciones");
                foreach (var ident in rsc.Identifier)
                {
                    Console.WriteLine($"System:{ident.System}");
                    Console.WriteLine($"Value:{(ident.Value != null ? ident.Value : "")}");
                }

                Console.WriteLine("---------------------------------");


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.ReadKey();
        }

        static Hl7.Fhir.Model.Bundle GetPatient()
        {
            var baseUrl = "https://testapp.hospitalitaliano.org.ar/masterfile-federacion-service/fhir";

            var client = new Hl7.Fhir.Rest.FhirClient(baseUrl);
            client.OnBeforeRequest += OnBeforeRequestFhirServer;
            client.OnAfterResponse += OnAfterResponseFhirServer;

            Hl7.Fhir.Model.Bundle ret = null;

            try
            {
                var qp = new Hl7.Fhir.Rest.SearchParams();
                qp.Add("identifier", "http://www.msal.gov.ar|2222222");
                ret = client.Search<Hl7.Fhir.Model.Patient>(qp);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }

            return ret;
        }

        private static Hl7.Fhir.Model.Patient GetPatient2(string patientId)
        {
            var baseUrl = $"http://hapi.fhir.org/baseDstu3";
            var getPatientUrl = $"http://hapi.fhir.org/baseDstu3/Patient/{patientId}";

            var client = new Hl7.Fhir.Rest.FhirClient(baseUrl);
        

            var version = client.VerifyFhirVersion;
            Console.WriteLine($"Version de FHIR:{version}");

            var ret = client.Read<Hl7.Fhir.Model.Patient>(getPatientUrl);
            return ret;
        }

        private static void CreatePatient()
        {
            //Create a patient resource instance
            var MyPatient = new Hl7.Fhir.Model.Patient();

            //Patient's Name
            var patientName = new Hl7.Fhir.Model.HumanName();
            patientName.Use = Hl7.Fhir.Model.HumanName.NameUse.Official;
            patientName.Prefix = new string[] { "Mr" };
            patientName.Given = new string[] { "Sam" };
            patientName.Family = "Fhirman";
            MyPatient.Name = new List<Hl7.Fhir.Model.HumanName>();
            MyPatient.Name.Add(patientName);

            //Patient Identifier 
            var patientIdentifier = new Hl7.Fhir.Model.Identifier();
            patientIdentifier.System = "http://ns.electronichealth.net.au/id/hi/ihi/1.0";
            patientIdentifier.Value = Guid.NewGuid().ToString();
            MyPatient.Identifier = new List<Hl7.Fhir.Model.Identifier>();
            MyPatient.Identifier.Add(patientIdentifier);

            var service = new Hl7.Fhir.Rest.FhirClient("http://hapi.fhir.org/baseDstu3");
            service.Timeout = (30 * 1000); // 30 segundos

            //Attempt to send the resource to the server endpoint
            Hl7.Fhir.Model.Patient returnedPatient = service.Create<Hl7.Fhir.Model.Patient>(MyPatient);

            try
            {
                //Attempt to send the resource to the server endpoint
                Hl7.Fhir.Model.Patient ReturnedPatient = service.Create<Hl7.Fhir.Model.Patient>(MyPatient);
                Console.WriteLine(string.Format("Resource is available at: {0}", ReturnedPatient.Id));
                Console.WriteLine();

                Console.WriteLine("This is what we sent up: ");
                Console.WriteLine();
                var serializer = new Hl7.Fhir.Serialization.FhirXmlSerializer();
                var xml = serializer.SerializeToString(MyPatient);
                Console.WriteLine(xml);


                Console.WriteLine();
                Console.WriteLine("This is what we received back: ");
                Console.WriteLine();
                xml = serializer.SerializeToString(ReturnedPatient);
                Console.WriteLine(xml);
                Console.WriteLine();
            }
            catch (Hl7.Fhir.Rest.FhirOperationException FhirOpExec)
            {
                //Process any Fhir Errors returned as OperationOutcome resource
                Console.WriteLine();
                Console.WriteLine("An error message: " + FhirOpExec.Message);
                Console.WriteLine();
                var serializer = new Hl7.Fhir.Serialization.FhirXmlSerializer();
                string xml = serializer.SerializeToString(FhirOpExec.Outcome);
                Console.WriteLine(xml);
            }
            catch (Exception GeneralException)
            {
                Console.WriteLine();
                Console.WriteLine("An error message: " + GeneralException.Message);
                Console.WriteLine();
            }

            Console.WriteLine("Press any key to end.");
            Console.ReadKey();
        }

        static void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.Console()
               .WriteTo.File("Logs\\log-.txt", rollingInterval: RollingInterval.Day)
               .CreateLogger();

            //Log.Information("Hello, world!");

            //int a = 10, b = 0;
            //try
            //{
            //    Log.Debug("Dividing {A} by {B}", a, b);
            //    Console.WriteLine(a / b);
            //}
            //catch (Exception ex)
            //{
            //    Log.Error(ex, "Something went wrong");
            //}

        }

        static void OnBeforeRequestFhirServer(object sender, Hl7.Fhir.Rest.BeforeRequestEventArgs e) 
        {
            var requestAdderss = e.RawRequest.Address.ToString();
            var requestBody = e.Body;

            Log.Information($"requestAdderss:{requestAdderss}");
            Log.Information($"requestBody:{requestBody}");

            // Replace with a valid bearer token for the server
            //e.RawRequest.Headers.Add("Authorization", "Bearer XXXXXXX");
        }

        static void OnAfterResponseFhirServer(object sender, Hl7.Fhir.Rest.AfterResponseEventArgs e)
        {
            Log.Information("Received response with status: " + e.RawResponse.StatusCode);
        }  
    }

   
}
