﻿using System;
using System.Collections.Generic;

namespace Alternet.UI.Versioning
{
    public static class BuildNumberSetter
    {
        public static void SetBuildNumber(Repository repository, int buildNumber)
        {
            MasterVersionBuildNumberSetter.SetBuildNumber(repository, buildNumber);
            PatchVSPackageManifests(buildNumber, new FileLocator(repository));
        }

        private static void PatchVSPackageManifests(int buildNumber, FileLocator locator)
        {
            var patcher = new XmlPatcher(omitXmlDeclaration: false);
            foreach (var file in locator.GetVSPackageManifestFiles())
            {
                const string Selector = "/x:PackageManifest/x:Metadata/x:Identity";
                var namespaceBindings = new Dictionary<string, string> { { "x", XmlNamespaces.VSManifest.NamespaceName } };
                const string AttributeName = "Version";

                var versionString = XmlValueReader.ReadAttributeValue(file, Selector, AttributeName, namespaceBindings);
                if (versionString == null)
                    throw new InvalidOperationException();

                var version = Version.Parse(versionString);

                var newVersion = new Version(version.Major, version.Minor, buildNumber);

                patcher.PatchAttribute(
                    file,
                    Selector,
                    AttributeName,
                    newVersion.ToString(4),
                    namespaceBindings);
            }
        }
    }
}