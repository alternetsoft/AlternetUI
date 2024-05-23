using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public class Caret : HandledObject<object>
    {
        private readonly IControl? control;
        private SizeI? size;

        /// <summary>
        /// Initializes a new instance of the <see cref="Caret"/> class.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Caret()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Caret"/> class
        /// with the given size (in pixels) and associates it with the control.
        /// </summary>
        /// <param name="control">A control the caret is associated with.</param>
        /// <param name="width">Caret width in pixels.</param>
        /// <param name="height">Caret height in pixels.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Caret(IControl control, int width, int height)
        {
            this.control = control;
            size = (width, height);
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
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return NativeDrawing.Default.CaretGetBlinkTime();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                NativeDrawing.Default.CaretSetBlinkTime(value);
            }
        }

        /// <summary>
        /// Gets or sets the caret size.
        /// </summary>
        public SizeI Size
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return NativeDrawing.Default.CaretGetSize(this);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                NativeDrawing.Default.CaretSetSize(this, value);
            }
        }

        /// <summary>
        /// Gets or sets the caret position (in pixels).
        /// </summary>
        public PointI Position
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return NativeDrawing.Default.CaretGetPosition(this);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                NativeDrawing.Default.CaretSetPosition(this, value);
            }
        }

        /// <summary>
        /// Returns true if the caret was created successfully.
        /// </summary>
        public bool IsOk
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return NativeDrawing.Default.CaretIsOk(this);
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
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return NativeDrawing.Default.CaretGetVisible(this);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                NativeDrawing.Default.CaretSetVisible(this, value);
            }
        }

        /// <summary>
        ///  Hides the caret, same as Show(false).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Hide()
        {
            Visible = false;
        }

        /// <summary>
        /// Shows or hides the caret.
        /// </summary>
        /// <param name="show">A <see cref="bool"/> value indicating whether
        /// the caret is visible.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Show(bool show = true)
        {
            Visible = show;
        }

        /// <inheritdoc/>
        protected override object CreateHandler()
        {
            if(control is null || size is null)
                return NativeDrawing.Default.CreateCaret();
            return NativeDrawing.Default.CreateCaret(control, size.Value.Width, size.Value.Height);
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            NativeDrawing.Default.DisposeCaret(this);
        }
    }
}