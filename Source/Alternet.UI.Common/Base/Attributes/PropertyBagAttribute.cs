using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Indicates that a property is part of a property bag.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public partial class PropertyBagAttribute : BaseAttribute
    {
    }
}
