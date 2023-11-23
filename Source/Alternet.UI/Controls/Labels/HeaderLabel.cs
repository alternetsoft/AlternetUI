using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements label control that looks like header.
    /// </summary>
    public class HeaderLabel : CustomLabel
    {
        private CardPanelHeader headerPanel = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderLabel"/> class.
        /// </summary>
        public HeaderLabel()
        {
            headerPanel.Parent = this;
        }
        
        /// <inheritdoc/>
        public override string Text
        {
            get
            {
                return headerPanel.Text;
            }

            set
            {
                headerPanel.Text = value;
            }
        }
    }
}
