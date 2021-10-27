using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using Telstra.Twins.Attributes;

namespace Telstra.Twins.Serialization
{
    public abstract class TwinConverterBase<T> : JsonConverter<T>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            // The type we're converting must match the type of T
            var typesMatch = typeToConvert == typeof(T);

            // The type we're converting has to have a [DigitalTwin] attribute
            var hasTwinAttribute = Attribute.IsDefined(typeToConvert, typeof(DigitalTwinAttribute));

            return typesMatch && hasTwinAttribute;
        }

        protected static IEnumerable<PropertyInfo> GetTwinProperties<TA>(string[] propertiesToExclude, BindingFlags bindingFlags, Func<PropertyInfo, string> getNameFunc) where TA : Attribute
        {
            var typeToAnalyze = typeof(T);
            var properties = typeToAnalyze
                .GetProperties(bindingFlags)
                .Where(prop => prop.IsDefined(typeof(TA)))
                .Where(prop => !propertiesToExclude.Contains(getNameFunc(prop)));
            return properties;
        }

        protected static IEnumerable<PropertyInfo> GetTwinProperties<TA>(BindingFlags bindingFlags) where TA : Attribute
        {
            var typeToAnalyze = typeof(T);
            var properties = typeToAnalyze
                .GetProperties(bindingFlags)
                .Where(prop => prop.IsDefined(typeof(TA)));
            return properties;
        }
    }
}
