using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="GenericLabel"/> with side buttons.
    /// </summary>
    public partial class LabelAndButton : ControlAndButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LabelAndButton"/> class.
        /// </summary>
        public LabelAndButton()
        {
        }

        /// <summary>
        /// Gets main child control.
        /// </summary>
        [Browsable(false)]
        public new GenericLabel MainControl => (GenericLabel)base.MainControl;

        /// <summary>
        /// Gets main child control, same as <see cref="MainControl"/>.
        /// </summary>
        [Browsable(false)]
        public GenericLabel Label => (GenericLabel)base.MainControl;

        /// <summary>
        /// Gets or sets <see cref="AbstractControl.Text"/> property of the main child control.
        /// </summary>
        [Browsable(true)]
        public override string Text
        {
            get
            {
                return Label.Text;
            }

            set
            {
                Label.Text = value;
            }
        }

        /// <inheritdoc/>
        protected override AbstractControl CreateControl()
        {
            return new GenericLabel();
        }
    }
}
