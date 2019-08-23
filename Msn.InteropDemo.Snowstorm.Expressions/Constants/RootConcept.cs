namespace Msn.InteropDemo.Snowstorm.Expressions.Constants
{
    public sealed class RootConcept
    {        
        public static readonly RootConcept Hallazgo = new RootConcept { SctId = "404684003", SctTerm = "hallazgo clínico" };

        public static readonly RootConcept FarmacoMedicamento = new RootConcept { SctId = "410942007", SctTerm = "fármaco o medicamento" };

        public static readonly RootConcept Procedimiento = new RootConcept { SctId = "71388002", SctTerm = "procedimiento" };

        public string SctId { get; private set; }
        public string SctTerm { get; private set; }
    }
}

