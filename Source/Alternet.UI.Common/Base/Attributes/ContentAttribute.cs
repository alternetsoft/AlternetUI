using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Indicates which property of a type is the content property. An UIXML processor uses this information
    /// when processing child elements of UIXML representations of the attributed type.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class ContentAttribute : BaseAttribute
    {
    }
}
