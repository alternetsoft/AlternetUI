using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Linq;
using Alternet.Drawing;

namespace Alternet.UI
{
#pragma warning disable
    internal static class ResourcesIndexReaderWriter
#pragma warning restore
    {
        private const int LastKnownVersion = 1;

        public static List<UIResourcesIndexEntry> Read(Stream stream)
        {
            var ver = new BinaryReader(stream).ReadInt32();
            if (ver > LastKnownVersion)
                throw new Exception("Resources index format version is not known");

            var assetDoc = XDocument.Load(stream);
            XNamespace assetNs = assetDoc.Root!.Attribute("xmlns")!.Value;
            List<UIResourcesIndexEntry> entries =
                (from entry in assetDoc.Root.Element(assetNs + "Entries")!.Elements(assetNs + "UIResourcesIndexEntry")
                 select new UIResourcesIndexEntry
                 {
                     Path = entry.Element(assetNs + "Path")!.Value,
                     Offset = int.Parse(entry.Element(assetNs + "Offset")!.Value),
                     Size = int.Parse(entry.Element(assetNs + "Size")!.Value),
                 }).ToList();

            return entries;
        }

        public static void Write(Stream stream, List<UIResourcesIndexEntry> entries)
        {
            new BinaryWriter(stream).Write(LastKnownVersion);
            new DataContractSerializer(typeof(UIResourcesIndex)).WriteObject(
                stream,
                new UIResourcesIndex()
                {
                    Entries = entries,
                });
        }

        public static byte[] Create(Dictionary<string, byte[]> data)
        {
            var sources = data.ToList();
            var offsets = new Dictionary<string, int>();
            var coffset = 0;
            foreach (var s in sources)
            {
                offsets[s.Key] = coffset;
                coffset += s.Value.Length;
            }

            var index = sources.Select(s => new UIResourcesIndexEntry
            {
                Path = s.Key,
                Size = s.Value.Length,
                Offset = offsets[s.Key],
            }).ToList();
            var output = new MemoryStream();
            var ms = new MemoryStream();
            ResourcesIndexReaderWriter.Write(ms, index);
            new BinaryWriter(output).Write((int)ms.Length);
            ms.Position = 0;
            ms.CopyTo(output);
            foreach (var s in sources)
            {
                output.Write(s.Value, 0, s.Value.Length);
            }

            return output.ToArray();
        }
    }

    [DataContract]
#pragma warning disable
    internal class UIResourcesIndex
#pragma warning restore
    {
        [DataMember]
        public List<UIResourcesIndexEntry> Entries { get; set; } = new();
    }

    [DataContract]
#pragma warning disable
    internal class UIResourcesIndexEntry
#pragma warning restore
    {
        [DataMember]
        public string? Path { get; set; }

        [DataMember]
        public int Offset { get; set; }

        [DataMember]
        public int Size { get; set; }
    }
}