using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Item of the <see cref="PropertyGrid"/>.
    /// </summary>
    public interface IPropertyGridItem
    {
        /// <summary>
        /// Item handle.
        /// </summary>
        IntPtr Handle { get; }
    }
}
