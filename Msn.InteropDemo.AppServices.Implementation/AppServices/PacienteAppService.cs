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
using Msn.InteropDemo.Common.Utils.Helpers;

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
                    op.AddError("El Federador no ha devuelto resultados.");
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
            //BUSQUEDA BY MATCH: NO UTULIZADA, SE DEJA A MODO DE DOCUENTACION
            //var sexo = (model.Sexo == "M") ? Sexo.Masculino : Sexo.Femenido;
            //var fn = DateTime.Parse(model.FechaNacimiento);
            //var request = new Msn.InteropDemo.Fhir.Model.Request.ExistPatientRequest
            //{
            //    Dni = model.NroDocumento.Value,
            //    FechaNacimiento = fn,
            //    Sexo = sexo,
            //    OtrosNombres = model.OtrosNombres,
            //    PrimerApellido = model.PrimerApellido,
            //    PrimerNombre = model.PrimerNombre
            //};

            //var exists = _patientManager.ExistsPatient(request);

            var model = GetById(pacienteId);

            if (model == null)
            {
                throw new Exception("No se ha encontrado el Paciente en la Base de datos Local");
            }

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

            var nombreSoundex = Common.Utils.Helpers.StringHelper.Soundex(nombre);
            var apellidoSoundex = Common.Utils.Helpers.StringHelper.Soundex(apellido);
           
            Expression<Func<Entities.Pacientes.Paciente, bool>> filterExp = x => x.PrimerApellidoSoundex == apellidoSoundex ||
                                                                                 x.PrimerNombreSoundex == nombreSoundex ||
                                                                                 x.NroDocumento == nroDocumento ||
                                                                                 x.FechaNacimiento == dtFechaNac;

            var lst = Get<PacienteListItemViewModel>(filter: filterExp,
                                                             includeProperties: "TipoDocumento",
                                                             orderBy: o=>o.OrderBy(n=>n.PrimerApellido).ThenBy(t=>t.PrimerNombre),
                                                             take: 50) //Solo los primeros 50 coincidentes
                                                             .ToList();

            decimal tmpScore;
            foreach (var item in lst)
            {
                tmpScore = MatchScoreHelper.MatchApellido.CalculateScore(item.PrimerApellido.ToLower(), apellido.ToLower());
                item.Score += tmpScore;
                item.PrimerApellidoEsCoincidente = (tmpScore == MatchScoreHelper.MatchApellido.MatchValue);

                tmpScore = MatchScoreHelper.MatchNombre.CalculateScore(item.PrimerNombre.ToLower(), nombre.ToLower());
                item.Score += tmpScore;
                item.PrimerNombreEsCoincidente = (tmpScore == MatchScoreHelper.MatchNombre.MatchValue);

                tmpScore = MatchScoreHelper.MatchNroDocumento.CalculateScore(item.NroDocumento.Value.ToString(), nroDocumento.ToString());
                item.Score += tmpScore;
                item.NroDocumentoEsCoincidente = (tmpScore == MatchScoreHelper.MatchNroDocumento.MatchValue);

                tmpScore = MatchScoreHelper.MatchFechaNacimiento.CalculateScore(item.FechaNacimientoPlane, dtFechaNac.ToString("yyyyMMdd"));
                item.Score += tmpScore;
                item.FechaNacimientoEsCoincidente = (tmpScore == MatchScoreHelper.MatchFechaNacimiento.MatchValue);

                if (item.TipoDocumentoId == (int)tipoDocumento)
                {
                    item.Score += MatchScoreHelper.MatchTipoDocumento.MatchValue;
                    item.TipoDocumentoEsCoincidente = true;
                }

                if (item.Sexo.ToLower() == sexo.ToLower())
                {
                    item.Score += MatchScoreHelper.MatchSexo.MatchValue;
                    item.SexoEsCoincidente = true;
                }
            }
            
            lst = lst.OrderByDescending(x => x.Score).ToList();

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

            entity.PrimerNombreSoundex = Common.Utils.Helpers.StringHelper.Soundex(entity.PrimerNombre);
            entity.PrimerApellidoSoundex = Common.Utils.Helpers.StringHelper.Soundex(entity.PrimerApellido);

            var op = base.Save(entity);
            op.Id = entity.Id;

            return op;
        }
    }
}
