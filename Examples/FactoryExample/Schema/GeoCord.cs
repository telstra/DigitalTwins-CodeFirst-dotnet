using Telstra.Twins.Attributes;

namespace FactoryExample.Schema
{
    [DigitalTwin(Version = 1)]
    public class GeoCord
    {
        [TwinProperty("lat")] public double Latitude { get; set; }
        [TwinProperty("lon")] public double Longitude { get; set; }
    }
}
