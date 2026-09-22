using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the kind of window.
    /// </summary>
    public enum WindowKind
    {
        /// <summary>
        /// Window.
        /// </summary>
        Window = 0,

/*
        /// <summary>
        /// Dialog.
        /// </summary>
        Dialog = 1,
*/

        /// <summary>
        /// Mini frame.
        /// </summary>
        MiniFrame = 2,

        /// <summary>
        /// Control.
        /// </summary>
        Control = 3,

        /// <summary>
        /// Popup.
        /// </summary>
        Popup = 4,

        /// <summary>
        /// Transient popup.
        /// </summary>
        TransientPopup = 5,
    }
}
