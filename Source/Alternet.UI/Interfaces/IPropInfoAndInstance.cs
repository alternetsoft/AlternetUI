using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains object and property information.
    /// </summary>
    public interface IPropInfoAndInstance
    {
        /// <summary>
        /// Gets or sets objects instance in which property is contained.
        /// </summary>
        object? Instance { get; set; }

        /// <summary>
        /// Gets or sets property information.
        /// </summary>
        PropertyInfo? PropInfo { get; set; }
    }
}
