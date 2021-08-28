using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.IO;

namespace Alternet.UI.Build.Tasks
{
    class UIXmlDocument
    {
        public UIXmlDocument(string className, string resourceName)
        {
            ClassName = className;
            ResourceName = resourceName;
        }

        public string ClassName { get; }
        public string ResourceName { get; }
    }
}