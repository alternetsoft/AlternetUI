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
    public class CardPanelHeaderItem : BaseControlItem
    {
        private readonly CardPanelCustomButton headerControl;
        private Control? cardControl;
        private ObjectUniqueId? cardUniqueId;

        internal CardPanelHeaderItem(CardPanelCustomButton headerControl)
        {
            this.headerControl = headerControl;
        }

        /// <summary>
        /// Gets associated header control.
        /// </summary>
        public CardPanelCustomButton HeaderButton => headerControl;

        /// <summary>
        /// Gets or sets associated card control.
        /// </summary>
        public Control? CardControl
        {
            get => cardControl;
            set => cardControl = value;
        }

        /// <summary>
        /// Gets or sets associated <see cref="CardPanelItem"/> unique id.
        /// </summary>
        public ObjectUniqueId? CardUniqueId
        {
            get => cardUniqueId;
            set => cardUniqueId = value;
        }
    }
}
