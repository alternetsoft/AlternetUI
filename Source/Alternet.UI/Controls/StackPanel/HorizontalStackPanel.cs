using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class HorizontalStackPanel : StackPanel
    {
        public HorizontalStackPanel()
            : base()
        {
        }

        public override StackPanelOrientation Orientation
        {
            get => StackPanelOrientation.Horizontal;

            set
            {
            }
        }
    }
}
