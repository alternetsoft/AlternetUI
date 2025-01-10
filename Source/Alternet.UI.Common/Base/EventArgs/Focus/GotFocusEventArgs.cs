using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides additional data for the <see cref="AbstractControl.GotFocus"/>
    /// event of the <see cref="AbstractControl"/>.
    /// </summary>
    public class GotFocusEventArgs : BaseFocusEventArgs
    {
        /// <summary>
        /// Gets an empty <see cref="GotFocusEventArgs"/> instance with default parameters.
        /// </summary>
        public static readonly new GotFocusEventArgs Empty = new EmptyGotFocusEventArgs();

        /// <summary>
        /// Initializes a new instance of the <see cref="GotFocusEventArgs"/> class.
        /// </summary>
        public GotFocusEventArgs()
            : base(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GotFocusEventArgs"/> class.
        /// </summary>
        /// <param name="oldFocusedControl">
        /// The control that was focused. If Null, some native
        /// control was focused (possibly outside the application) and it can not be casted to
        /// the <see cref="AbstractControl"/>.
        /// </param>
        public GotFocusEventArgs(AbstractControl? oldFocusedControl)
            : base(oldFocusedControl)
        {
        }

        private class EmptyGotFocusEventArgs : GotFocusEventArgs
        {
            public EmptyGotFocusEventArgs()
                : base(null)
            {
            }

            public override AbstractControl? Value
            {
                get => base.Value;

                set
                {
                }
            }
        }
    }
}
