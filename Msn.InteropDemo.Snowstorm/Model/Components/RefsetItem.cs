namespace Msn.InteropDemo.Snowstorm.Model.Components
{
    public class RefsetItem
    {
        public bool active { get; set; }
        public bool released { get; set; }
        public int releasedEffectiveTime { get; set; }
        public string memberId { get; set; }
        public string moduleId { get; set; }
        public string refsetId { get; set; }
        public string referencedComponentId { get; set; }
        public ReferencedComponent referencedComponent { get; set; }
        public string effectiveTime { get; set; }
    }
}
