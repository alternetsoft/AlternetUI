using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if MACCATALYST
using Foundation;
using UIKit;
#endif

namespace Alternet.UI
{
#if MACCATALYST
    public partial class EventView : UIView
    {
        public EventView(IntPtr handle)
            : base(handle)
        {
        }

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
