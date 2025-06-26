using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the <see cref="ControlAndButton.ButtonClick"/> event.
    /// </summary>
    public class ControlAndButtonClickEventArgs : HandledEventArgs
    {
        /// <summary>
        /// Gets id of the clicked button.
        /// </summary>
        public virtual ObjectUniqueId? ButtonId { get; set; }

        /// <summary>
        /// Determines if the button is a combo button.
        /// </summary>
        public virtual bool IsButtonCombo(AbstractControl buttonOwner)
        {
            if(buttonOwner is ControlAndButton controlAndButton)
                return ButtonId == controlAndButton.IdButtonCombo;
            return false;
        }

        /// <summary>
        /// Determines if the button is a plus button.
        /// </summary>
        public virtual bool IsButtonPlus(AbstractControl buttonOwner)
        {
            if(buttonOwner is ControlAndButton controlAndButton)
                return ButtonId == controlAndButton.IdButtonPlus;
            return false;
        }

        /// <summary>
        /// Determines if the button is a minus button.
        /// </summary>
        public virtual bool IsButtonMinus(AbstractControl buttonOwner)
        {
            if(buttonOwner is ControlAndButton controlAndButton)
                return ButtonId == controlAndButton.IdButtonMinus;
            return false;
        }

        /// <summary>
        /// Determines if the button is an ellipsis button.
        /// </summary>
        public virtual bool IsButtonEllipsis(AbstractControl buttonOwner)
        {
            if(buttonOwner is ControlAndButton controlAndButton)
                return ButtonId == controlAndButton.IdButtonEllipsis;
            return false;
        }
    }
}