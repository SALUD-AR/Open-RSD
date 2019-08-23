using System.Collections.Generic;

namespace Msn.InteropDemo.ViewModel.Pacientes
{
    public class PacienteListViewModel
    {
        public PacienteListViewModel() => Items = new List<PacienteListItemViewModel>();

        public List<PacienteListItemViewModel> Items { get; set; }
    }
}
