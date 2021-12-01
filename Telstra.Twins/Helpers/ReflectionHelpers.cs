using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using Telstra.Twins.Attributes;
using Telstra.Twins.Common;

namespace Telstra.Twins.Helpers
{
    public static class ReflectionHelpers
    {
        public static List<PropertyInfo> GetTwinProperties(this Type t) =>
            t.GetProperties()
            .Where(p => Attribute.IsDefined(p, typeof(TwinPropertyAttribute))
                     || Attribute.IsDefined(p, typeof(TwinComponentAttribute)))
            .ToList();

        /// <summary>
        /// Dependencies for a given type are:
        ///   ModelType derivations (base types)
        ///   Component types (and their derivations)
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static List<Type> GetModelDependencies(this Type t)
        {
            var deps = t.GetInheritance();

            // get the component types
            deps.AddRange(
                t.GetModelComponents().Select(m => m.PropertyType.GetModelPropertyType()));

            //if (deps.Any())
            //{
            var moreDeps = deps.SelectMany(d => d.GetModelDependencies()).ToList();
            deps.AddRange(moreDeps);
            //            }

            return deps
                .Distinct()
                .OrderBy(t => t, TypeDerivationComparer.Instance)
                .ToList();
        }

        public static List<Type> GetInheritance(this Type t, List<Type> types = null)
        {
            if (types == null)
                types = new List<Type>();

            if (t.BaseType != typeof(TwinBase))
            {
                types.AddRange(GetInheritance(t.BaseType, types));

                types.Add(t.BaseType);
            }

            return types;
        }


        public static List<PropertyInfo> GetModelComponents(this Type t) =>
            t.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.DeclaringType == t)
            .Where(p => Attribute.IsDefined(p, typeof(TwinComponentAttribute)))
            .ToList();

        public static List<PropertyInfo> GetModelProperties(this Type t) =>
            t.GetProperties()
            .Where(p => p.DeclaringType == t)
            .Where(p => Attribute.IsDefined(p, typeof(TwinPropertyAttribute)))
            .ToList();

        public static List<PropertyInfo> GetModelPropertiesFromAbstractParent(this Type t) =>
            t.BaseType.IsAbstract && t.BaseType.Name != typeof(TwinBase).Name && !Attribute.IsDefined(t.BaseType.GetModelPropertyType(), typeof(DigitalTwinAttribute))
                ? t.BaseType.GetProperties()
                    .Where(p => p.DeclaringType == t.BaseType)
                    .Where(p => Attribute.IsDefined(p, typeof(TwinPropertyAttribute)))
                    .ToList()
                : new();

        public static List<PropertyInfo> GetModelOnlyProperties(this Type t) =>
            t.GetProperties()
                .Where(p => p.DeclaringType == t)
                .Where(p => Attribute.IsDefined(p, typeof(TwinModelOnlyPropertyAttribute)))
                .ToList();

