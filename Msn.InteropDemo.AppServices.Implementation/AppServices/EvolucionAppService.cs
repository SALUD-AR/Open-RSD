using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Msn.InteropDemo.AppServices.Core;
using Msn.InteropDemo.Snowstorm;
using Msn.InteropDemo.ViewModel.Evoluciones;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Msn.InteropDemo.AppServices.Implementation.AppServices
{
    public class EvolucionAppService : Core.GenericService<Entities.Evoluciones.Evolucion>, IEvolucionAppService
    {
        private readonly ISnowstormManager _snowstormManager;

        public EvolucionAppService(Snowstorm.ISnowstormManager snowstormManager,
                                   ICurrentContext currentContext,
                                   ILogger<EvolucionAppService> logger) : base(currentContext, logger)
        {
            _snowstormManager = snowstormManager;
        }

        public EvolucionViewModel GetById(int evolucionId)
        {
            var entity = GetById<EvolucionViewModel>(criteria: x => x.Id == evolucionId,
                                                     includeProperties: "Diagnosticos.Cie10Mapeos,Medicamentos,Vacunas,CreatedUser");
            return entity;
        }

        public IEnumerable<EvolucionDiagnosticoViewModel> GetDiagnosticos(int evolucionId)
        {
            var entities = CurrentContext.DataContext.EvolucionDiagnosticos.Where(x => x.EvolucionId == evolucionId);
            var model = Mapper.Map<IEnumerable<ViewModel.Evoluciones.EvolucionDiagnosticoViewModel>>(entities);
            return model;
        }


        public IEnumerable<ViewModel.Snomed.SnomedItem> SearchSnowstormByExpressionTerm(string expression, string term)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                throw new ArgumentException("message", nameof(expression));
            }

            if (string.IsNullOrWhiteSpace(term))
            {
                throw new ArgumentException("message", nameof(term));
            }

            IEnumerable<ViewModel.Snomed.SnomedItem> items;

            term = term.Trim();
            var qr = _snowstormManager.RunQuery(expression, term);
            items = Mapper.Map<IEnumerable<ViewModel.Snomed.SnomedItem>>(qr.Items);

            items = items.OrderBy(x => x.Description.Length).ToList();

            return items;
        }

        public IEnumerable<ViewModel.Snomed.SnomedItem> SearchSnowstormHallazgos(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                throw new ArgumentException("message", nameof(term));
            }

            var expBuilder = new Snowstorm.Expressions.ExpresionBuilders.HallazgoExpBuilder(term);
            var qr = _snowstormManager.RunQuery(expBuilder);
            var items = Mapper.Map<IEnumerable<ViewModel.Snomed.SnomedItem>>(qr.Items);

            items = items.OrderBy(x => x.Description.Length);

            return items;
        }

        public List<EvolucionHistoItemViewModel> GetEvolucionHistoDates(int pacienteId)
        {
            var items = Get<EvolucionHistoItemViewModel>(filter: x => x.PacienteId == pacienteId,
                                                         includeProperties: "Paciente,CreatedUser")
                                                            .OrderByDescending(x => x.CreatedDateTime)
                                                            .ToList();

            return items;
        }

        public EvolucionHistoViewModel GetEvolucionesHisto(int pacienteId)
        {
            var entity = CurrentContext.DataContext.Pacientes
                            .Include(x => x.TipoDocumento)
                            .FirstOrDefault(x => x.Id == pacienteId);

            if (entity == null)
            {
                throw new Exception($"Paciente No encontrado ID: {pacienteId}");
            }

            var model = Mapper.Map<EvolucionHistoViewModel>(entity);

            var items = Get<EvolucionHistoItemViewModel>(filter: x => x.PacienteId == pacienteId,
                                                   includeProperties: "Paciente,CreatedUser")
                                                   .ToList();

            model.Items = items;

            return model;
        }
    }
}
