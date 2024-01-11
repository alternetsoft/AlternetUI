using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements custom abstract button.
    /// </summary>
    public abstract class CustomButton : Control, ITextProperty
    {
        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        [Browsable(false)]
        public virtual bool HasBorder
        {
            get
            {
                return false;
            }

            set
            {
            }
        }
    }
}