        public static List<PropertyInfo> GetModelTelemetry(this Type t) =>
            t.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.DeclaringType == t)
            .Where(p => Attribute.IsDefined(p, typeof(TwinTelemetryAttribute)))
            .ToList();

        public static List<PropertyInfo> GetModelRelationships(this Type t) =>
            t.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.DeclaringType == t)
            .Where(p => Attribute.IsDefined(p, typeof(TwinRelationshipAttribute)))
            .ToList();

        public static List<PropertyInfo> GetModelRelationshipsFromAbstractParent(this Type t) =>
           t.BaseType.IsAbstract && t.BaseType.Name != typeof(TwinBase).Name && !Attribute.IsDefined(t.BaseType.GetModelPropertyType(), typeof(DigitalTwinAttribute))
               ? t.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.DeclaringType == t.BaseType)
                    .Where(p => Attribute.IsDefined(p, typeof(TwinRelationshipAttribute)))
                    .ToList()
               : new();

        public static List<PropertyInfo> GetTwinRelationships(this Type t) =>
            t.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
            .Where(p => Attribute.IsDefined(p, typeof(TwinRelationshipAttribute)))
            .ToList();

        public static string GetDigitalTwinModelId(this Type t)
        {
            var modelType = t.GetModelPropertyType();

            if (Attribute.IsDefined(modelType, typeof(DigitalTwinAttribute)))
            {
                // ReSharper disable once PossibleNullReferenceException
                return modelType.GetCustomAttribute<DigitalTwinAttribute>().GetFullModelId(modelType);
            }

            return null;
        }

        public static string GetDigitalTwinModelExtends(this Type t)
        {
            var modelType = t.GetModelPropertyType();

            if (Attribute.IsDefined(modelType, typeof(DigitalTwinAttribute)))
            {
                // ReSharper disable once PossibleNullReferenceException
                return modelType.GetCustomAttribute<DigitalTwinAttribute>().ExtendsModelId;
            }

            return null;
        }

        public static Type GetModelPropertyType(this Type t)
        {
            if (t.IsGenericType)
                return t.GetGenericArguments()[0];

            var nullableType = Nullable.GetUnderlyingType(t);
            if (nullableType != null)
                return nullableType;

            return t;
        }

        public static string GetTwinPropertyName(this PropertyInfo p)
        {
            var twinPropertyName = p.Name.ToCamelCase();

            if (Attribute.IsDefined(p, typeof(TwinOnlyPropertyAttribute)))
            {
                var customAttribute = p.GetCustomAttribute<TwinOnlyPropertyAttribute>();
                twinPropertyName = customAttribute?.Name;
            }
            else if (Attribute.IsDefined(p, typeof(TwinPropertyAttribute)))
            {
                var customAttribute = p.GetCustomAttribute<TwinPropertyAttribute>();
                twinPropertyName = customAttribute?.Name;
            }
            else if (Attribute.IsDefined(p, typeof(JsonPropertyNameAttribute)))
            {
                var customAttribute = p.GetCustomAttribute<JsonPropertyNameAttribute>();
                twinPropertyName = customAttribute?.Name;
            }
            else if (Attribute.IsDefined(p, typeof(JsonPropertyAttribute)))
            {
                var customAttribute = p.GetCustomAttribute<JsonPropertyAttribute>();
                twinPropertyName = customAttribute?.PropertyName;
            }

            return twinPropertyName ?? p.Name.ToCamelCase();
        }

        public static string GetModelPropertyName(this PropertyInfo p)
        {
            var modelPropertyName = p.Name.ToCamelCase();

            if (Attribute.IsDefined(p, typeof(TwinModelOnlyPropertyAttribute)))
            {
                var modelOnlyPropertyAttribute = p.GetCustomAttribute<TwinModelOnlyPropertyAttribute>();
                modelPropertyName = modelOnlyPropertyAttribute?.Name;
            }
            else if (Attribute.IsDefined(p, typeof(TwinPropertyAttribute)))
            {
                var modelOnlyPropertyAttribute = p.GetCustomAttribute<JsonPropertyAttribute>();
                modelPropertyName = modelOnlyPropertyAttribute?.PropertyName;
            }

            return modelPropertyName;
        }

        public static string GetJsonPropertyName(this PropertyInfo p)
        {
            var jsonPropertyName = p.Name.ToCamelCase();

            if (Attribute.IsDefined(p, typeof(JsonPropertyNameAttribute)))
            {
                var jsonNameAttribute = p.GetCustomAttribute<JsonPropertyNameAttribute>();
                jsonPropertyName = jsonNameAttribute?.Name;
            }
            else if (Attribute.IsDefined(p, typeof(JsonPropertyAttribute)))
            {
                var jsonNameAttribute = p.GetCustomAttribute<JsonPropertyAttribute>();
                jsonPropertyName = jsonNameAttribute?.PropertyName;
            }

            return jsonPropertyName;
        }

        // returns the value as is, EXCEPT if the value is an integer
        // in which case it is downgraded as far as possible
        // this is because the boxing/unboxing process converts all integers to Int64
        public static object GetValue(this Dictionary<string, object> values, string key)
        {
            if (values[key] is Int64)
            {
                var longVal = Convert.ToInt64(values[key]);
                if (longVal <= Byte.MaxValue && longVal >= Byte.MinValue)
                    return Convert.ToByte(values[key]);
                else if (longVal >= Int16.MinValue && longVal <= Int16.MaxValue)
                    return Convert.ToInt16(values[key]);
                else if (longVal >= Int32.MinValue && longVal <= Int32.MaxValue)
                    return Convert.ToInt32(values[key]);
                else
                    return longVal;
            }

            return values[key];
        }
    }
}
