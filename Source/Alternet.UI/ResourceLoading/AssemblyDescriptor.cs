using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class AssemblyDescriptor : IAssemblyDescriptor
    {
        public AssemblyDescriptor(Assembly assembly)
        {
            Assembly = assembly;

            if (assembly != null)
            {
                Resources = assembly.GetManifestResourceNames().ToDictionary(
                    n => n,
                    n => (IAssetDescriptor)new AssemblyResourceDescriptor(assembly, n));
                Name = assembly.GetName().Name;

                using var resources = assembly.GetManifestResourceStream(
                    ResourceConsts.UIResourceName);

                if (resources != null)
                {
                    Resources.Remove(ResourceConsts.UIResourceName);

                    var indexLength = new BinaryReader(resources).ReadInt32();
                    var index = ResourcesIndexReaderWriter.Read(
                        new SlicedStream(resources, 4, indexLength));
                    var baseOffset = indexLength + 4;
                    UIResources = index.ToDictionary(
                        r => GetPathRooted(r),
                        r => (IAssetDescriptor)new UIResourceDescriptor(
                            assembly,
                            baseOffset + r.Offset,
                            r.Size));
                }
            }
        }

        public Assembly Assembly { get; }

        public Dictionary<string, IAssetDescriptor>? Resources { get; }

        public Dictionary<string, IAssetDescriptor>? UIResources { get; }

        public string? Name { get; }

        private static string GetPathRooted(UIResourcesIndexEntry r) =>
            r.Path![0] == '/' ? r.Path : '/' + r.Path;
    }
}