using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides additional data for the <see cref="AbstractControl.LostFocus"/>
    /// event of the <see cref="AbstractControl"/>.
    /// </summary>
    public class LostFocusEventArgs : BaseFocusEventArgs
    {
        /// <summary>
        /// Gets an empty <see cref="LostFocusEventArgs"/> instance with default parameters.
        /// </summary>
        public static readonly new LostFocusEventArgs Empty = new EmptyLostFocusEventArgs();

        /// <summary>
        /// Initializes a new instance of the <see cref="LostFocusEventArgs"/> class.
        /// </summary>
        public LostFocusEventArgs()
            : base(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LostFocusEventArgs"/> class.
        /// </summary>
        /// <param name="newFocusedControl">
        /// The control that will be focused. If Null, some native
        /// control will be focused (possibly outside the application) and it can not be casted to
        /// the <see cref="AbstractControl"/>.
        /// </param>
        public LostFocusEventArgs(AbstractControl? newFocusedControl)
            : base(newFocusedControl)
        {
        }

        private class EmptyLostFocusEventArgs : LostFocusEventArgs
        {
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
