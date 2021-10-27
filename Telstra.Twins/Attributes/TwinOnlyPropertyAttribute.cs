using System;

namespace Telstra.Twins.Attributes
{
    /// <summary>
    /// Indicates a property should only be serialized when the twin is being serialized, rather than the model
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class TwinOnlyPropertyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of <see cref="TwinOnlyPropertyAttribute"/>.
        /// </summary>
        public TwinOnlyPropertyAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TwinOnlyPropertyAttribute"/> with the specified twin property name.
        /// </summary>
        /// <param name="name">The name of the model property.</param>
        public TwinOnlyPropertyAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// The name of the twin only property.
        /// </summary>
        public string Name { get; }
    }
}
