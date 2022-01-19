using System;
using System.Collections.Generic;
using Telstra.Twins.Examples.Twins;

namespace Telstra.Twins.Services
{
    public interface IModelLibrary
    {
        Type GetById(string modelId);
        List<Type> GetDerivedTypes(Type t);
        List<Type> GetRelatedTypes(Type t);
        // Aggregates the results of GetDerivedTypes and GetRelatedTypes
        List<Type> GetDependendentTypes(Type t);
        Type GetTypeFromJson(string json);
        TwinModel GetTwinModel(Type type);

        List<Type> All { get; }

        public IExampleProvider ExampleProvider { get; }
    }
}
