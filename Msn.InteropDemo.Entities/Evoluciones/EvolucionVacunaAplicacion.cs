using System;

namespace Msn.InteropDemo.Entities.Evoluciones
{
    public class EvolucionVacunaAplicacion
    {
        public int Id { get; set; }

        public int EvolucionId { get; set; }

        public Evolucion Evolucion { get; set; }

        public decimal? SctConceptId { get; set; }

        public string SctDescriptionTerm { get; set; }

        public DateTime? FechaAplicacion { get; set; }

        public string Lote { get; set; }

        public int? AplicacionNomivacVacunaId { get; set; }

        public int? AplicacionNomivacEsquemaId { get; set; }

        public int? NomivanImmunizationId { get; set; }

    }
}
