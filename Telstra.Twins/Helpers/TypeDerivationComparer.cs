using System;
using System.Collections.Generic;

namespace Telstra.Twins.Helpers
{
    public class TypeDerivationComparer : IComparer<Type>
    {
        private static TypeDerivationComparer _instance;
        public static TypeDerivationComparer Instance => _instance == null ? _instance = new TypeDerivationComparer() : _instance;

        public int Compare(Type x, Type y)
        {
            if (requires(x, y))
                return 1;

            if (requires(y, x))
                return -1;

            return 0;
        }

        // 
        private static bool requires(Type x, Type y)
        {
            if (y.IsAssignableFrom(x))
                return true;

            var result = false;
            var props = x.GetModelRelationships();
            props.AddRange(x.GetModelComponents());
            props.ForEach(p =>
                {
                    if (!result)
                    {
                        var relationshipType = p.PropertyType.GetModelPropertyType();
                        if (relationshipType == y || relationshipType.IsSubclassOf(y))
                            result = true;
                        else
                        {
                            if (relationshipType != x)
                                result = requires(relationshipType, y);
                        }
                    }
                });

            return result;
        }

    }
}
