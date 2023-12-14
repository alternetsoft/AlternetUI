using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies constants that define which mouse button was pressed.
    /// </summary>
    [Flags]
    public enum MouseButtons
    {
        /// <summary>
        /// No mouse button was pressed.
        /// </summary>
        None = 0,

        /// <summary>
        /// The left mouse button was pressed.
        /// </summary>
        Left = 0x100000,
        
        /// <summary>
        /// The right mouse button was pressed.
        /// </summary>
        Right = 0x200000,
        
        /// <summary>
        /// The middle mouse button was pressed.
        /// </summary>
        Middle = 0x400000,
        
        /// <summary>
        /// The first XButton was pressed.
        /// </summary>
        XButton1 = 0x800000,
        
        /// <summary>
        /// The second XButton was pressed.
        /// </summary>
        XButton2 = 0x1000000
    }
}
