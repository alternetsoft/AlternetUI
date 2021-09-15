using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Alternet.UI.Integration.IntelliSense.AssemblyMetadata
{
    public class MetadataReader
    {
        private readonly IMetadataProvider _provider;

        public MetadataReader(IMetadataProvider provider)
        {
            _provider = provider;
        }



        IEnumerable<string> GetAssemblies(string path)
        {
            var depsPath = Path.Combine(Path.GetDirectoryName(path),
                Path.GetFileNameWithoutExtension(path) + ".deps.json");
            if (File.Exists(depsPath))
                return DepsJsonAssemblyListLoader.ParseFile(depsPath);
            return Directory.GetFiles(Path.GetDirectoryName(path)).Where(f => f.EndsWith(".dll") || f.EndsWith(".exe"));
        }

        public Metadata GetForTargetAssembly(string path)
        {
            IEnumerable<string> assemblies;
            if (File.Exists(path))
                assemblies = GetAssemblies(path);
            else
                assemblies = DefaultAssembliesLocator.GetAssemblies();

            using (var session = _provider.GetMetadata(assemblies))
                return MetadataConverter.ConvertMetadata(session);
        }
        
    }
}
