#nullable enable
using System;

namespace Telstra.Twins.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TwinTelemetryAttribute : Attribute
    {
        /// <summary>
        ///     Gets or sets the semantic type of the telemetry.
        /// </summary>
        public string? SemanticType { get; set; }

        /// <summary>
        ///     Gets or sets the units of the semantic telemetry.
        /// </summary>
        public string? Unit { get; set; }
    }
}
