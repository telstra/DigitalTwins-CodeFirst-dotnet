using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Telstra.Twins.Models;

namespace Telstra.Twins.Services
{
    public record TwinModel
    {
        public List<Content> contents { get; init; }

        public Dictionary<PropertyInfo, ModelRelationship> Relationships { get; init; } = new();
        public Dictionary<PropertyInfo, ModelRelationship> ExtendingRelationships { get; init; } = new();

        public Dictionary<PropertyInfo, ModelRelationship> AllRelationships => Relationships
            .Union(ExtendingRelationships)
            .ToDictionary(pair => pair.Key, pair => pair.Value);
    }
}
