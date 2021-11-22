#nullable enable
using System;

namespace Telstra.Twins.Attributes
{
    /// <summary>
    ///     Specifies that a property is available both to the digital twin and the digital twin model.
    ///     It also optionally allows the property name to be specified.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class TwinPropertyAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of <see cref="TwinPropertyAttribute" />.
        /// </summary>
        public TwinPropertyAttribute()
        {
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="TwinPropertyAttribute" /> with the specified model property
        ///     name.
        /// </summary>
        /// <param name="name">The name of the model property.</param>
        public TwinPropertyAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        ///     The name of the digital twin property.
        /// </summary>
        public string? Name { get; }

        /// <summary>
        ///     Gets or sets a value to manually override the generated schema for the property.
        /// </summary>
        public string? Schema { get; set; }

        /// <summary>
        ///     Gets or sets the semantic type of the property.
        /// </summary>
        public string? SemanticType { get; set; }

        /// <summary>
        ///     Gets or sets the units of the semantic property.
        /// </summary>
        public string? Unit { get; set; }
    }
}
