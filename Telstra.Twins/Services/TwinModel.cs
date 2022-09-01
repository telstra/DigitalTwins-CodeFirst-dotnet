using System.Collections.Generic;
using System.Reflection;
using Telstra.Twins.Models;

namespace Telstra.Twins.Services
{
    public record TwinModel
    {
        public List<Content> contents { get; init; }

        public Dictionary<PropertyInfo, ModelRelationship> Relationships { get; init; }
        public Dictionary<PropertyInfo, ModelRelationship> ExtendingRelationships { get; init; } = new();
    }
}
