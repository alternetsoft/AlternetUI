using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements default button for the <see cref="CardPanelHeader"/> control.
    /// </summary>
    public class CardPanelHeaderButton : CardPanelCustomButton
    {
        private readonly Button button = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="CardPanelHeaderButton"/> class.
        /// </summary>
        public CardPanelHeaderButton()
        {
            button.Parent = this;    
        }

        /// <inheritdoc/>
        public override event EventHandler? Click
        {
            add => button.Click+=value;
            remove => button.Click -= value;
        }

        /// <inheritdoc/>
        public override string Text
        {
            get
            {
                return button.Text;
            }

            set
            {
                button.Text = value;
            }
        }

        /// <inheritdoc/>
        public override bool HasBorder
        {
            get
            {
                return button.HasBorder;
            }

            set
            {
                button.HasBorder = value;
            }
        }

        /// <inheritdoc/>
        public override Color? ForegroundColor
        {
            get => base.ForegroundColor;

            set
            {
                if (value == ForegroundColor)
                    return;
                base.ForegroundColor = value;
                button.ForegroundColor = value;
            }
        }

        /// <inheritdoc/>
        public override Color? BackgroundColor
        {
            get => base.BackgroundColor;

            set
            {
                if (value == BackgroundColor)
                    return;                    
                base.BackgroundColor = value;
                button.BackgroundColor = value;
            }
        }
    }
}
