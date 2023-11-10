using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// A caret is a blinking cursor showing the position where the typed text will appear.
    /// Text controls usually have their own caret but <see cref="Caret"/> provides a way to use
    /// a caret in other controls.
    /// </summary>
    /// <remarks>
    /// A caret is always associated with a control and the current caret can be retrieved
    /// using <see cref="Control"/> methods. The same caret can't be reused in two different controls.
    /// </remarks>
    /// <remarks>
    /// Currently, the caret appears as a rectangle of the given size.
    /// </remarks>
    public class Caret : DisposableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Caret"/> class.
        /// </summary>
        public Caret()
            : base(Native.WxOtherFactory.CreateCaret(), true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Caret"/> class
        /// with the given size (in pixels) and associates it with the control.
        /// </summary>
        /// <param name="control">A control the caret is associated with.</param>
        /// <param name="width">Caret width in pixels.</param>
        /// <param name="height">Caret height in pixels.</param>
        public Caret(Control control, int width, int height)
            : base(Native.WxOtherFactory.CreateCaret2(control.WxWidget, width, height), true)
        {
        }

        /// <summary>
        /// Gets or sets blink time of the carets.
        /// </summary>
        /// <remarks>
        /// Blink time is measured in milliseconds and is the time elapsed
        /// between 2 inversions of the caret (blink time of the caret is common
        /// to all carets in the application, so this property is static).
        /// </remarks>
        public static int BlinkTime
        {
            get
            {
                return Native.WxOtherFactory.CaretGetBlinkTime();
            }

            set
            {
                Native.WxOtherFactory.CaretSetBlinkTime(value);
            }
        }

        /// <summary>
        /// Gets or sets the caret size.
        /// </summary>
        public Int32Size Size
        {
            get
            {
                return Native.WxOtherFactory.CaretGetSize(Handle);
            }

            set
            {
                Native.WxOtherFactory.CaretSetSize(Handle, value.Width, value.Height);
            }
        }

        /// <summary>
        /// Gets or sets the caret position (in pixels).
        /// </summary>
        public Int32Point Position
        {
            get
            {
                return Native.WxOtherFactory.CaretGetPosition(Handle);
            }

            set
            {
                Native.WxOtherFactory.CaretMove(Handle, value.X, value.Y);
            }
        }

        /// <summary>
        /// Returns true if the caret was created successfully.
        /// </summary>
        public bool IsOk
        {
            get
            {
                return Native.WxOtherFactory.CaretIsOk(Handle);
            }
        }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value indicating whether the caret is visible.
        /// </summary>
        /// <remarks>
        /// Returns <c>true</c> if the caret is visible and <c>false</c> if it
        /// is permanently hidden (if it is blinking and not shown
        /// currently but will be after the next blink, this method still returns true).
        /// </remarks>
        public bool Visible
        {
            get
            {
                return Native.WxOtherFactory.CaretIsVisible(Handle);
            }

            set
            {
                Native.WxOtherFactory.CaretShow(Handle, value);
            }
        }

        /// <summary>
        ///  Hides the caret, same as Show(false).
        /// </summary>
        public void Hide()
        {
            Native.WxOtherFactory.CaretHide(Handle);
        }

        /// <summary>
        /// Shows or hides the caret.
        /// </summary>
        /// <param name="show">A <see cref="bool"/> value indicating whether
        /// the caret is visible.</param>
        public void Show(bool show = true)
        {
            Native.WxOtherFactory.CaretShow(Handle, show);
        }

        internal IntPtr GetWindow()
        {
            return Native.WxOtherFactory.CaretGetWindow(Handle);
        }

        /// <inheritdoc/>
        protected override void DisposeUnmanagedResources()
        {
            Native.WxOtherFactory.DeleteCaret(Handle);
        }
    }
}