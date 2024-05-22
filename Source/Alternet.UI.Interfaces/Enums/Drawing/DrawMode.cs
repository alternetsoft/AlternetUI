using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies how the elements of a control are drawn.
    /// </summary>
    public enum DrawMode
    {
        /// <summary>
        /// All the elements in a control are drawn by the operating system and
        /// are of the same size.
        /// </summary>
        Normal,

        /// <summary>
        /// All the elements in the control are drawn manually and are
        /// of the same size.
        /// </summary>
        OwnerDrawFixed,

        /// <summary>
        /// All the elements in the control are drawn manually
        /// and can differ in size.
        /// </summary>
        OwnerDrawVariable,
    }
}
