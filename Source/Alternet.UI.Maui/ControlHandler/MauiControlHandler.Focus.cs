using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal partial class MauiControlHandler
    {
        private bool canSelect = true;

        public virtual bool CanSelect
        {
            get => canSelect;
        }

        public virtual bool IsFocused
        {
            get
            {
                var c = container?.Control;

                if (c is null)
                    return false;

                return AbstractControl.FocusedControlEquals(c);
            }

            set
            {
                if (IsFocused == value)
                    return;
                AbstractControl.FocusedControl?.RaiseLostFocus(LostFocusEventArgs.Empty);
                AbstractControl.FocusedControl = container?.Control;
                AbstractControl.FocusedControl?.RaiseGotFocus(GotFocusEventArgs.Empty);
            }
        }

        public virtual bool SetFocus()
        {
            if (container is null)
                return false;
            container.SetFocusIfPossible();
            return container.IsFocused;
        }
    }
}
