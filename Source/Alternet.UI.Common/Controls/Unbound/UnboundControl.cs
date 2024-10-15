using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    internal class UnboundControl : Control
    {
        public static bool UseUnboundControls = true;

        public static IControlHandler CreateUnboundControlHandler(AbstractControl control)
        {
            if(UseUnboundControls)
                return new PlessControlHandler();
            return ControlFactory.Handler.CreateControlHandler(control);
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return CreateUnboundControlHandler(this);
        }
    }
}
