﻿using System;
using System.Collections.Generic;
using FactoryExample.Schema;
using Telstra.Twins;
using Telstra.Twins.Attributes;

namespace FactoryExample.Models
{
    [DigitalTwin(Version = 1, DisplayName = "Digital Factory - Interface Model")]
    public class Factory : TwinBase
    {
        [TwinProperty] public string? Country { get; set; }

        [TwinProperty] public string? FactoryId { get; set; }

        [TwinProperty(Writable = true)] public string? FactoryName { get; set; }

        [TwinRelationship(DisplayName = "Has Floors")]
        public IList<FactoryFloor> Floors { get; } = new List<FactoryFloor>();

        [TwinProperty] public GeoCord? GeoLocation { get; set; }

        [TwinProperty] public DateTimeOffset LastSupplyDate { get; set; }

        // ServesRetailer
        // SuppliedBy
        // TransportationBy

        [TwinProperty(Writable = true)] public string? Tags { get; set; }

        [TwinProperty(Writable = true)] public string? ZipCode { get; set; }
    }
}
