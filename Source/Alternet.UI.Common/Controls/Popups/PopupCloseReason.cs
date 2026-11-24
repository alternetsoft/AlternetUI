using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the reason why a popup was closed.
    /// This struct encapsulates the reason for closing
    /// and optionally the key or character that caused the closure.
    /// </summary>
    public readonly struct PopupCloseReason
    {
        /// <summary>
        /// Gets a <see cref="PopupCloseReason"/> instance that indicates the popup was closed
        /// by unknown reason.
        /// </summary>
        public static readonly PopupCloseReason Other = new ();

        /// <summary>
        /// Gets a <see cref="PopupCloseReason"/> instance that indicates the popup was closed
        /// by clicking outside the popup and inside the container.
        /// </summary>
        public static readonly PopupCloseReason ClickContainer
            = new (PopupControl.CloseReason.ClickContainer);

        /// <summary>
        /// Gets a <see cref="PopupCloseReason"/> instance that indicates the popup was closed
        /// by losing focus.
        /// </summary>
        public static readonly PopupCloseReason FocusLost
            = new (PopupControl.CloseReason.FocusLost);

        /// <summary>
        /// Gets a <see cref="PopupCloseReason"/> instance that indicates the popup was closed
        /// by moving the mouse outside the popup.
        /// </summary>
        public static readonly PopupCloseReason MouseOverChanged
            = new (PopupControl.CloseReason.MouseOverChanged);

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupCloseReason"/> struct.
        /// </summary>
        /// <param name="key">The key that caused the popup to close.</param>
        public PopupCloseReason(Key key)
        {
            Key = key;
            Reason = PopupControl.CloseReason.Key;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupCloseReason"/> struct.
        /// </summary>
        /// <param name="ch">The character that caused the popup to close.</param>
        public PopupCloseReason(char ch)
        {
            Char = ch;
            Reason = PopupControl.CloseReason.Char;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupCloseReason"/> struct.
        /// </summary>
        /// <param name="reason">The reason for closing the popup.</param>
        public PopupCloseReason(PopupControl.CloseReason reason)
        {
            Reason = reason;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupCloseReason"/> struct.
        /// </summary>
        public PopupCloseReason()
        {
        }

        /// <summary>
        /// Gets the key that caused the popup to close.
        /// </summary>
        public readonly Key? Key { get; }

        /// <summary>
        /// Gets the character that caused the popup to close, if applicable.
        /// </summary>
        public readonly char? Char { get; }

        /// <summary>
        /// Gets the reason for closing the popup, if applicable.
        /// </summary>
        public PopupControl.CloseReason Reason { get; } = PopupControl.CloseReason.Other;
    }
}
