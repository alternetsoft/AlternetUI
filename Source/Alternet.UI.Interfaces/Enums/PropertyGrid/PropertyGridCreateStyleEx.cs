using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines <see cref="PropertyGrid"/> extended options.
    /// </summary>
    [Flags]
    public enum PropertyGridCreateStyleEx
    {
        /// Speeds up switching to HideCategories mode. Initially, if
        /// HideCategories is not defined, the non-categorized data storage is
        /// not activated, and switching the mode first time becomes somewhat slower.
        /// InitNoCat activates the non-categorized data storage right away.
        /// NOTE: If you do plan not switching to non-categoric mode, or if
        /// you don't plan to use categories at all, then using this style will result
        /// in waste of resources.
        /// <summary>
        /// </summary>
        InitNoCat = 0x00001000,

        /// <summary>
        /// Extended window style that sets PropertyGridManager toolbar to not
        /// use flat style.
        /// </summary>
        NoFlatToolbar = 0x00002000,

        /// <summary>
        /// Shows alphabetic/categoric mode buttons from toolbar.
        /// </summary>
        ModeButtons = 0x00008000,

        /// <summary>
        /// Show property help strings as tool tips instead as text on the status bar.
        /// You can set the help strings using SetPropertyHelpString member function.
        /// </summary>
        HelpAsTooltips = 0x00010000,

        /// <summary>
        /// Allows relying on native double-buffering.
        /// </summary>
        NativeDoubleBuffering = 0x00080000,

        /// <summary>
        /// Set this style to let user have ability to set values of properties to
        /// unspecified state. Same as setting PropAutoUnspecified for
        /// all properties.
        /// </summary>
        AutoUnspecifiedValues = 0x00200000,

        /// <summary>
        /// If this style is used, built-in attributes (such as FloatPrecision
        /// and StringPassword) are not stored into property's attribute storage
        /// (thus they are not readable).
        /// Note that this option is global, and applies to all property
        /// containers.
        /// </summary>
        WriteOnlyBuiltinAttributes = 0x00400000,

        /// <summary>
        /// Hides page selection buttons from toolbar.
        /// </summary>
        HidePageButtons = 0x01000000,

        /// <summary>
        /// Allows multiple properties to be selected by user (by pressing SHIFT
        /// when clicking on a property, or by dragging with left mouse button
        /// down).
        /// </summary>
        MultipleSelection = 0x02000000,

        /// <summary>
        /// This enables top-level window tracking which allows <see cref="PropertyGrid"/> to
        /// notify the application of last-minute property value changes by user.
        /// This style is not enabled by default because it may cause crashes when
        /// wxPropertyGrid is used in with wxAUI or similar system.
        /// Note: If you are not in fact using any system that may change
        /// wxPropertyGrid's top-level parent window on its own, then you
        /// are recommended to enable this style.
        /// </summary>
        EnableTlpTracking = 0x04000000,

        /// <summary>
        /// Don't show divider above toolbar, on Windows.
        /// </summary>
        NoToolbarDivider = 0x08000000,

        /// <summary>
        /// Show a separator below the toolbar.
        /// </summary>
        ToolbarSeparator = 0x10000000,

        /// <summary>
        /// Allows to take focus on the entire area (on canvas)
        /// even if <see cref="PropertyGrid"/> is not a standalone control.
        /// </summary>
        AlwaysAllowFocus = 0x00100000,

        /// <summary>
        /// Default style.
        /// </summary>
        DefaultStyle = AlwaysAllowFocus,
    }
}