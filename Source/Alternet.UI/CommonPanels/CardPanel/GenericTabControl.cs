using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class GenericTabControl : Control
    {
        private readonly CardPanel cardPanel = new();
        private readonly CardPanelHeader cardPanelHeader = new();

        public GenericTabControl()
            : base()
        {
        }

        public CardPanel CardPanel => cardPanel;

        public CardPanelHeader CardPanelHeader => cardPanelHeader;
    }
}
