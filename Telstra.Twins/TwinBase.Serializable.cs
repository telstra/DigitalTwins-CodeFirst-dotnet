using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json.Serialization;
using Azure.DigitalTwins.Core;
using Telstra.Twins.Attributes;
using Telstra.Twins.Helpers;
using Telstra.Twins.Common;
using Telstra.Twins.Models;

namespace Telstra.Twins
{
    public abstract partial class TwinBase
    {
        private string _modelId;

        protected TwinBase()
        {
            // Read any information provided using the DigitalTwinAttribute.
            // This is results in the attributes for the type being cached.
            this.ReadAttributeInfo();
        }

        /// <summary>
        /// Compiles a flat list of a twin and its related twins
        /// Conveniently (not by design) the twins are returned in order of dependency
        /// </summary>
        /// <returns></returns>
        public List<TwinBase> Flatten()
        {
            List<TwinBase> list = new List<TwinBase> { this };
            list.AddRange(TraverseTwin(this));
            return list.Distinct().ToList();
        }

        public List<Type> GetDependentTypes()
        {
            var modelTypes = this.Flatten()
                .Select(t => t.GetType())
                .ToList();

            var derivedTypes = new List<Type>();
            modelTypes.ForEach(m =>
            {
                derivedTypes.AddRange(GetInheritance(m));
                derivedTypes.AddRange(m.GetModelComponents().Select(c => c.PropertyType.GetModelPropertyType()));
            });

            modelTypes.AddRange(derivedTypes);
            modelTypes = modelTypes.Distinct()
                .OrderBy(t => t, TypeDerivationComparer.Instance)
                .ToList();

            return modelTypes;
        }

        private List<Type> GetInheritance([NotNull] Type t)
        {
            var types = new List<Type>();

            if (t.BaseType != null && t.BaseType != typeof(TwinBase))
            {
                types.AddRange(GetInheritance(t.BaseType, types));
                types.Add(t.BaseType);
            }

            return types;
        }

        private List<Type> GetInheritance([NotNull] Type t, [NotNull] List<Type> types)
        {
            if (t.BaseType != null && t.BaseType != typeof(TwinBase))
            {
                types.AddRange(GetInheritance(t.BaseType, types));
                types.Add(t.BaseType);
            }

            return types;
        }

        public List<BasicRelationship> GetRelationships()
        {
            var relationships = this.GetType().GetTwinRelationships();
            var result = new List<BasicRelationship>();

            relationships.ForEach(r =>
            {
                var prop = r.GetValue(this);
                var relationshipType = r.Name.ToCamelCase();
                if (prop is IEnumerable<TwinBase> twinBaseObjects)
                {
                    twinBaseObjects.ToList().ForEach(r =>
                    {
                        result.Add(new BasicRelationship { SourceId = this.TwinId, TargetId = r.TwinId, Name = relationshipType });
                    });
                }
                else if (prop is TwinBase twinProp)
                {
                    result.Add(new BasicRelationship { SourceId = this.TwinId, TargetId = twinProp.TwinId, Name = relationshipType });
                }
            });
            return result;
        }

        private List<TwinBase> TraverseTwin(TwinBase twin)
        {
            var list = new List<TwinBase>();
            twin.GetType()
                .GetTwinRelationships()
                .ForEach(p =>
                {
                    var prop = p.GetValue(twin);
                    if (prop is IEnumerable<TwinBase>)
                    {
                        var relationships = prop as IEnumerable<TwinBase>;
                        relationships.ToList().ForEach(r =>
                        {
                            list.Add(r);
                            list.AddRange(TraverseTwin(r));
                        });
                    }
                    else
                    {
                        if (prop is TwinBase twinProp)
                        {
                            list.Add(twinProp);
                            list.AddRange(TraverseTwin(twinProp));
                        }
                    }
                });

            return list;
        }

