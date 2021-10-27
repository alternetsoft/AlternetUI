using System;
using System.Linq;
using System.Xml.Linq;

namespace Alternet.UI.PublicSourceGenerator.Generators
{
    class CsprojFile
    {
        readonly string fileName;
        readonly XDocument document;

        public CsprojFile(string fileName)
        {
            document = XDocument.Load(fileName);
            this.fileName = fileName;
        }

        public void AddPackageReference(string name, string version, string? privateAssets = null)
        {
            var reference = new XElement(
                        "PackageReference",
                        new XAttribute("Include", name),
                        new XAttribute("Version", version));

            if (privateAssets != null)
                reference.Add(new XElement("PrivateAssets", new XText(privateAssets)));
            
            document.Root.Add(new XElement("ItemGroup", reference));
        }

        public void RemovePackageReferenceCondition(string name)
        {
            foreach (var group in document.Descendants("ItemGroup"))
            {
                var condition = group.Attribute("Condition");
                if (condition == null)
                    continue;

                foreach (var reference in group.Descendants("PackageReference"))
                {
                    if (reference.Attribute("Include")?.Value != name &&
                        reference.Descendants("Include").SingleOrDefault()?.Value != name)
                        continue;

                    condition.Remove();
                    return;
                }
            }
        }

        public void RemoveProjectReference(string projectName)
        {
            XElement? groupContainingFoundElement = null;
            
            foreach (var group in document.Descendants("ItemGroup"))
            {
                var projectReference = group.Descendants("ProjectReference").Where(
                    x => x.Attribute("Include")?.Value.EndsWith(projectName) ?? false).SingleOrDefault();
                if (projectReference != null)
                {
                    groupContainingFoundElement = group;
                    projectReference.Remove();
                    break;
                }
            }

            if (groupContainingFoundElement == null)
                throw new InvalidOperationException();

            if (!groupContainingFoundElement.Elements().Any())
                groupContainingFoundElement.Remove();
        }

        public void SetPackageReferenceVersion(string name, string version)
        {
            foreach (var group in document.Descendants("ItemGroup"))
            {
                foreach (var reference in group.Descendants("PackageReference"))
                {
                    if (reference.Attribute("Include")?.Value != name &&
                        reference.Descendants("Include").SingleOrDefault()?.Value != name)
                        continue;

                    var versionAttribute = reference.Attribute("Version");
                    if (versionAttribute != null)
                    {
                        versionAttribute.Value = version;
                        return;
                    }

                    var versionDescendant = reference.Descendants("Version").SingleOrDefault();
                    if (versionDescendant != null)
                    {
                        versionDescendant.Value = version;
                        return;
                    }
                }
            }
        }

        public void Save()
        {
            document.Save(fileName);
        }
    }
}