using System;
using System.Reflection;
using System.Xml.Linq;

namespace Alternet.UI.Build.Tasks.ApiInfoCollector
{
    internal static class ApiInfoGenerator
    {
        public static XDocument Generate(Assembly assembly)
        {
            var document = new XDocument();
            var root = new XElement("ApiInfo");
            GenerateAssembly(assembly, root);
            document.Add(root);
            return document;
        }

        private static void GenerateAssembly(Assembly assembly, XElement parentElement)
        {
            var assemblyElement = new XElement("Assembly");
            assemblyElement.Add(new XAttribute("Name", assembly.FullName));
            foreach (var type in assembly.GetExportedTypes())
                GenerateType(type, assemblyElement);

            parentElement.Add(assemblyElement);
        }

        private static void GenerateType(Type type, XElement parentElement)
        {
            var typeElement = new XElement("Type");
            typeElement.Add(new XAttribute("Name", type.FullName));
            foreach (var @event in type.GetEvents(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
                GenerateEvent(@event, typeElement);

            parentElement.Add(typeElement);
        }

        private static void GenerateEvent(EventInfo @event, XElement parentElement)
        {
            var eventElement = new XElement("Event");
            eventElement.Add(new XAttribute("Name", @event.Name));
            parentElement.Add(eventElement);
        }
    }
}