        [TwinModelOnlyProperty("@id")]
        public string ModelId
        {
            get => _modelId;
            set
            {
                _modelId = value;
                var twinMetadata = this.Metadata;
                if (twinMetadata != null)
                {
                    twinMetadata.ModelId = ModelId;
                }
            }
        }

        [TwinModelOnlyProperty("@type")]
        public string ModelType { get; set; } = "Interface";

        [TwinModelOnlyProperty("extends")]
        public string ExtendsModelId { get; set; }

        [TwinModelOnlyProperty("@context")]
        public string Context { get; set; } = "dtmi:dtdl:context;2";

        [TwinModelOnlyProperty("displayName")]
        protected string DisplayName { get; set; }

        [TwinOnlyProperty(DigitalTwinsJsonPropertyNames.DigitalTwinId)]
        [JsonPropertyName(DigitalTwinsJsonPropertyNames.DigitalTwinId)]
        public string TwinId { get; set; }

        /// <summary>
        /// A string representing a weak ETag for the entity that this request performs an operation against, as per RFC7232.
        /// </summary>
        [TwinOnlyProperty(DigitalTwinsJsonPropertyNames.DigitalTwinETag)]
        [JsonPropertyName(DigitalTwinsJsonPropertyNames.DigitalTwinETag)]
        public string ETag { get; set; }

        [TwinOnlyProperty(DigitalTwinsJsonPropertyNames.DigitalTwinMetadata)]
        [JsonPropertyName(DigitalTwinsJsonPropertyNames.DigitalTwinMetadata)]
        public virtual TwinMetadata Metadata { get; set; } = new TwinMetadata();

        private void ReadAttributeInfo()
        {
            var type = this.GetType();
            if (type.TryGetAttribute<DigitalTwinAttribute>(out var twinAttribute))
            {
                DisplayName = twinAttribute.DisplayName;
                ModelId = twinAttribute.GetFullModelId(type);
                ModelType = twinAttribute.ModelType;

                // Get the model Id that this model is extending.
                if (twinAttribute.ExtendsModelId != null)
                {
                    ExtendsModelId = twinAttribute.ExtendsModelId;
                }
                else if (type.BaseType != null &&
                         type.BaseType.TryGetAttribute<DigitalTwinAttribute>(out var baseTwinAttribute))
                {
                    ExtendsModelId = baseTwinAttribute.GetFullModelId(type.BaseType);
                }
            }
        }

        public BasicDigitalTwin ToBasicTwin()
        {
            var properties = this.GetType().GetTwinProperties();

            var basicTwin = new BasicDigitalTwin()
            {
                Id = this.TwinId,
                Metadata = new DigitalTwinMetadata()
                {
                    ModelId = ModelId,
                    PropertyMetadata = properties.Select(p => (key: p.Name, value: new DigitalTwinPropertyMetadata() { LastUpdatedOn = DateTimeOffset.UtcNow }))
                        .ToDictionary(c => c.key, c => c.value)
                },
                Contents = properties.Select(p => (key: p.Name.ToCamelCase(), value: p.GetValue(this)))
                    .ToDictionary(c => c.key,
                        c => c.value is TwinBase twinBase ?
                            twinBase.ToTwinComponent() : c.value)
            };

            return basicTwin;
        }

        public BasicDigitalTwinComponent ToTwinComponent()
        {
            var properties = this.GetType().GetTwinProperties();

            var basicTwinComponent = new BasicDigitalTwinComponent()
            {
                Metadata = properties.Select(p => (key: p.Name, value: new DigitalTwinPropertyMetadata() { LastUpdatedOn = DateTimeOffset.UtcNow }))
                        .ToDictionary(c => c.key, c => c.value),
                Contents = properties.Select(p => (key: p.Name.ToCamelCase(), value: p.GetValue(this)))
                    .ToDictionary(c => c.key,
                        c => c.value is TwinBase twinBase ?
                            twinBase.ToTwinComponent() : c.value)
            };

            return basicTwinComponent;
        }
    }
}
