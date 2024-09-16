using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    internal class UnboundGenericLabel : GenericLabel
    {
        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return UnboundControl.CreateUnboundControlHandler(this);
        }
    }
}
