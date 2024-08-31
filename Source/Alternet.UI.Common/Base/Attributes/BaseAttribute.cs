using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Base attribute class. All other attributes descend from <see cref="BaseAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public partial class BaseAttribute : Attribute
    {
    }
}
