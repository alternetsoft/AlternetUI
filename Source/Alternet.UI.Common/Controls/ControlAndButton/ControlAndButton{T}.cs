using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control with a generic child control and associated button functionality.
    /// </summary>
    /// <typeparam name="T">The type of the generic child control.</typeparam>
    public partial class ControlAndButton<T> : ControlAndButton
        where T : AbstractControl, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControlAndButton{T}"/> class.
        /// </summary>
        public ControlAndButton()
        {
        }

        /// <summary>
        /// Gets main child control.
        /// </summary>
        [Browsable(false)]
        public new T MainControl => (T)base.MainControl;

        /// <summary>
        /// Gets or sets <see cref="AbstractControl.Text"/> property of the main child control.
        /// </summary>
        [Browsable(true)]
        public override string Text
        {
            get
            {
                return MainControl.Text;
            }

            set
            {
                MainControl.Text = value;
            }
        }

        /// <inheritdoc/>
        protected override AbstractControl CreateControl()
        {
            return new T();
        }
    }
}
