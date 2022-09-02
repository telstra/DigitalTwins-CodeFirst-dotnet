using System;
using Azure.DigitalTwins.Core;

namespace Telstra.Twins.Serialization
{
    public class ObjectToTwinComponentConverter<T> : ObjectToTwinConverter<T>
    {
        public override string[] SpecialTwinPropertyNames =>
            Array.Empty<string>();

        public override string[] TwinPropertyNamesToExclude => new[]
        {
            DigitalTwinsJsonPropertyNames.DigitalTwinId, DigitalTwinsJsonPropertyNames.DigitalTwinETag,DigitalTwinsJsonPropertyNames.DigitalTwinMetadata, "@id", "@type", "extends", "@context", "displayName"
        };
    }
}
