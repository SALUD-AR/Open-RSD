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
using Msn.InteropDemo.ViewModel.Request;
using Msn.InteropDemo.ViewModel.Response;
using Microsoft.EntityFrameworkCore;

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

        public ViewModel.Response.CoeficienteBusquedaResponce GetCoeficienteBusqueda(CoeficienteBusquedaIngresadoRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var entity = CurrentContext.DataContext.Pacientes.Include(x => x.TipoDocumento)
                                                             .FirstOrDefault(x=>x.Id == request.PacienteId);
            if(entity == null)
            {
                throw new Exception($"Paciente inexistente con ID:{request.PacienteId}");
            }

            var rq = new CoeficienteBusquedaRequest
            {
                ApellidoIngresado = request.ApellidoIngresado,
                ApellidoObtenido = entity.PrimerApellido,
                FechaNacimientoIngresado = request.FechaNacimientoIngresado,
                FechaNacimientoObtenido = entity.FechaNacimiento.ToString("dd/MM/yyyy"),
                NombreIngresado = request.NombreIngresado,
                NombreObtenido = entity.PrimerNombre,
                NroDocumentoIngresado = request.NroDocumentoIngresado,
                NroDocumentoObtenido = entity.NroDocumento.ToString(),
                SexoIngresado = request.SexoIngresado,
                SexoObtenido = entity.Sexo,
                TipoDocumentoIngresado = request.TipoDocumentoIngresado,
                TipoDocumentoObtenido = entity.TipoDocumento.Nombre

            };

            var ret = GetCoeficienteBusqueda(rq);
            return ret;
        }

        public ViewModel.Response.CoeficienteBusquedaResponce GetCoeficienteBusqueda(CoeficienteBusquedaRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (!Common.Utils.Helpers.DateTimeHelper.TryParseFromAR(request.FechaNacimientoIngresado, out var dtFechaIngrasada))
            {
                throw new System.ArgumentException("Formato invalido debe ser: dd/mm/yyyy", nameof(request.FechaNacimientoIngresado));
            }
            if (!Common.Utils.Helpers.DateTimeHelper.TryParseFromAR(request.FechaNacimientoObtenido, out var dtFechaObtenida))
            {
                throw new System.ArgumentException("Formato invalido debe ser: dd/mm/yyyy", nameof(request.FechaNacimientoObtenido));
            }

            var ret = new ViewModel.Response.CoeficienteBusquedaResponce();

            var element = MatchScoreHelper.MatchApellido.GenerateScoreElement(request.ApellidoObtenido, request.ApellidoIngresado);
            ret.ApellidoScoreElement = Mapper.Map<CoeficienteScoreElementResponse>(element);
            ret.CoeficienteTotalFinal += ret.ApellidoScoreElement.CoeficienteFinal;

            element = MatchScoreHelper.MatchNombre.GenerateScoreElement(request.NombreObtenido, request.NombreIngresado);
            ret.NombreScoreElement = Mapper.Map<CoeficienteScoreElementResponse>(element);
            ret.CoeficienteTotalFinal += ret.NombreScoreElement.CoeficienteFinal;

            element = MatchScoreHelper.MatchNroDocumento.GenerateScoreElement(request.NroDocumentoObtenido, request.NroDocumentoIngresado);
            ret.NroDocumentoScoreElement = Mapper.Map<CoeficienteScoreElementResponse>(element);
            ret.CoeficienteTotalFinal += ret.NroDocumentoScoreElement.CoeficienteFinal;

            element = MatchScoreHelper.MatchFechaNacimiento.GenerateScoreElement(dtFechaObtenida.ToString("yyyyMMdd"), dtFechaIngrasada.ToString("yyyyMMdd"));
            element.Ingresado = request.FechaNacimientoIngresado;
            element.Obtenido = request.FechaNacimientoObtenido;
            ret.FechaNacimientoScoreElement = Mapper.Map<CoeficienteScoreElementResponse>(element);
            ret.CoeficienteTotalFinal += ret.FechaNacimientoScoreElement.CoeficienteFinal;

            ret.TipoDocumentoScoreElement = new CoeficienteScoreElementResponse
            {
                PesoValor = MatchScoreHelper.MatchTipoDocumento.MatchValue,
                PesoValorUI = MatchScoreHelper.MatchTipoDocumento.MatchValue.ToString("P2"),
                Ingresado = request.TipoDocumentoIngresado,
                Obtenido = request.TipoDocumentoObtenido,
                LevenshteinDistante = $"(no aplicable)",
                CoeficienteParcialUI = $"(no aplicable)",
                ObtenidoLen = $"(no aplicable)",
                CoeficienteFinal = (request.TipoDocumentoIngresado == request.TipoDocumentoObtenido) ? MatchScoreHelper.MatchTipoDocumento.MatchValue : 0,
                CoeficienteFinalUI = (request.TipoDocumentoIngresado == request.TipoDocumentoObtenido) ? MatchScoreHelper.MatchTipoDocumento.MatchValue.ToString("P2") : 0M.ToString("P2")
            };
            ret.CoeficienteTotalFinal += ret.TipoDocumentoScoreElement.CoeficienteFinal;

            ret.SexoScoreElement = new CoeficienteScoreElementResponse
            {
                PesoValor = MatchScoreHelper.MatchSexo.MatchValue,
                PesoValorUI = MatchScoreHelper.MatchSexo.MatchValue.ToString("P2"),
                Ingresado = request.SexoIngresado,
                Obtenido = request.SexoObtenido,
                LevenshteinDistante = $"(no aplicable)",
                CoeficienteParcialUI = $"(no aplicable)",
                ObtenidoLen = $"(no aplicable)",
                CoeficienteFinal = (request.SexoIngresado== request.SexoObtenido) ? MatchScoreHelper.MatchSexo.MatchValue : 0,
                CoeficienteFinalUI = (request.SexoIngresado == request.SexoObtenido) ? MatchScoreHelper.MatchSexo.MatchValue.ToString("P2") : 0M.ToString("P2")
            };
            ret.CoeficienteTotalFinal += ret.SexoScoreElement.CoeficienteFinal;

            ret.CoeficietneTotalFinalUI = ret.CoeficienteTotalFinal.ToString("P2");

            return ret;
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
                                                             orderBy: o => o.OrderBy(n => n.PrimerApellido).ThenBy(t => t.PrimerNombre),
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
