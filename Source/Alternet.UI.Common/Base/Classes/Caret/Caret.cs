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
    public class Caret : HandledObject<ICaretHandler>
    {
        private readonly Control? control;
        private SizeI? size;

        /// <summary>
        /// Initializes a new instance of the <see cref="Caret"/> class.
        /// </summary>
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
        public Caret(Control control, int width, int height)
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
        /// to all carets in the application).
        /// </remarks>
        public virtual int BlinkTime
        {
            get
            {
                return Handler.BlinkTime;
            }

            set
            {
                Handler.BlinkTime = value;
            }
        }

        /// <summary>
        /// Gets or sets the caret size.
        /// </summary>
        public virtual SizeI Size
        {
            get
            {
                return Handler.Size;
            }

            set
            {
                Handler.Size = value;
            }
        }

        /// <summary>
        /// Gets or sets the caret position (in pixels).
        /// </summary>
        public virtual PointI Position
        {
            get
            {
                return Handler.Position;
            }

            set
            {
                Handler.Position = value;
            }
        }

        /// <summary>
        /// Returns true if the caret was created successfully.
        /// </summary>
        public virtual bool IsOk
        {
            get
            {
                return Handler.IsOk;
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
        public virtual bool Visible
        {
            get
            {
                return Handler.Visible;
            }

            set
            {
                Handler.Visible = value;
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
        protected override ICaretHandler CreateHandler()
        {
            if(control is null || size is null)
                return BaseApplication.Handler.CreateCaretHandler();
            return BaseApplication.Handler.CreateCaretHandler(
                control,
                size.Value.Width,
                size.Value.Height);
        }
    }
}