using System.Runtime.Serialization;

namespace Telstra.Twins.Core
{
    public class Context 
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [DataMember(Name = "name", Order = 0)]
        public string Name { get; set; } = "https://schema.org";

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        [DataMember(Name = "@language", Order = 1)]
        public string Language { get; set; }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Context"/> to <see cref="string"/>.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The result of the conversion.</returns>
#pragma warning disable CA1062 // Validate arguments of public methods.
        public static implicit operator string(Context context) => context.Name;
#pragma warning restore CA1062 // Validate arguments of public methods
    }
}