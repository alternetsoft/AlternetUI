using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public abstract class DateTimePickerHandler : ControlHandler
    {
        /// <inheritdoc/>
        public new DateTimePicker Control => (DateTimePicker)base.Control;

        /// <inheritdoc/>
        //protected override bool VisualChildNeedsNativeControl => true;
    }
}
