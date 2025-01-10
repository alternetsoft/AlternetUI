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

        private bool tabStop = true;

        private bool acceptsFocusRecursively = true;

        public virtual bool CanSelect
        {
            get => canSelect;
        }

        public virtual bool TabStop
        {
            get => tabStop;

            set
            {
                tabStop = value;
            }
        }

        public Action? GotFocus { get; set; }

        public Action? LostFocus { get; set; }

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

        public virtual void FocusNextControl(bool forward = true, bool nested = true)
        {
        }

        public virtual void SetFocusFlags(bool canSelect, bool tabStop, bool acceptsFocusRecursively)
        {
            this.canSelect = canSelect;
            this.tabStop = tabStop;
            this.acceptsFocusRecursively = acceptsFocusRecursively;
        }
    }
}
