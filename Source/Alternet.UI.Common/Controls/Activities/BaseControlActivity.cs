using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Base class for all control activities.
    /// </summary>
    public class BaseControlActivity : ControlNotification
    {
        /// <summary>
        /// Initializes activity for the specified control.
        /// </summary>
        /// <param name="control">The control for which to perform initialization.</param>
        public virtual void Initialize(AbstractControl control)
        {
            control.AddNotification(this);
        }
    }
}
