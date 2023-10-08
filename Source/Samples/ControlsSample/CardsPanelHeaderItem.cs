using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class CardsPanelHeaderItem : BaseObject
    {
        private readonly Control headerControl;
        private Control? cardControl;

        internal CardsPanelHeaderItem(Control headerControl)
        {
            this.headerControl = headerControl;
        }

        public Control HeaderControl => headerControl;

        public Control? CardControl
        {
            get => cardControl;
            set => cardControl = value;
        }
    }
}
