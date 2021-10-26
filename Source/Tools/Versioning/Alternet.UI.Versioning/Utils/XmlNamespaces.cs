using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Alternet.UI.Versioning
{
    static class XmlNamespaces
    {
        public static readonly XNamespace MSBuild = "http://schemas.microsoft.com/developer/msbuild/2003";
        public static readonly XNamespace VSManifest = "http://schemas.microsoft.com/developer/vsx-schema/2011";
    }
}