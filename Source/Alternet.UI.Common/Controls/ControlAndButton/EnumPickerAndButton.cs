using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="EnumPicker"/> with side buttons.
    /// </summary>
    public partial class EnumPickerAndButton : ControlAndButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumPickerAndButton"/> class.
        /// </summary>
        public EnumPickerAndButton()
        {
        }

        /// <summary>
        /// Gets main child control.
        /// </summary>
        [Browsable(false)]
        public new EnumPicker MainControl => (EnumPicker)base.MainControl;

        /// <summary>
        /// Gets main child control, same as <see cref="MainControl"/>.
        /// </summary>
        [Browsable(false)]
        public EnumPicker EnumPicker => (EnumPicker)base.MainControl;

        /// <inheritdoc/>
        protected override AbstractControl CreateControl()
        {
            return new EnumPicker();
        }
    }
}
