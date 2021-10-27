#nullable enable
using System;
using Telstra.Twins.Helpers;

namespace Telstra.Twins.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class TwinComponentAttribute : Attribute
    {
        public TwinComponentAttribute()
        {

        }

        public string? Name { get; set; }
        public string? Schema { get; set; }
        public string? Comment { get; set; }
        public string? Description { get; set; }
        public string? DisplayName { get; set; }

        public Type SchemaTypev { 
            set
            {
                this.Schema = value.GetDigitalTwinModelId();
            }
        }
    }
}
