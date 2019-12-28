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
                                                    .Include(p => p.NomivacVacuna)
                                                    .Include(p => p.NomivacEsquema)
                                                    .Where(x => x.NomivacVacuna.SctId == sctId)
                                                    .Select(x => x.NomivacEsquema)
                                                    .ToListAsync();
            return lst;
        }

        public int RegistrarAplicacionNomivac(int pacienteId,
                                              int evolucionVacunaAplicacionId,
                                              decimal vacunaSctId,
                                              int nomivacEsquemaId,
                                              DateTime fechaAplicacion)
        {


            var paciente = _currentContext.DataContext.Pacientes.Find(pacienteId);
            if (paciente == null)
            {
                throw new Exception($"Paciente no encontrado con el ID:{pacienteId}");
            }

            var vacuna = _currentContext.DataContext.NomivacVacunas.FirstOrDefault(x => x.SctId == vacunaSctId);
            if (vacuna == null)
            {
                throw new Exception($"Vacuna Nomivac no encontrada con el SctID:{vacunaSctId}");
            }

            var nomivacEsquema = _currentContext.DataContext.NomivacEsquemas.Find(nomivacEsquemaId);
            if (nomivacEsquema == null)
            {
                throw new Exception($"Esquema no encontrado para el ID:{nomivacEsquemaId}");
            }

            var nomivacVacunaId = vacuna.Id;
            if (!_currentContext.DataContext.NomivacVacunaEsquemas.Any(x => x.NomivacEsquemaId == nomivacEsquemaId && x.NomivacVacunaId == nomivacVacunaId))
            {
                throw new Exception($"Esquema Nomivac no encontrado para la Vacuna ID:{nomivacVacunaId} y el Esquema ID:{nomivacEsquemaId}");
            }

            //TODO: Falta filtar por las que No estan aplicadas, falta agregar las columnas en la Tabla para 
            //Los datos del registro de la Aplicacion
            var evolucionVacuna = _currentContext.DataContext.EvolucionVacunaAplicaciones
                                                    .Include(e => e.Evolucion)
                                                    .FirstOrDefault(p => p.Id == evolucionVacunaAplicacionId
                                                                        && p.Evolucion.PacienteId == pacienteId
                                                                        && p.SctConceptId.Value == vacunaSctId);
            if (evolucionVacuna == null)
            {
                throw new Exception($"No se ha encontrado la Evolucion para ID:{evolucionVacunaAplicacionId}, PacienteID:{pacienteId}, VacunaSctId{vacunaSctId}");
            }


            var model = new Fhir.Model.Request.RegistrarInmunizationRequest
            {
                AplicacionVacunaFecha = fechaAplicacion,
                CurrentLocationId = "10060492200870",
                CurrentLocationName = "Hospital Municipal Hospital Doctor Ángel Pintos",
                DNI = paciente.NroDocumento,
                FechaNacimiento = paciente.FechaNacimiento,
                LocalPacienteId = paciente.Id.ToString(),
                PrimerApellido = paciente.PrimerApellido,
                PrimerNombre = paciente.PrimerNombre,
                SctConceptId = vacuna.SctId.ToString(),
                SctTerm = vacuna.SctTerm,
                Sexo = paciente.Sexo == "M" ? Common.Constants.Sexo.Masculino : Common.Constants.Sexo.Femenido,
                VacunaEsquemaId = nomivacEsquemaId.ToString(),
                VacunaEsquemaNombre = nomivacEsquema.Nombre
                //VacunaLote = "649718"
            };

            var response = _immunizationManager.RegistrarAplicacionVacuna(model);

            if (string.IsNullOrWhiteSpace(response.Id))
            {
                throw new Exception("No se ha obtenido una respuesta del Servicio Nomivac");
            }


            var immunizationId = int.Parse(response.Id);

            //TODO: Actualizar en la tabla la Entity evolucionVacuna con la info. obtenida. 

            return immunizationId;
        }

    }
}
