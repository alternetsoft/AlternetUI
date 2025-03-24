using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates known system fonts.
    /// </summary>
    /// <remarks>
    /// These values map 1:1 the native values supported by the Windows' GetStockObject function.
    /// Note that other ports (other than Windows) will try to provide meaningful fonts but
    /// they usually map the same font to various font values.
    /// </remarks>
    public enum SystemSettingsFont
    {
        /// <summary>
        /// Original equipment manufacturer dependent fixed-pitch font.
        /// </summary>
        OemFixed = 10,

        /// <summary>
        /// Fixed-pitch (monospaced) font.
        /// </summary>
        AnsiFixed = 11,

        /// <summary>
        /// Variable-pitch (proportional) font.
        /// </summary>
        AnsiVar = 12,

        /// <summary>
        /// System font. By default, the system uses the system font to draw menus,
        /// dialog box controls, and text.
        /// </summary>
        System = 13,

        /// <summary>
        /// Device-dependent font.
        /// </summary>
        DeviceDefault = 14,

        /// <summary>
        /// Default font for user interface objects such as menus and dialog boxes.
        /// </summary>
        /// <remarks>
        /// Note that with modern GUIs nothing guarantees that the same font is used for
        /// all GUI elements, so some controls might use a different font by default.
        /// </remarks>
        DefaultGui = 17,
    }
}