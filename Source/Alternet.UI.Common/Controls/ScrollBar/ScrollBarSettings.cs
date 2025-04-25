using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the settings for a scrollbar, including visibility configuration.
    /// </summary>
    public partial class ScrollBarSettings : BaseObjectWithNotify
    {
        private HiddenOrVisible visibility;

        /// <summary>
        /// Gets or sets the suggested visibility for the scrollbar.
        /// </summary>
        /// <value>
        /// A <see cref="HiddenOrVisible"/> value indicating the recommended visibility state.
        /// </value>
        public virtual HiddenOrVisible SuggestedVisibility
        {
            get
            {
                return visibility;
            }

            set
            {
                SetProperty(ref visibility, value);
            }
        }
    }
}