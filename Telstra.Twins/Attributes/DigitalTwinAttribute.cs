#nullable enable
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
// ReSharper disable InconsistentNaming

namespace Telstra.Twins.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class DigitalTwinAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string? DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public int Version { get; set; } = 1;

        /// <summary>
        /// Gets or sets the type of digital twin model.
        /// The default is 'Interface'.
        /// </summary>
        /// <value>The new value.</value>
        public string? ModelType { get; set; } = "Interface";

        /// <summary>
        /// Gets full model Id.
        /// </summary>
        public string GetFullModelId(Type t)
        {
            var modelIdResult = $"dtmi:{t.Namespace?.ToLower().Replace(".", ":")}:{t.Name.ToLower()};{this.Version}";
            return modelIdResult;
        }

        /// <summary>
        /// Gets or sets the parent model Id that this digital twin extends.
        /// </summary>
        /// <value>The new parent model Id.</value>
        public string? ExtendsModelId { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Type"/> of the <see cref="NamingStrategy"/>.
        /// </summary>
        /// <value>The <see cref="Type"/> of the <see cref="NamingStrategy"/>.</value>
        public Type? NamingStrategyType
        {
            get => _namingStrategyType;
            set
            {
                _namingStrategyType = value;
                NamingStrategyInstance = null;
            }
        }

        private Type? _namingStrategyType;
        internal NamingStrategy? NamingStrategyInstance { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalTwinAttribute"/> class.
        /// </summary>
        public DigitalTwinAttribute()
        {
            NamingStrategyType = typeof(CamelCaseNamingStrategy);
        }
    }
}
