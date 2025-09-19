using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates all supported UI platform kinds.
    /// </summary>
    public enum UIPlatformKind
    {
        /// <summary>
        /// Platform is WxWidgets.
        /// </summary>
        WxWidgets,

        /// <summary>
        /// Platform is Microsoft Maui.
        /// </summary>
        Maui,

        /// <summary>
        /// Platform is AvaloniaUI.
        /// </summary>
        Avalonia,

        /// <summary>
        /// Platform is WinForms.
        /// </summary>
        WinForms,

        /// <summary>
        /// Platform is WinUI.
        /// </summary>
        WinUI,

        /// <summary>
        /// Platform-independent.
        /// </summary>
        Unspecified,
    }
}
