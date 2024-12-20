using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// This item supports the framework infrastructure and is not intended to be used directly
    /// from your code.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class XmlnsDefinitionAttribute : Attribute
    {
        /// <summary>
        /// This item supports the framework infrastructure and is not intended to be used
        /// directly from your code.
        /// </summary>
        public XmlnsDefinitionAttribute(string xmlNamespace, string clrNamespace)
        {
        }
    }
}
