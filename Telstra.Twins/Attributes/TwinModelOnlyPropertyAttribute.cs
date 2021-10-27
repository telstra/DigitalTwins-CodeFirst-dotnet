using System;

namespace Telstra.Twins.Attributes
{
    /// <summary>
    /// Specifies that a property is only shown in the digital twin model and optionally
    /// allows the property name to be specified.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class TwinModelOnlyPropertyAttribute : TwinPropertyAttribute
    {
        /// <summary>
        /// Initializes a new instance of <see cref="TwinModelOnlyPropertyAttribute"/>.
        /// </summary>
        public TwinModelOnlyPropertyAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TwinModelOnlyPropertyAttribute"/> with the specified model property name.
        /// </summary>
        /// <param name="name">The name of the model property.</param>
        public TwinModelOnlyPropertyAttribute(string name) : base(name)
        {
        }
    }
}
