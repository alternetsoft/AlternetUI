using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains <see cref="PropertyGrid"/> settings related to <see cref="Type"/>.
    /// </summary>
    public interface IPropertyGridTypeRegistry
    {
        /// <summary>
        /// Gets or sets <see cref="IPropertyGridItem"/> create function.
        /// </summary>
        PropertyGridItemCreate? CreateFunc { get; set; }
    }
}
