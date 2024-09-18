using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if MACCATALYST
using Foundation;

using SkiaSharp;

using UIKit;
#endif

namespace Alternet.UI
{
#if MACCATALYST
    /// <summary>
    /// Adds additional functionality to the <see cref="SkiaSharp.Views.iOS.SKCanvasView"/> control.
    /// </summary>
    public partial class SKCanvasViewAdv : SkiaSharp.Views.iOS.SKCanvasView
    {
        private static readonly Lazy<bool> isValidEnvironment = new(() =>
        {
            try
            {
                SKPMColor.PreMultiply(SKColors.Black);
                return true;
            }
            catch (DllNotFoundException)
            {
                return false;
            }
        });

        /// <summary>
        /// Initializes a new instance of the <see cref="SKCanvasViewAdv"/> class.
        /// </summary>
        /// <param name="handle"></param>
        public SKCanvasViewAdv(IntPtr handle)
            : base(handle)
        {
        }

        internal static bool IsValidEnvironment => isValidEnvironment.Value;

        /// <inheritdoc/>
        public override bool CanBecomeFocused
        {
            get
            {
                return true;
            }
        }

        /// <inheritdoc/>
        public override void PressesBegan(NSSet<UIPress> presses, UIPressesEvent evt)
        {
            var handled = false;

            foreach (UIPress press in presses)
            {
                var pressType = press.Type;

                // Was the Touch Surface clicked?
                if (press.Type == UIPressType.Select)
                {
                }

                if (press.Key is null)
                    continue;

                /*
                    public enum UIKeyModifierFlags : long
                    {
                        AlphaShift = 65536,
                        Shift = 131072,
                        Control = 262144,
                        Alternate = 524288,
                        Command = 1048576,
                        NumericPad = 2097152
                    }

                    static var alphaShift: UIKeyModifierFlags
                        A modifier flag that indicates the user pressed the Caps Lock key.
                    static var shift: UIKeyModifierFlags
                        A modifier flag that indicates the user pressed the Shift key.
                    static var control: UIKeyModifierFlags
                        A modifier flag that indicates the user pressed the Control key.
                    static var alternate: UIKeyModifierFlags
                        A modifier flag that indicates the user pressed the Option key.
                    static var command: UIKeyModifierFlags
                        A modifier flag that indicates the user pressed the Command key.
                    static var numericPad: UIKeyModifierFlags
                        A modifier flag that indicates the user pressed a key located on the numeric keypad.
                */

                var shift =
                    press.Key.ModifierFlags.HasFlag(UIKeyModifierFlags.AlphaShift) ||
                    press.Key.ModifierFlags.HasFlag(UIKeyModifierFlags.Shift);

                var keyCode = press.Key.KeyCode;

                if (keyCode == UIKeyboardHidUsage.KeyboardTab)
                {
                }

                if (!handled)
                {
                    base.PressesBegan(presses, evt);
                }
            }
        }

        /// <inheritdoc/>
        public override void PressesCancelled(NSSet<UIPress> presses, UIPressesEvent evt)
        {
            base.PressesCancelled(presses, evt);
        }

        /// <inheritdoc/>
        public override void PressesChanged(NSSet<UIPress> presses, UIPressesEvent evt)
        {
            base.PressesChanged(presses, evt);
        }

        /// <inheritdoc/>
        public override void PressesEnded(NSSet<UIPress> presses, UIPressesEvent evt)
        {
            base.PressesEnded(presses, evt);
        }
    }
#endif
}
