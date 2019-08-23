using Msn.InteropDemo.AppServices.Core;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Msn.InteropDemo.Common.OperationResults;
using System;
using Msn.InteropDemo.Common.Constants;
using Msn.InteropDemo.ViewModel.Pacientes;
using Msn.InteropDemo.Fhir;
using System.Linq.Expressions;

namespace Msn.InteropDemo.AppServices.Implementation.AppServices
{
    public class PacienteAppService : Core.GenericService<Entities.Pacientes.Paciente>, IPacienteAppService
    {
        private readonly IPatientManager _patientManager;

        public PacienteAppService(IPatientManager patientManager,
                                  ICurrentContext context,
                                  ILogger<PacienteAppService> logger) : base(context, logger)
        {
            _patientManager = patientManager;
        }


        public Common.OperationResults.OperationResult FederarPaciente(int pacienteId)
        {
            var op = new OperationResult();

            try
            {
                var entity = CurrentContext.DataContext.Pacientes.Find(pacienteId);

                var request = new Fhir.Model.Request.FederarPacienteRequest
                {
                    DNI = entity.NroDocumento,
                    FechaNacimiento = entity.FechaNacimiento,
                    Sexo = entity.Sexo == "M" ? Sexo.Masculino : Sexo.Femenido,
                    LocalId = entity.Id.ToString(),
                    PrimerNombre = entity.PrimerNombre,
                    PrimerApellido = entity.PrimerApellido,
                    OtrosNombres = entity.OtrosNombres
                };

                var resp = _patientManager.FederarPaciente(request);
                if (resp.Id.HasValue)
                {
                    entity.FederadoDateTime = DateTime.Now;
                    entity.FederadorId = resp.Id.Value;
                    CurrentContext.DataContext.SaveChanges();

                    op.Id = resp.Id.Value;
                }
                else
                {
                    op.AddError("El registro en el BUS noha devuelto resultados.");
                }

            }
            catch (Exception ex)
            {
                var strError = $"Error registrando Paciente en el BUS: {ex.Message}";
                op.AddError(strError);
                Logger.LogError(ex, strError);
            }

            return op;
        }

        public bool ExistePacienteEnBUS(int pacienteId)
        {
            var model = GetById(pacienteId);

            if (model == null)
            {
                throw new Exception("No se ha encontrado el Paciente en la Base de datos Local");
            }

            var sexo = (model.Sexo == "M") ? Sexo.Masculino : Sexo.Femenido;
            var fn = DateTime.Parse(model.FechaNacimiento);

            var request = new Msn.InteropDemo.Fhir.Model.Request.ExistPatientRequest
            {
                Dni = model.NroDocumento.Value,
                FechaNacimiento = fn,
                Sexo = sexo,
                OtrosNombres = model.OtrosNombres,
                PrimerApellido = model.PrimerApellido,
                PrimerNombre = model.PrimerNombre
            };

            //Busqueda By Match
            //var exists = _patientManager.ExistsPatient(request);
            var exists = _patientManager.ExistsPatient(DomainName.LocalDomain, pacienteId.ToString());

            return exists;
        }

        public PacienteViewModel GetById(int pacienteId)
        {
            var model = GetById<PacienteViewModel>(x => x.Id == pacienteId, includeProperties: "TipoDocumento");
            return model;
        }

        public List<PacienteListItemViewModel> BuscarPacienteCoincidentesLocal(string apellido,
                                                               string nombre,
                                                               TipoDocumento tipoDocumento,
                                                               int nroDocumento,
                                                               string sexo,
                                                               string fechaNacimiento)
        {
            if (string.IsNullOrWhiteSpace(apellido))
            {
                throw new System.ArgumentException("No provisto", nameof(apellido));
            }

            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new System.ArgumentException("No provisto", nameof(nombre));
            }

            if (string.IsNullOrWhiteSpace(sexo))
            {
                throw new System.ArgumentException("No provisto", nameof(sexo));
            }
            if (string.IsNullOrWhiteSpace(fechaNacimiento))
            {
                throw new System.ArgumentException("No provisto", nameof(fechaNacimiento));
            }
            if (!Common.Utils.Helpers.DateTimeHelper.TryParseFromAR(fechaNacimiento, out var dtFechaNac))
            {
                throw new System.ArgumentException("Formato invalido debe ser: dd/mm/yyyy", nameof(fechaNacimiento));
            }

            Expression<Func<Entities.Pacientes.Paciente, bool>> filterExp = x => x.PrimerNombre.ToLower().StartsWith(nombre.ToLower()) ||
                                                                              x.PrimerApellido.ToLower().StartsWith(apellido.ToLower()) ||
                                                                              x.NroDocumento == nroDocumento ||
                                                                              //x.Sexo == sexo || (no tiene sentido buscar por sexo y Tipo doc.)
                                                                              x.FechaNacimiento == dtFechaNac;

            var lst = Get<ViewModel.Pacientes.PacienteListItemViewModel>(filter: filterExp,
                                                                         includeProperties: "TipoDocumento",
                                                                         take: 50) //Solo los primeros 50 coincidentes
                                                                         .ToList();

            foreach (var item in lst)
            {
                if (item.PrimerApellido.ToLower() == apellido.ToLower())
                {
                    item.Score += (decimal)0.2;
                    item.PrimerApellidoEsCoincidente = true;
                }
                if (item.PrimerNombre.ToLower() == nombre.ToLower())
                {
                    item.Score += (decimal)0.1;
                    item.PrimerNombreEsCoincidente = true;
                }
                if (item.TipoDocumentoId == (int)tipoDocumento)
                {
                    item.Score += (decimal)0.1;
                    item.TipoDocumentoEsCoincidente = true;
                }
                if (item.NroDocumento == (int)nroDocumento)
                {
                    item.Score += (decimal)0.3;
                    item.NroDocumentoEsCoincidente = true;
                }
                if (item.Sexo.ToLower() == sexo.ToLower())
                {
                    item.Score += (decimal)0.1;
                    item.SexoEsCoincidente = true;
                }
                if (item.FechaNacimiento == dtFechaNac.ToString("dd/MM/yyyy"))
                {
                    item.Score += (decimal)0.2;
                    item.FechaNacimientoEsCoincidente = true;
                }
            }

            lst = lst.OrderByDescending(x => x.Score).ToList();

            //CurrentContext.RegisterActivityLog(Entities.Activity.ActivityType.SEARCH_PACIENTES_COINCIDENTES_DB_LOCAL,
            //                                   filterExp.ToString(),
            //                                   $"Pacientes obtenidos:{lst.Count}");
            
            return lst;
        }

        public override OperationResult<int> Save(Entities.Pacientes.Paciente entity)
        {
            if (entity == null)
            {
                throw new System.ArgumentNullException(nameof(entity));
            }

            if (string.IsNullOrWhiteSpace(entity.PrimerApellido))
            {
                throw new Exception("El apellido es requerido.");
            }
            if (string.IsNullOrWhiteSpace(entity.PrimerNombre))
            {
                throw new Exception("El nombre es requerido.");
            }
            if (string.IsNullOrWhiteSpace(entity.Sexo))
            {
                throw new Exception("El sexo es requerido.");
            }

            var op = base.Save(entity);
            op.Id = entity.Id;

            //CurrentContext.RegisterActivityLog(Entities.Activity.ActivityType.CREATE_PACIENTE_EN_DB_LOCAL,
            //                                   entity.ToString(),
            //                                   $"Paciente generado:{entity.Id}");

            return op;
        }
    }
}
