namespace Msn.InteropDemo.Entities.Evoluciones
{
    public class EvolucionDiagnosticoCie10
    {
        public int Id { get; set; }

        public int EvolucionDiagnosticoId { get; set; }

        public EvolucionDiagnostico EvolucionDiagnostico { get; set; }
        
        public string Cie10SubcategoriaId { get; set; }

        public string Cie10SubcategoriaNombre { get; set; }
    }
}
