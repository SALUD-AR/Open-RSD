using System.Collections.Generic;

namespace Msn.InteropDemo.Entities.Evoluciones
{
    public class EvolucionDiagnostico
    {
        public int Id { get; set; }

        public int EvolucionId { get; set; }

        public Evolucion Evolucion { get; set; }

        public decimal? SctConceptId { get; set; }

        public string SctDescriptionTerm { get; set; }

        public string Cie10SubcategoriaId { get; set; }

        public string Cie10SubcategoriaNombre { get; set; }

        public ICollection<EvolucionDiagnosticoCie10> Cie10Mapeos { get; set; }
    }
}
