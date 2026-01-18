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
    [ControlCategory("Editors")]
    public partial class ListPicker : SpeedButtonWithListPopup
    {
        /// <summary>
        /// Gets or sets whether to assign default control colors
        /// in the constructor using <see cref="AbstractControl.UseControlColors"/>.
        /// Default is <c>true</c>.
        /// </summary>
        public static bool DefaultUseControlColors = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListPicker"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ListPicker(AbstractControl parent)
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
            UseControlColors(DefaultUseControlColors);
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.Other;

        /// <inheritdoc/>
        protected override void OnSystemColorsChanged(EventArgs e)
        {
            UseControlColors(DefaultUseControlColors);
            base.OnSystemColorsChanged(e);
        }
    }
}