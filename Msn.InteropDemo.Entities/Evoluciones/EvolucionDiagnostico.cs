namespace Msn.InteropDemo.Entities.Evoluciones
{
    public class EvolucionDiagnostico
    {
        public int Id { get; set; }

        public int EvolucionId { get; set; }

        public Evolucion Evolucion { get; set; }

        public decimal? SctConceptId { get; set; }

        public string SctDescriptionTerm { get; set; }
    }
}
