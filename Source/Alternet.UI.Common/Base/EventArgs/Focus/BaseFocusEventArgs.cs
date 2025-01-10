using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides additional data for the <see cref="AbstractControl.LostFocus"/>
    /// and <see cref="AbstractControl.GotFocus"/>
    /// events of the <see cref="AbstractControl"/>.
    /// </summary>
    public class BaseFocusEventArgs : BaseEventArgs<AbstractControl?>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseFocusEventArgs"/> class.
        /// </summary>
        /// <param name="control">The control that was focused
        /// or control that will be focused.</param>
        public BaseFocusEventArgs(AbstractControl? control)
            : base(control)
        {
        }
    }
}
