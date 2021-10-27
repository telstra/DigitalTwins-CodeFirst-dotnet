using System;
using System.Runtime.Serialization;
using Telstra.Twins.Attributes;
using Telstra.Twins.Models;

namespace Telstra.Twins
{
    [DigitalTwin(Version = 1, DisplayName = "Telstra Device")]
    public class Device : TwinBase
    {
        [TwinProperty]
        public string VendorId { get; set; }
        [TwinProperty]
        public string DeviceId { get; set; }
    }
}
