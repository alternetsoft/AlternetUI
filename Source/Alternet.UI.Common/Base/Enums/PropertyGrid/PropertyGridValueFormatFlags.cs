using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Misc. argument flags used in the value format methods in the property grid control.
    /// </summary>
    /// <remarks>
    /// This enumeration supports a bitwise combination of its member values.
    /// </remarks>
    [Flags]
    public enum PropertyGridValueFormatFlags
    {
        /// <summary>
        /// Get/Store full value instead of displayed value.
        /// </summary>
        FullValue = 0x00000001,

        /// <summary>
        /// Perform special action in case of unsuccessful conversion.
        /// </summary>
        ReportError = 0x00000002,

        /// <summary>
        /// Property specific flag.
        /// </summary>
        PropertySpecific = 0x00000004,

        /// <summary>
        /// Get/Store editable value instead of displayed one (should only be
        /// different in the case of common values).
        /// </summary>
        EditableValue = 0x00000008,

        /// <summary>
        /// Used when dealing with fragments of composite string value.
        /// </summary>
        CompositeFragment = 0x00000010,

        /// <summary>
        /// Means property for which final string value is for cannot really be edited.
        /// </summary>
        UneditableCompositeFragment = 0x00000020,

        /// <summary>
        /// ValueToString() called from GetValueAsString()
        /// (guarantees that input wxVariant value is current own value).
        /// </summary>
        ValueIsCurrent = 0x00000040,

        /// <summary>
        /// Value is being set programmatically (i.e. not by user).
        /// </summary>
        ProgrammaticValue = 0x00000080,
    }
}
