using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="ColorPicker"/> with side buttons.
    /// </summary>
    public partial class ColorPickerAndButton : ControlAndButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorPickerAndButton"/> class.
        /// </summary>
        public ColorPickerAndButton()
        {
        }

        /// <summary>
        /// Gets main child control.
        /// </summary>
        [Browsable(false)]
        public new ColorPicker MainControl => (ColorPicker)base.MainControl;

        /// <summary>
        /// Gets main child control, same as <see cref="MainControl"/>.
        /// </summary>
        [Browsable(false)]
        public ColorPicker ColorPicker => (ColorPicker)base.MainControl;

        /// <inheritdoc/>
        protected override AbstractControl CreateControl()
        {
            return new ColorPicker();
        }
    }
}
