using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Platform independent implementation of the <see cref="ICaretHandler"/> interface.
    /// </summary>
    public class PlessCaretHandler : DisposableObject, ICaretHandler
    {
        private static int blinkTime = 530;
        private static LightDarkColor? color;

        private readonly CaretInfo info = new();
        private Control? control;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlessCaretHandler"/> class.
        /// </summary>
        public PlessCaretHandler()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlessCaretHandler"/> class.
        /// </summary>
        public PlessCaretHandler(Control control, int width, int height)
        {
            info.Size = (width, height);
            if(control is not null)
            {
                this.control = control;
                control.CaretInfo = info;
            }
        }

        /// <summary>
        /// Gets or sets caret colors.
        /// </summary>
        public static LightDarkColor CaretColor
        {
            get
            {
                return color ??= new(Color.Black, Color.White);
            }

            set
            {
                color = value;
            }
        }

        /// <summary>
        /// Gets or sets caret blonk time. This is a dummy property and is not currently used.
        /// </summary>
        public static int CaretBlinkTime
        {
            get
            {
                return blinkTime;
            }

            set
            {
                blinkTime = value;
            }
        }

        /// <inheritdoc/>
        public virtual bool IsOk
        {
            get => control != null && !IsDisposed && !control.IsDisposed;
        }

        /// <inheritdoc/>
        public virtual Control? Control
        {
            get => control;
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
            get => CaretBlinkTime;

            set
            {
                CaretBlinkTime = value;
                Changed();
            }
        }

        /// <inheritdoc/>
        public virtual SizeI Size
        {
            get => info.Size;

            set
            {
                if (info.Size == value)
                    return;
                info.Size = value;
                Changed();
            }
        }

        /// <inheritdoc/>
        public virtual PointI Position
        {
            get => info.Position;

            set
            {
                if (info.Position == value)
                    return;
                info.Position = value;
                Changed();
            }
        }

        /// <inheritdoc/>
        public virtual bool Visible
        {
            get => info.Visible;

            set
            {
                if (info.Visible == value)
                    return;
                info.Visible = value;
                Changed();
            }
        }

        /// <summary>
        /// Called to update the caret on screen when it's position, size or visibility were changed.
        /// </summary>
        protected virtual void Changed()
        {
            control?.InvalidateCaret();
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            base.DisposeManaged();
            info.IsDisposed = true;
            if(control is not null)
            {
                control = null;
            }
        }
    }
}
