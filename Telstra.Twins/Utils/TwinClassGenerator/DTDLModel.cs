using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Telstra.Twins.Common;

namespace Telstra.Twins.Utils.TwinClassGenerator
{
    [Serializable]
    public class DTDLModel
    {
        [JsonProperty("@id")]
        public string ModelId { get; set; }

        [JsonIgnore]
        public string ClassName => ModelId.Split(';')[0]
            .Split(':')
            .Last()
            .ToCapitalCase();

        // Must be Interface
        [JsonProperty("@type")]
        public string ModelType { get; set; }

        // Must be DTDL V2
        [JsonProperty("@context")]
        public string Context { get; set; }

        public string DisplayName { get; set; }

        public string Extends { get; set; }

        [JsonIgnore]
        public string BaseClassName => Extends?.Split(';')[0]
            .Split(':')
            .Last()
            .ToCapitalCase() ?? "TwinBase";


        public List<ModelContent> Contents { get; set; } = new List<ModelContent>();

        public class ModelContent
        {
            [JsonProperty("@type")]
            public string ContentType { get; set; }
            public string Name { get; set; }
            public string Schema { get; set; }
            public string Target { get; set; }

            [JsonIgnore]
            public string TargetClassName => Target.Split(';')[0]
                .Split(':')
                .Last()
                .ToCapitalCase();
        }
    }
}
