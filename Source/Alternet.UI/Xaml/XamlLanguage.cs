using Alternet.UI;
using System;
using System.Collections.Generic;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

[assembly: XmlnsDefinition("http://schemas.alternetsoft.com/ui/2021", "Alternet.UI")]

namespace Alternet.UI
{
    public interface ITestRootObjectProvider
    {
        object RootObject { get; }
    }

    public interface ITestProvideValueTarget
    {
        object TargetObject { get; }

        object TargetProperty { get; }
    }

    public interface ITestUriContext
    {
        Uri BaseUri { get; set; }
    }

    public class ContentAttribute : Attribute
    {
    }

    public class XmlnsDefinitionAttribute : Attribute
    {
        public XmlnsDefinitionAttribute(string xmlNamespace, string clrNamespace)
        {
        }
    }

    public class UsableDuringInitializationAttribute : Attribute
    {
        public UsableDuringInitializationAttribute(bool usable)
        {
        }
    }

    public class DeferredContentAttribute : Attribute
    {
    }
}

namespace XamlX.Runtime
{
    public interface IXamlParentStackProviderV1
    {
        IEnumerable<object> Parents { get; }
    }

    public interface IXamlXmlNamespaceInfoProviderV1
    {
        IReadOnlyDictionary<string, IReadOnlyList<XamlXmlNamespaceInfoV1>> XmlNamespaces { get; }
    }

    public class XamlXmlNamespaceInfoV1
    {
        public string? ClrNamespace { get; set; }

        public string? ClrAssemblyName { get; set; }
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
