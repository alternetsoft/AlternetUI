using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public partial interface IControlHandler
    {
        Action? GotFocus { get; set; }

        Action? LostFocus { get; set; }

        bool IsFocused { get; }

        void SetFocusFlags(bool canSelect, bool tabStop, bool acceptsFocusRecursively);

        void FocusNextControl(bool forward = true, bool nested = true);

        bool SetFocus();
    }
}
