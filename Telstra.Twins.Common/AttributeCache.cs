using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Telstra.Twins.Common
{
    public static class AttributeCache
    {
        public static T GetAttribute<T>(this Type type)
            where T : Attribute
        {
            return Cache
                .GetOrAdd(type, t => t.GetCustomAttributes(true).OfType<Attribute>().ToArray())
                .OfType<T>()
                .First();
        }

        public static bool TryGetAttribute<T>(this Type type, out T attribute)
            where T : Attribute
        {
            var attributes = Cache
                .GetOrAdd(type, t => t.GetCustomAttributes(true).OfType<Attribute>().ToArray()).OfType<T>();

            // ReSharper disable once PossibleMultipleEnumeration
            var attributeFound = attributes.Any();
            // ReSharper disable once PossibleMultipleEnumeration
            attribute = attributeFound ? attributes.First() : null;

            return attributeFound;
        }

        public static IEnumerable<T> GetAttributes<T>(this Type type)
            where T : Attribute => Cache
            .GetOrAdd(type, t => t.GetCustomAttributes(true).OfType<Attribute>().ToArray())
            .OfType<T>();

        static ConcurrentDictionary<Type, Attribute[]> Cache { get; } =
            new ConcurrentDictionary<Type, Attribute[]>();
    }
}