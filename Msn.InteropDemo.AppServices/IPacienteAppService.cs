using Msn.InteropDemo.Common.Constants;
using Msn.InteropDemo.ViewModel.Pacientes;
using System.Collections.Generic;

namespace Msn.InteropDemo.AppServices
{
    public interface IPacienteAppService : Core.IGenericService<Entities.Pacientes.Paciente>
    {

        List<PacienteListItemViewModel> BuscarPacienteCoincidentesLocal(string apellido,
                                                               string nombre,
                                                               TipoDocumento tipoDocumento,
                                                               int nroDocumento,
                                                               string sexo,
                                                               string fechaNacimiento);
        bool ExistePacienteEnBUS(int pacienteId);
        Common.OperationResults.OperationResult FederarPaciente(int pacienteId);
        PacienteViewModel GetById(int pacienteId);
    }
}
