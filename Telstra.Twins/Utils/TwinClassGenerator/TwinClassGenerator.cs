using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using Telstra.Twins.Common;

namespace Telstra.Twins.Utils.TwinClassGenerator
{
    public class TwinClassGenerator : ITwinClassGenerator
    {
        private Dictionary<string, string> TypeMap = new Dictionary<string, string>()
        {
            {"string", "string" },
            { "integer", "int" },
            { "boolean", "bool" }
        };

        public string FromDTDL(string dtdl) => FromModel(JsonConvert.DeserializeObject<DTDLModel>(dtdl));

        public string FromModel(DTDLModel model)
        {
            var ass = Assembly.GetExecutingAssembly();
            var names = ass.GetManifestResourceNames();
            var path = names.FirstOrDefault(n => n.Contains("SampleTwin"));

            using (var reader = new StreamReader(ass.GetManifestResourceStream(path)))
            {
                var twinClass = reader.ReadToEnd();

                twinClass = twinClass.Replace(ModelTemplateTags.DisplayName, model.DisplayName);

                twinClass = twinClass.Replace(ModelTemplateTags.BaseClass, model.BaseClassName);

                var idParts = model.ModelId.Split(';');
                twinClass = twinClass.Replace(ModelTemplateTags.Version, idParts[1]);

                var nameParts = idParts[0].Split(':').ToList();
                nameParts.RemoveAt(0);  // throw away the dtmi

                var name = nameParts[nameParts.Count() - 1].ToCapitalCase();
                nameParts.RemoveAt(nameParts.Count() - 1);
                while (twinClass.Contains(ModelTemplateTags.Name))
                    twinClass = twinClass.Replace(ModelTemplateTags.Name, name);

                var ns = string.Join('.', nameParts.Select(n => n.ToCapitalCase()));
                twinClass = twinClass.Replace(ModelTemplateTags.Namespace, ns);


                var spacesRegex = new Regex(@"( +)\$content\$");
                var match = spacesRegex.Match(twinClass);
                var spaces = match.Groups[1].Value;

                var bob = new StringBuilder();
                model.Contents.ForEach(c =>
                {
                    switch (c.ContentType)
                    {
                        case "Property":
                            bob.AppendLine($"{spaces}[TwinProperty]");
                            bob.AppendLine($"{spaces}public {TypeMap[c.Schema]} {c.Name.ToCapitalCase()} {{ get; set; }}");
                            break;

                        case "Telemetry":
                            bob.AppendLine($"{spaces}[Telemetry]");
                            bob.AppendLine($"{spaces}public Telemetry<{TypeMap[c.Schema]}> {c.Name.ToCapitalCase()} {{ get; set; }}");
                            break;

                        case "Relationship":
                            bob.AppendLine($"{spaces}[ModelRelationship]");
                            if (c.Name.EndsWith('s') && !c.Name.EndsWith("us"))
                                bob.AppendLine($"{spaces}public List<{c.TargetClassName}> {c.Name.ToCapitalCase()} {{ get; set; }}  = new List<{c.TargetClassName}>();");
                            else
                                bob.AppendLine($"{spaces}public {c.TargetClassName} {c.Name.ToCapitalCase()} {{ get; set; }}");
                            break;
                    }
                });

                twinClass = twinClass.Replace($"{spaces}{ModelTemplateTags.Content}", bob.ToString());

                return twinClass;
            }
        }
    }
}
