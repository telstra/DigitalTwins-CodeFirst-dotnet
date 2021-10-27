using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Telstra.Twins.Serialization
{
    public static class PropertyInfoSetExtensions
    {
#pragma warning disable 8632
        public static Dictionary<string, object?> ToNameValueDictionary<T>(
#pragma warning restore 8632
            this IEnumerable<PropertyInfo> propertyInfoSet, T instance, Func<PropertyInfo, string> getNameFunc)
        {
            var nameValueDictionary = propertyInfoSet
                .ToDictionary(
                    getNameFunc,
                    prop => prop.GetValue(instance));

            return nameValueDictionary;
        }
    }
}
