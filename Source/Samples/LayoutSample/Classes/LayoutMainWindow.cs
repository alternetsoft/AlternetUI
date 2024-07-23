using System;
using Alternet.UI;
using LayoutSample;

namespace LayoutSample
{
    public partial class LayoutMainWindow : Window
    {
        public LayoutMainWindow()
        {
            MinimumSize = (600, 600);

		var control = new LayoutMainControl();
		control.Parent = this;

            SetSizeToContent();
        }
    }
}