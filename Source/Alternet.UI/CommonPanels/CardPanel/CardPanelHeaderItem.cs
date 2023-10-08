using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Individual page of the <see cref="CardPanelHeader"/>
    /// </summary>
    public class CardPanelHeaderItem : BaseObject
    {
        private readonly Control headerControl;
        private Control? cardControl;

        internal CardPanelHeaderItem(Control headerControl)
        {
            this.headerControl = headerControl;
        }

        /// <summary>
        /// Gets associated header control.
        /// </summary>
        public Control HeaderControl => headerControl;

        /// <summary>
        /// Gets or sets associated card control.
        /// </summary>
        public Control? CardControl
        {
            get => cardControl;
            set => cardControl = value;
        }
    }
}
