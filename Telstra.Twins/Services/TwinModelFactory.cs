using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Telstra.Twins.Attributes;
using Telstra.Twins.Helpers;
using Telstra.Twins.Models;

namespace Telstra.Twins.Services
{
    public class TwinModelFactory
    {
        public string[] SpecialModelPropertyNames { get; } = { "@id", "@type", "extends", "@context", "displayName" };

        public TwinModel CreateTwinModel(Type dt)
        {
            var contents = new List<Content>();
            AddPropertiesContent(dt, contents);
            var relationships = AddModelRelationshipsContent(dt, contents);
            AddComponentsContent(dt, contents);
            AddTelemetryContent(dt, contents);

            return new TwinModel() { contents = contents, Relationships = relationships };
        }

        public TwinModel CreateTwinModel<T>()
        {
            return CreateTwinModel(typeof(T));
        }

        protected static void AddTelemetryContent(Type typeToAnalyze, List<Content> contents)
        {
            var modelProperties = typeToAnalyze.GetModelTelemetry()
                .ToList()
                .Select(ModelProperty.Create);
            contents.AddRange(modelProperties);
        }

        protected static void AddComponentsContent(Type typeToAnalyze, List<Content> contents)
        {
            var modelProperties = typeToAnalyze.GetModelComponents()
                .ToList()
                .Select(ModelComponent.Create);
            contents.AddRange(modelProperties);
        }

        protected static void AddPropertiesContent(Type typeToAnalyze, List<Content> contents)
        {
            var modelProperties = typeToAnalyze.GetModelProperties()
                .ToList()
                .Select(ModelProperty.Create);
            var parentProperties = typeToAnalyze.GetModelPropertiesFromAbstractParent()
                .ToList()
                .Select(ModelProperty.Create);

            contents.AddRange(parentProperties);
            contents.AddRange(modelProperties);
        }


        protected static Dictionary<PropertyInfo, ModelRelationship> AddModelRelationshipsContent(Type typeToAnalyze, List<Content> contents)
        {
            var modelProperties = typeToAnalyze.GetModelRelationships()
                .ToList();

            var parentProperties = typeToAnalyze.GetModelRelationshipsFromAbstractParent()
                .ToList();

            var dict = modelProperties
                .Union(parentProperties)
                .ToDictionary(r => r, r => ModelRelationship.Create(r));

            contents.AddRange(dict.Values);

            return dict;
        }

        protected static IEnumerable<PropertyInfo> GetModelOnlyProperty(Type typeToAnalyze, string[] modelOnlyPropertyNames)
        {
            var properties = typeToAnalyze
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(prop => prop.IsDefined(typeof(TwinModelOnlyPropertyAttribute)))
                .Where(prop => modelOnlyPropertyNames.Contains(prop.GetModelPropertyName()));
            return properties;
        }
    }
}
