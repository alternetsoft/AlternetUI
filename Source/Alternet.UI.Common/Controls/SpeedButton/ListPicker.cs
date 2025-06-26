using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents control that displays a selected item and allows to change it
    /// with the popup window with virtual list box.
    /// </summary>
    [ControlCategory("Other")]
    public partial class ListPicker : SpeedButtonWithListPopup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListPicker"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ListPicker(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListPicker"/> class.
        /// </summary>
        public ListPicker()
        {
            UseTheme = KnownTheme.StaticBorder;
            SetContentHorizontalAlignment(HorizontalAlignment.Left);
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.Other;
    }
}