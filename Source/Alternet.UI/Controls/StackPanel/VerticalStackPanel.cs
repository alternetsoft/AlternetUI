using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class VerticalStackPanel : StackPanel
    {
        public VerticalStackPanel()
            : base()
        {
        }

        public override StackPanelOrientation Orientation
        {
            get => StackPanelOrientation.Vertical;

            set
            {
            }
        }
    }
}
