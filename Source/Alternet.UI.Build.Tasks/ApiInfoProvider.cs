using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Alternet.UI.Build.Tasks
{
    internal class ApiInfoProvider
    {
        private ApiInfo apiInfo;

        public ApiInfoProvider(Stream dataStream)
        {
            apiInfo = Loader.Load(dataStream);
        }

        public bool IsEvent(string assemblyName, string classFullName, string memberName)
        {
            return apiInfo.TryGetAssembly(assemblyName)?.TryGetType(classFullName)?.HasEvent(memberName) ?? false;
        }

        private static class Loader
        {
            public static ApiInfo Load(Stream dataStream)
            {
                var document = XDocument.Load(dataStream);
                return new ApiInfo(
                    document.Descendants("Assembly").Select(
                        x => new AssemblyInfo(
                            x.Attribute("Name").Value,
                            document.Descendants("Type").Select(
                                x => new TypeInfo(
                                    x.Attribute("Name").Value,
                                    x.Descendants("Event").Select(x => x.Attribute("Name").Value))))));
            }
        }

        private class TypeInfo
        {
            private HashSet<string> events;

            public TypeInfo(string fullName, IEnumerable<string> events)
            {
                FullName = fullName;
                this.events = new HashSet<string>(events);
            }

            public string FullName { get; }

            public bool HasEvent(string name) => events.Contains(name);
        }

        private class AssemblyInfo
        {
            private Dictionary<string, TypeInfo> types;

            public AssemblyInfo(string name, IEnumerable<TypeInfo> types)
            {
                Name = new AssemblyName(name);
                this.types = types.ToDictionary(x => x.FullName);
            }

            public AssemblyName Name { get; }

            public TypeInfo? TryGetType(string name)
            {
                if (types.TryGetValue(name, out var result))
                    return result;
                return null;
            }
        }

        private class ApiInfo
        {
            private Dictionary<string, AssemblyInfo> assemblies;

            public ApiInfo(IEnumerable<AssemblyInfo> assemblies)
            {
                this.assemblies = assemblies.ToDictionary(x => x.Name.Name ?? throw new Exception());
            }

            public AssemblyInfo? TryGetAssembly(string name)
            {
                if (assemblies.TryGetValue(name, out var result))
                    return result;
                return null;
            }
        }
    }
}