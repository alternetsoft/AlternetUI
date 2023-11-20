using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements custom abstract button for the <see cref="CardPanelHeader"/> control.
    /// </summary>
    public abstract class CardPanelCustomButton : Control
    {
        /// <summary>
        /// Gets or sets function which creates button for the <see cref="CardPanelHeader"/>.
        /// </summary>
        public static Func<CardPanelCustomButton>? CreateButton;

        /// <summary>
        /// Gets or sets the text displayed on this button.
        /// </summary>
        public abstract string Text { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        public abstract bool HasBorder { get; set; }
    }
}
