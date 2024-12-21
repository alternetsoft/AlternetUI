#pragma warning disable
using System;
using System.Collections.Generic;
using Alternet.UI;

namespace Alternet.UI
{
    /// <summary>
    /// This item supports the framework infrastructure and is not intended to be used directly
    /// from your code.
    /// </summary>
    public interface ITestRootObjectProvider
    {
        /// <summary>
        /// This item supports the framework infrastructure and is not intended to be used
        /// directly from your code.
        /// </summary>
        object RootObject { get; }
    }

    /// <summary>
    /// This item supports the framework infrastructure and is not intended to be used directly
    /// from your code.
    /// </summary>
    public interface ITestProvideValueTarget
    {
        /// <summary>
        /// This item supports the framework infrastructure and is not intended to be used
        /// directly from your code.
        /// </summary>
        object TargetObject { get; }

        /// <summary>
        /// This item supports the framework infrastructure and is not intended to be used
        /// directly from your code.
        /// </summary>
        object TargetProperty { get; }
    }

    /// <summary>
    /// This item supports the framework infrastructure and is not intended to be used directly
    /// from your code.
    /// </summary>
    public interface ITestUriContext
    {
        /// <summary>
        /// This item supports the framework infrastructure and is not intended to be used
        /// directly from your code.
        /// </summary>
        Uri BaseUri { get; set; }
    }
}

namespace XamlX.Runtime
{
    /// <summary>
    /// This item supports the framework infrastructure and is not intended to be used directly
    /// from your code.
    /// </summary>
    public interface IXamlParentStackProviderV1
    {
        /// <summary>
        /// This item supports the framework infrastructure and is not intended to be used
        /// directly from your code.
        /// </summary>
        IEnumerable<object> Parents { get; }
    }

    /// <summary>
    /// This item supports the framework infrastructure and is not intended to be used directly
    /// from your code.
    /// </summary>
    public interface IXamlXmlNamespaceInfoProviderV1
    {
        /// <summary>
        /// This item supports the framework infrastructure and is not intended to be used
        /// directly from your code.
        /// </summary>
        IReadOnlyDictionary<string, IReadOnlyList<XamlXmlNamespaceInfoV1>> XmlNamespaces { get; }
    }

    /// <summary>
    /// This item supports the framework infrastructure and is not intended to be used directly
    /// from your code.
    /// </summary>
    public class XamlXmlNamespaceInfoV1
    {
        /// <summary>
        /// This item supports the framework infrastructure and is not intended to be used
        /// directly from your code.
        /// </summary>
        public string? ClrNamespace { get; set; }

        /// <summary>
        /// This item supports the framework infrastructure and is not intended to be used
        /// directly from your code.
        /// </summary>
        public string? ClrAssemblyName { get; set; }
    }
}
