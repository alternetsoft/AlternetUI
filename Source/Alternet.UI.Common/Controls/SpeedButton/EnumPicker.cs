using System;
using System.ComponentModel;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents control that displays a selected enum value and allows to change it
    /// with the popup window.
    /// </summary>
    [ControlCategory("Other")]
    public partial class EnumPicker : SpeedEnumButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumPicker"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public EnumPicker(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumPicker"/> class.
        /// </summary>
        public EnumPicker()
        {
            UseTheme = KnownTheme.StaticBorder;
            SetContentHorizontalAlignment(HorizontalAlignment.Left);
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.ColorPicker;
    }
}