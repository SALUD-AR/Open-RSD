using System;
using System.Collections.Generic;
using System.Text;

namespace Msn.InteropDemo.ViewModel.Snomed
{
    public class Cie10MapResultViewModel
    {
        public string ConceptId { get; set; }
        public string Description { get; set; }
        public string FSN { get; set; }
        public string Language { get; set; }
        public string SubcategoriaId { get; set; }
        public string SubcategoriaNombre { get; set; }
        public string CategoriaNombre { get; set; }
        public string MapAdvice { get; set; }
        public int MapGroup { get; set; }
        public int MapPriority { get; set; }
        public string MapRule { get; set; }
        public string MapTarget { get; set; }
    }
}
