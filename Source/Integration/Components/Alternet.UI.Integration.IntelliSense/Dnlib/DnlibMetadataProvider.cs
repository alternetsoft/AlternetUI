﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI.Integration.IntelliSense.AssemblyMetadata;
using dnlib.DotNet;

namespace Alternet.UI.Integration.IntelliSense.Dnlib
{
    public class DnlibMetadataProvider : IMetadataProvider
    {

        public IMetadataReaderSession GetMetadata(IEnumerable<string> paths)
        {
            return new DnlibMetadataProviderSession(paths.ToArray());
        }
    }

    class DnlibMetadataProviderSession : IMetadataReaderSession
    {
        public IEnumerable<IAssemblyInformation> Assemblies { get; }
        public DnlibMetadataProviderSession(string[] directoryPath)
        {
            Assemblies = LoadAssemblies(directoryPath).Select(a => new AssemblyWrapper(a)).ToList();
        }

        static List<AssemblyDef> LoadAssemblies(string[] lst)
        {
            AssemblyResolver asmResolver = new AssemblyResolver();
            ModuleContext modCtx = new ModuleContext(asmResolver);
            asmResolver.DefaultModuleContext = modCtx;
            asmResolver.EnableTypeDefCache = true;

            var assemblyNames = lst.Concat(new[] { new Uri(typeof(System.EventHandler).Assembly.CodeBase).LocalPath }).ToArray();

            foreach (var path in assemblyNames)
                asmResolver.PreSearchPaths.Add(path);

            List<AssemblyDef> assemblies = new List<AssemblyDef>();

            foreach (var asm in assemblyNames)
            {
                try
                {
                    var def = AssemblyDef.Load(File.ReadAllBytes(asm));
                    def.Modules[0].Context = modCtx;
                    asmResolver.AddToCache(def);
                    assemblies.Add(def);
                }
                catch
                {
                    //Ignore
                }
            }

            return assemblies;
        }

        public void Dispose()
        {
            //no-op
        }
    }
}
