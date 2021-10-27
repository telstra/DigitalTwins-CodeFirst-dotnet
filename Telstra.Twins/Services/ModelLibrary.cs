using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Telstra.Twins.Attributes;
using Telstra.Twins.Examples.Twins;
using Telstra.Twins.Helpers;
using Telstra.Twins.Plugins;

namespace Telstra.Twins.Services
{
    public class ModelLibrary : IModelLibrary
    {
        private Dictionary<string, Type> _modelTypes;

        // NOTE: For now there can only be one example provider.
        //       The first provider discovered will be used.
        public IExampleProvider ExampleProvider { get; private set; }

//        private static PluginLoadContext _loadContext;

        private const string PLUGIN_PATH = "plugins";

        private string FullPath => $"{Environment.CurrentDirectory}\\{PLUGIN_PATH}";

        public ModelLibrary()
        {
//            _loadContext = new PluginLoadContext(FullPath);
            Init();
        }

        private void Init()
        {
            _modelTypes = new Dictionary<string, Type>();

            //Directory.GetFiles(FullPath)
            //    .ToList()
            //    .ForEach(p => {
            //        _loadContext.LoadFromAssemblyPath(p);
            //    });

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var types = assemblies.SelectMany(a => 
                                    a.GetTypes()
                                    .Where(t => Attribute.IsDefined(t, typeof(DigitalTwinAttribute))))
                                    .OrderBy(t => t, TypeDerivationComparer.Instance)
                                .ToList();

            types.ForEach(dt =>
            {
                if (!dt.IsInterface)
                {
                    var attr = dt.GetCustomAttribute<DigitalTwinAttribute>();
                    _modelTypes.Add(attr.GetFullModelId(dt), dt);
                }
            });

            var exampleProviderType = assemblies.Select(a => a.GetTypes()
                                                               .FirstOrDefault(ac => typeof(IExampleProvider).IsAssignableFrom(ac)))
                                                             .FirstOrDefault();
            if (exampleProviderType != null)
                ExampleProvider = (IExampleProvider)Activator.CreateInstance(exampleProviderType);
        }

        public List<Type> All => _modelTypes.Values.ToList();

        public Type GetById(string modelId)
        {
            if (!_modelTypes.ContainsKey(modelId))
                return null;

            return _modelTypes[modelId];
        }

        public List<Type> GetDerivedTypes(Type t) => 
            _modelTypes.Values.Where(mt => t.GetModelPropertyType().IsAssignableFrom(mt) && mt != t.GetModelPropertyType())
            .ToList();

        // returns types that define this type in a relationship or component
        // The list is a parameter as the function is called recursively
        private List<Type> TraverseRelationships(Type modelType, List<Type> list = null)
        {
            if (list == null)
                list = new List<Type>();
            modelType = modelType.GetModelPropertyType();

            var props = modelType.GetProperties().Where(p =>
                p.DeclaringType == modelType && (Attribute.IsDefined(p, typeof(TwinRelationshipAttribute)) ||
                                                 Attribute.IsDefined(p, typeof(TwinComponentAttribute))))
                .Select(p => p.PropertyType.GetModelPropertyType())
                .ToList();

            props.ForEach(t => {
                if (t != modelType)
                {
                    TraverseRelationships(t, list);
                    list.Add(t);
                }
            });

            return list;
        }

        public List<Type> GetRelatedTypes(Type t) =>
            _modelTypes.Values.Where(mt => TraverseRelationships(mt).Contains(t)).ToList();

        public List<Type> GetDependendentTypes(Type t)
        {
            var result = GetDerivedTypes(t);
            result.AddRange(GetRelatedTypes(t));

            return result;
        }

        public Type GetTypeFromJson(string json)
        {
            var modelId =  JsonHelpers.GetModelId(json);

            if (!_modelTypes.ContainsKey(modelId))
                return null;

            return _modelTypes[modelId];
        }

   }
}
