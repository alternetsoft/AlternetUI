using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    // Misc. argument flags.
    [Flags]
    internal enum PropertyGridMiscArgFlags
    {
        // Get/Store full value instead of displayed value.
        FullValue = 0x00000001,

        // Perform special action in case of unsuccessful conversion.
        ReportError = 0x00000002,

        PropertySpecific = 0x00000004,

        // Get/Store editable value instead of displayed one (should only be
        // different in the case of common values)
        EditableValue = 0x00000008,

        // Used when dealing with fragments of composite string value
        CompositeFragment = 0x00000010,

        // Means property for which final string value is for cannot really be
        // edited.
        UneditableCompositeFragment = 0x00000020,

        // ValueToString() called from GetValueAsString()
        // (guarantees that input wxVariant value is current own value)
        ValueIsCurrent = 0x00000040,

        // Value is being set programmatically (i.e. not by user)
        ProgrammaticValue = 0x00000080,
    }
}
