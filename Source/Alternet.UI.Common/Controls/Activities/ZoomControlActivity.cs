using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Implements "Zoom in" and "Zoom out" using Ctrl+Shift+Plus and Ctrl+Shift+Minus keys.
    /// In order to use this activity correctly,
    /// set True to <see cref="AbstractControl.DefaultUseParentFont"/> before creating any controls.
    /// </summary>
    public class ZoomControlActivity : BaseControlActivity
    {
        /// <inheritdoc/>
        public override void Initialize(AbstractControl control)
        {
            base.Initialize(control);
            if (control is Window window)
                window.KeyPreview = true;
        }

        /// <inheritdoc/>
        public override void AfterKeyDown(AbstractControl sender, KeyEventArgs e)
        {
            base.AfterKeyDown(sender, e);

            if (!e.ControlAndShift)
                return;

            if (e.IsAnyPlusKey)
            {
                App.DebugLogIf("Pressed Ctrl+Shift+Plus: Zoom In Font", false);
                sender.Font = sender.RealFont.Larger();
                e.Suppressed();
            }

            if (e.IsAnyMinusKey)
            {
                App.DebugLogIf("Pressed Ctrl+Shift+Minus: Zoom Out Font", false);
                sender.Font = sender.RealFont.Smaller();
                e.Suppressed();
            }
        }
    }
}
