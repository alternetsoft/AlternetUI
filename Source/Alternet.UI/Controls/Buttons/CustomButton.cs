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
        /// Gets or sets function which creates button for the <see cref="CardPanelHeader"/>.
        /// </summary>
        public static Func<CustomButton>? CreateButton;

        /// <summary>
        /// Gets or sets the text displayed on this button.
        /// </summary>
        public abstract string Text { get; set; }

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
