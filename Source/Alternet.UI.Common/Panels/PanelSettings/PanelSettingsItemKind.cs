using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates known kinds of the <see cref="PanelSettingsItem"/>.
    /// </summary>
    public enum PanelSettingsItemKind
    {
        /// <summary>
        /// Item is empty.
        /// </summary>
        Empty,

        /// <summary>
        /// Item is horizontal line.
        /// </summary>
        Line,

        /// <summary>
        /// Item is separator (empty space).
        /// </summary>
        Spacer,

        /// <summary>
        /// Item is generic label.
        /// </summary>
        Label,

        /// <summary>
        /// Item is link label.
        /// </summary>
        LinkLabel,

        /// <summary>
        /// Item is value of the specified type.
        /// </summary>
        Value,

        /// <summary>
        /// Items is button.
        /// </summary>
        Button,

        /// <summary>
        /// Items is selector with the list of values and non-editable text.
        /// </summary>
        Selector,

        /// <summary>
        /// Items is selector with the list of values and editable text.
        /// </summary>
        EditableSelector,
    }
}
