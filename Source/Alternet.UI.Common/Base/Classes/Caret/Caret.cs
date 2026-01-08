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
    /// using <see cref="AbstractControl"/> methods. The same caret can't be reused
    /// in two different controls.
    /// </remarks>
    /// <remarks>
    /// Currently, the caret appears as a rectangle of the given size.
    /// </remarks>
    public class Caret : HandledObject<ICaretHandler>
    {
        private WeakReferenceValue<AbstractControl>? control;
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
        public Caret(AbstractControl control, int width, int height)
        {
            this.control = new(control);
            size = (width, height);
        }

        /// <summary>
        /// Occurs when caret position is changed. This event is called only for the carets
        /// attached to the controls (<see cref="Control"/> property is not Null).
        /// </summary>
        public static event EventHandler? ControlPositionChanged;

        /// <summary>
        /// Time, in milliseconds, for how long a blinking caret should stay visible during a
        /// single blink cycle before it disappears.
        /// </summary>
        /// <remarks>
        /// If this value is zero, caret should be visible all the time instead of blinking.
        /// If the value is negative, the platform does not support the user setting.
        /// </remarks>
        /// <remarks>
        /// Default on Windows = 530;
        /// </remarks>
        public static int CaretOnMSec
        {
            get
            {
                return SystemSettings.GetMetric(SystemSettingsMetric.CaretOnMSec);
            }
        }

        /// <summary>
        /// Gets or sets whether to use internal caret. This can be used for testing purposes.
        /// Usage of this property depends on platform. By default, it is true. 
        /// </summary>
        public static bool UseGeneric { get; set; } = true;

        /// <summary>
        /// Time, in milliseconds, for how long a blinking caret should stay invisible during
        /// a single blink cycle before it reappears.
        /// </summary>
        /// <remarks>
        /// If this value is zero, caret should be visible all the time instead of blinking.
        /// If the value is negative, the platform does not support the user setting.
        /// </remarks>
        /// <remarks>
        /// Default on Windows = 530;
        /// </remarks>
        public static int CaretOffMSec
        {
            get
            {
                return SystemSettings.GetMetric(SystemSettingsMetric.CaretOffMSec);
            }
        }

        /// <summary>
        /// Time, in milliseconds, for how long a caret should blink after a user interaction.
        /// </summary>
        /// <remarks>
        /// After this timeout has expired, the caret should stay continuously visible until
        /// the user interacts with the caret again(for example by entering, deleting or
        /// cutting text). If this value is negative, carets should blink forever;
        /// if it is zero, carets should not blink at all.
        /// </remarks>
        /// <remarks>
        /// Default on Windows = -1;
        /// </remarks>
        public static int CaretTimeoutMSec
        {
            get
            {
                return SystemSettings.GetMetric(SystemSettingsMetric.CaretTimeoutMSec);
            }
        }

        /// <summary>
        /// Gets or sets blink time of the carets.
        /// </summary>
        /// <remarks>
        /// Blink time is measured in milliseconds and is the time elapsed
        /// between 2 inversions of the caret (blink time of the caret is common
        /// to all carets in the application).
        /// </remarks>
        /// <remarks>
        /// Default on Windows: 530.
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
        /// Gets control where caret is located.
        /// </summary>
        public virtual AbstractControl? Control
        {
            get
            {
                return control?.Value;
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
                if (Position == value)
                    return;
                Handler.Position = value;
                var sender = Control;
                if (sender is null)
                    return;
                ControlPositionChanged?.Invoke(sender, EventArgs.Empty);
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
                if (Visible == value)
                    return;

                if (!Handler.IsOk)
                    return;

                try
                {
                    if (control?.Value?.ContainsFocus ?? false)
                    {
                        Handler.Visible = value;
                    }
                    else
                    {
                        Handler.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    Nop(ex);
                }
            }
        }

        /// <summary>
        /// Gets or sets caret's position in device-independent units.
        /// This property returns Null and doesn't change caret's position
        /// if caret is not attached to the control (<see cref="Control"/> property is Null).
        /// </summary>
        public virtual PointD? PositionDip
        {
            get
            {
                var owner = Control;
                if (owner is null)
                    return null;

                var posI = Position;
                var result = owner.PixelToDip(posI);
                return result;
            }

            set
            {
                var owner = Control;
                if (owner is null)
                    return;
                var posI = owner.PixelFromDip(value ?? PointD.Empty);
                Position = posI;
            }
        }

        /// <summary>
        /// Gets caret position for the specified control.
        /// </summary>
        /// <param name="control">Control for which to get the caret position.</param>
        /// <returns></returns>
        public static PointD? GetPosition(AbstractControl? control)
        {
            if (control is null)
                return null;

            var caret = control.Caret;
            var result = caret?.PositionDip;
            return result;
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
            var c = Control;
            if(c is null || size is null)
                return new PlessCaretHandler();
            return App.Handler.CreateCaretHandler(c, size.Value.Width, size.Value.Height);
        }
    }
}