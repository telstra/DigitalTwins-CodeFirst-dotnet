using System;

namespace Telstra.Twins.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class TwinRelationshipAttribute : Attribute
    {
        /// <summary>
        ///     Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        public string Comment { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName { get; set; }

        /// <summary>
        ///     Gets or sets the relationship identifier.
        /// </summary>
        /// <value>The relationship identifier.</value>
        public string Id { get; set; }

        /// <summary>
        ///     Gets or sets the max multiplicity.
        /// </summary>
        /// <value>The max multiplicity.</value>
        public int MaxMultiplicity { get; set; }

        /// <summary>
        ///     Gets or sets the min multiplicity.
        /// </summary>
        /// <value>The min multiplicity.</value>
        public int MinMultiplicity { get; set; }

        /// <summary>
        ///     Gets or sets the relationship name.
        /// </summary>
        /// <value>The relationship name.</value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the relationship target.
        /// </summary>
        /// <value>The target.</value>
        public string Target { get; set; }

        /// <summary>
        ///     Gets or sets if the relationship is writable.
        /// </summary>
        /// <value>The value indicating if the relationship is writable.</value>
        public bool Writable { get; set; }
    }
}
