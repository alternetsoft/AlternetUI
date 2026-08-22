using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Alternet.UI
{
    public class ResourceClassGenerator
    {
        public static string Generate(string xmlPath, string className = "Resources")
        {
            var doc = XDocument.Load(xmlPath);

            if (doc.Root == null || doc.Root.Name != "root")
            {
                throw new InvalidDataException("Invalid XML format. Root element 'root' not found.");
            }

            var properties = new List<string>();

            foreach (var data in doc.Root.Elements("data"))
            {
                var name = data.Attribute("name")?.Value;
                var value = data.Element("value")?.Value;

                if (!string.IsNullOrEmpty(name) && value != null)
                {
                    // Escape quotes inside value
                    var escapedValue = value.Replace("\"", "\\\"");
                    properties.Add(
                        $"    public string {name} {{ get; set; }} = \"{escapedValue}\";"
                    );
                }
            }

            return
    $@"public class {className}
{{
{string.Join(Environment.NewLine, properties)}
}}";
        }
    }

}