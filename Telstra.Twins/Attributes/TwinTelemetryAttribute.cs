using System;

namespace Telstra.Twins.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TwinTelemetryAttribute : Attribute
    {
        public string SemanticType { get; set; }
        public string Unit { get; set; }
    }
}
