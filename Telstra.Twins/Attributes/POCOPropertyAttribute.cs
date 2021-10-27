using System;

namespace Telstra.Twins.Attributes
{
    /// <summary>
    /// Indicates a property should only be serialized when the POCO schema is required
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class POCOPropertyAttribute : Attribute
    {
    }
}
