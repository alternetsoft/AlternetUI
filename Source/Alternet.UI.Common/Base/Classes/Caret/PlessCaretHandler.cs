using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public class PlessCaretHandler : DisposableObject, ICaretHandler
    {
        private static int blinkTime = 530;
        private static LightDarkColor? color;

        private Control? control;
        private readonly CaretInfo info = new();

        public PlessCaretHandler()
        {

        }

        public PlessCaretHandler(Control control, int width, int height)
        {
            info.Size = (width, height);
            this.control = control;
            control.CaretInfo = info;
        }

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

        public Control? Control
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
            }
        }

        public virtual SizeI Size
        {
            get => info.Size;

            set
            {
                if (info.Size == value)
                    return;
                info.Size = value;
                control?.InvalidateCaret();
            }
        }

        public virtual PointI Position
        {
            get => info.Position;

            set
            {
                if (info.Position == value)
                    return;
                info.Position = value;
                control?.InvalidateCaret();
            }
        }

        public virtual bool Visible
        {
            get => info.Visible;

            set
            {
                if (info.Visible == value)
                    return;
                info.Visible = value;
                control?.InvalidateCaret();
            }
        }

        public virtual bool IsOk
        {
            get => control != null && !IsDisposed && !control.IsDisposed;
        }

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
