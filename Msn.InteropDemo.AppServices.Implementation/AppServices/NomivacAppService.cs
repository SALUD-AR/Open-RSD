using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Msn.InteropDemo.AppServices.Core;
using Msn.InteropDemo.Fhir;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Msn.InteropDemo.AppServices.Implementation.AppServices
{
    public class NomivacAppService : INomivacAppService
    {
        private readonly ICurrentContext _currentContext;
        private readonly IImmunizationManager _immunizationManager;
        private readonly ILogger<NomivacAppService> _logger;

        public NomivacAppService(ICurrentContext currentContext,
                                 IImmunizationManager immunizationManager,
                                 ILogger<NomivacAppService> logger)
        {
            _currentContext = currentContext;
            _immunizationManager = immunizationManager;
            _logger = logger;
        }

        public async Task<IEnumerable<Entities.Nomivac.NomivacEsquema>> GetEsquemasForVacunaSctIdAsync(decimal sctId)
        {
            var lst = await _currentContext.DataContext.NomivacVacunaEsquemas
                                                    .AsNoTracking()
                                                    .Include(p => p.NomivacVacuna)
                                                    .Include(p => p.NomivacEsquema)
                                                    .Where(x => x.NomivacVacuna.SctId == sctId)
                                                    .Select(x => x.NomivacEsquema)
                                                    .ToListAsync();
            return lst;
        }

        public async Task<int> RegistrarAplicacionNomivacAsync(int evolucionVacunaAplicacionId,
                                              int nomivacEsquemaId,
                                              DateTime fechaAplicacion)
        {

            var nomivacEsquema = _currentContext.DataContext.NomivacEsquemas.Find(nomivacEsquemaId);
            if (nomivacEsquema == null)
            {
                _logger.LogError($"Esquema no encontrado para el ID:{nomivacEsquemaId}");
                throw new Exception($"Esquema no encontrado para el ID:{nomivacEsquemaId}");
            }
            
            var evolucionVacuna = _currentContext.DataContext.EvolucionVacunaAplicaciones
                                                    .Include(e => e.Evolucion.Paciente)
                                                    .FirstOrDefault(p => p.Id == evolucionVacunaAplicacionId);
            if (evolucionVacuna == null)
            {
                _logger.LogError($"No se ha encontrado la Evolucion para ID:{evolucionVacunaAplicacionId}, PacienteID:{evolucionVacuna.Evolucion.Paciente.Id}, VacunaSctId{evolucionVacuna.SctConceptId}");
                throw new Exception($"No se ha encontrado la Evolucion para ID:{evolucionVacunaAplicacionId}, PacienteID:{evolucionVacuna.Evolucion.Paciente.Id}, VacunaSctId{evolucionVacuna.SctConceptId}");
            }

            var vacuna = _currentContext.DataContext.NomivacVacunas.FirstOrDefault(x => x.SctId == evolucionVacuna.SctConceptId);
            if (vacuna == null)
            {
                _logger.LogError($"Vacuna Nomivac no encontrada con el SctID:{evolucionVacuna.SctConceptId}");
                throw new Exception($"Vacuna Nomivac no encontrada con el SctID:{evolucionVacuna.SctConceptId}");
            }

            var nomivacVacunaId = vacuna.Id;
            if (!_currentContext.DataContext.NomivacVacunaEsquemas.Any(x => x.NomivacEsquemaId == nomivacEsquemaId && x.NomivacVacunaId == nomivacVacunaId))
            {
                _logger.LogError($"Esquema Nomivac no encontrado para la Vacuna ID:{nomivacVacunaId} y el Esquema ID:{nomivacEsquemaId}");
                throw new Exception($"Esquema Nomivac no encontrado para la Vacuna ID:{nomivacVacunaId} y el Esquema ID:{nomivacEsquemaId}");
            }

            var model = new Fhir.Model.Request.RegistrarInmunizationRequest
            {
                AplicacionVacunaFecha = fechaAplicacion,
                CurrentLocationId = "10060492200870",
                CurrentLocationName = "Hospital Municipal Hospital Doctor Ángel Pintos",
                DNI =  evolucionVacuna.Evolucion.Paciente.NroDocumento,
                FechaNacimiento = evolucionVacuna.Evolucion.Paciente.FechaNacimiento,
                LocalPacienteId = evolucionVacuna.Evolucion.Paciente.Id.ToString(),
                PrimerApellido = evolucionVacuna.Evolucion.Paciente.PrimerApellido,
                PrimerNombre = evolucionVacuna.Evolucion.Paciente.PrimerNombre,
                SctConceptId = vacuna.SctId.ToString(),
                SctTerm = vacuna.SctTerm,
                Sexo = evolucionVacuna.Evolucion.Paciente.Sexo == "M" ? Common.Constants.Sexo.Masculino : Common.Constants.Sexo.Femenido,
                VacunaEsquemaId = nomivacEsquemaId.ToString(),
                VacunaEsquemaNombre = nomivacEsquema.Nombre
                //VacunaLote = string.Empty
            };

            Fhir.Model.Response.RegistrarImmunizationResponse response;
            try
            {
                response = await _immunizationManager.RegistrarAplicacionVacunaAsync(model);
            }
            catch (Hl7.Fhir.Rest.FhirOperationException ex)
            {
                string msg = string.Empty;
                foreach (var item in ex.Outcome.Issue)
                {
                    msg += item.Diagnostics;
                }

                _logger.LogError(ex, "Error no controlado registrando Aplicacion Nomivac.");
                throw new Exception($"{msg}");
            }

            if (string.IsNullOrWhiteSpace(response.Id))
            {
                _logger.LogError("No se ha obtenido una respuesta del Servicio Nomivac.");
                throw new Exception("No se ha obtenido una respuesta del Servicio Nomivac.");
            }

            var immunizationId = int.Parse(response.Id);
            evolucionVacuna.AplicacionNomivacEsquemaId = nomivacEsquemaId;
            evolucionVacuna.AplicacionNomivacVacunaId = nomivacVacunaId;
            evolucionVacuna.FechaAplicacion = fechaAplicacion;
            evolucionVacuna.NomivanImmunizationId = immunizationId;

            _currentContext.DataContext.SaveChanges();

            return immunizationId;
        }

    }
}
