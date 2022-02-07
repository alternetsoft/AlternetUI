using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Alternet.UI;

namespace SRAdder
{
    class Engine
    {
        string[] sourceResxPaths;
        string targetResxPath;
        string targetIDsPath;

        public Engine(string[] sourceResxPaths, string targetResxPath, string targetIDsPath)
        {
            this.sourceResxPaths = sourceResxPaths;
            this.targetResxPath = targetResxPath;
            this.targetIDsPath = targetIDsPath;
        }

        record Entry(string Id, string Value, string? Comment);

        static Entry? TryGetEntry(string resxPath, string id)
        {
            var doc = XDocument.Load(resxPath);

            var dataElement = doc.Descendants("data").FirstOrDefault(x => x.Attribute("name")?.Value == id);
            if (dataElement == null)
                return null;

            var valueElement = dataElement.Descendants().SingleOrDefault(x => x.Name == "value");
            if (valueElement == null)
                return null;

            var value = valueElement.Value;
            if (string.IsNullOrWhiteSpace(value))
                return null;

            var commentElement = dataElement.Descendants().SingleOrDefault(x => x.Name == "comment");

            return new Entry(id, value, commentElement?.Value);
        }

        public Status GetStatus(string id)
        {
            var sourceEntry = TryGetSourceEntry(id);
            if (sourceEntry == null)
                return Status.NotFound;

            var targetEntry = TryGetEntry(targetResxPath, id);
            if (targetEntry != null)
                return Status.AlreadyAdded;

            return Status.ReadyToAdd;
        }

        private Entry? TryGetSourceEntry(string id) => sourceResxPaths.Select(x => TryGetEntry(x, id)).FirstOrDefault(x => x != null);

        public void Apply(string id)
        {
            var entry = TryGetSourceEntry(id);
            if (entry == null)
                throw new InvalidOperationException();

            AddResxEntry(targetResxPath, entry);
            AddIdEntry(targetIDsPath, entry);
        }

        private static void AddResxEntry(string resxPath, Entry entry)
        {
            var doc = XDocument.Load(resxPath);
            var root = doc.Descendants().Single(x => x.Name == "root");
            root.Add(new XElement(
                "data",
                new object[]
                {
                    new XAttribute("name", entry.Id),
                    new XAttribute(XNamespace.Xml + "space", "preserve"),
                    new XElement("value", entry.Value)
                }.Concat(entry.Comment == null ? Array.Empty<object>() : new object[] { new XElement("comment", entry.Comment) })));

            doc.Save(resxPath);
        }

        private static void AddIdEntry(string idsPath, Entry entry)
        {
            var lines = File.ReadAllLines(idsPath).ToList();

            int? insertionIndex = null;
            for (int i = lines.Count - 1; i >= 0; i--)
            {
                if (lines[i].TrimStart().StartsWith("internal const string"))
                {
                    insertionIndex = i + 1;
                    break;
                }
            }

            if (insertionIndex == null)
                throw new InvalidOperationException();

            lines.Insert(insertionIndex.Value, $"        /// <summary>{ entry.Comment ?? entry.Value }</summary>");
            lines.Insert(insertionIndex.Value + 1, $"        internal const string @{ entry.Id } = \"{ entry.Id }\";");

            File.WriteAllLines(idsPath, lines);
        }
    }
}
