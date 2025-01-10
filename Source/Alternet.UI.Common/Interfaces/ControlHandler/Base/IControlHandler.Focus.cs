using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public partial interface IControlHandler
    {
        /// <inheritdoc cref="AbstractControl.CanSelect"/>
        bool CanSelect { get; }

        /// <inheritdoc cref="AbstractControl.Focused"/>
        bool IsFocused { get; }

        /// <inheritdoc cref="AbstractControl.SetFocus"/>
        bool SetFocus();

        /// <inheritdoc cref="AbstractControl.UpdateFocusFlags"/>
        void UpdateFocusFlags(bool canSelect, bool tabStop);
    }
}
