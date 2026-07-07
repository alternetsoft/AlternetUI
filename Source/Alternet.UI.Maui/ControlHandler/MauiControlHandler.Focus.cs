using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal partial class MauiControlHandler
    {
        private readonly bool canSelect = true;

        public virtual bool CanSelect
        {
            get => canSelect;
        }

        public virtual bool IsFocused
        {
            get
            {
                var c = (container as ControlView)?.Control;

                if (c is null)
                    return false;

                return AbstractControl.FocusedControlEquals(c);
            }

            set
            {
                if (IsFocused == value)
                    return;
                AbstractControl.FocusedControl?.RaiseLostFocus(LostFocusEventArgs.Empty);
                AbstractControl.FocusedControl = (container as ControlView)?.Control;
                AbstractControl.FocusedControl?.RaiseGotFocus(GotFocusEventArgs.Empty);
            }
        }

        public virtual bool SetFocus()
        {
            if (container is null)
                return false;
            (container as ControlView)?.SetFocusIfPossible();
            return (container as ControlView)?.IsFocused ?? false;
        }
    }
}
