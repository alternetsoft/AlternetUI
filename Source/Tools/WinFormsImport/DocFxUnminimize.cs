using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Alternet.UI;

namespace WinFormsImport
{
    internal class DocFxUnminimize
    {
        DocFxSchema? schema;
        public string[]? splittedMappings;

        public class DocFxSchema
        {
            public int version { get; set; }
            public string[]? sources { get; set; }
            public string[]? sourcesContent { get; set; }
            public string? mappings { get; set; }
            public string[]? names { get; set; }

        }

        public DocFxUnminimize(string filename)
        {
            var jsonString = File.ReadAllText(filename, Encoding.UTF8);

            schema = JsonSerializer.Deserialize<DocFxSchema>(jsonString);
        }

        public string? GetMinName(string name)
        {
            if (schema is null || schema.names is null)
                return null;
            if (splittedMappings is null)
                splittedMappings = schema.mappings?.Split(',');
            for(int i = 0; i < schema.names.Length; i++)
            {
                if (schema.names[i] == name)
                {
                    return splittedMappings![i];
                }
            }

            return null;
        }

    }
}
