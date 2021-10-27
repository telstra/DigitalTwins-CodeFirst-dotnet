using Azure.DigitalTwins.Core;

namespace Telstra.Twins.Serialization
{
    public class ObjectToTwinComponentConverter<T> : ObjectToTwinConverter<T>
    {
        public override string[] SpecialTwinPropertyNames =>
            new[] { DigitalTwinsJsonPropertyNames.DigitalTwinMetadata };

        public override string[] TwinPropertyNamesToExclude => new[]
        {
            DigitalTwinsJsonPropertyNames.DigitalTwinId, DigitalTwinsJsonPropertyNames.DigitalTwinETag, "@id", "@type", "extends", "@context", "displayName" 
        };
    }
}
