using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class AuiToolbar : Control
    {
        /// <inheritdoc/>
        public override ControlId ControlKind => ControlId.AuiToolbar;

        internal new NativeAuiToolbarHandler Handler
        {
            get
            {
                CheckDisposed();
                return (NativeAuiToolbarHandler)base.Handler;
            }
        }

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().CreateAuiToolbarHandler(this);
        }
    }
}
