using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI;

namespace PropertyGridSample
{
    internal partial class ObjectInitializers
    {
        public static void InitSplittedPanel(object control)
        {
            if (control is not SplittedPanel panel)
                return;
            panel.SuggestedSize = 200;
            var backColor = Color.Cornsilk;
            panel.FillPanel.BackColor = backColor;
            panel.RightPanel.BackColor = backColor;
            panel.LeftPanel.BackColor = backColor;
            panel.TopPanel.BackColor = backColor;
            panel.BottomPanel.BackColor = backColor;

            Label LeftLabel = new("Left")
            {
                Parent = panel.LeftPanel,
            };

            Label RightLabel = new("Right")
            {
                Parent = panel.RightPanel,
            };

            Label TopLabel = new("Top")
            {
                Parent = panel.TopPanel,
            };

            Label BottomLabel = new("Bottom")
            {
                Parent = panel.BottomPanel,
            };

            Label FillLabel = new("Fill")
            {
                Parent = panel.FillPanel,
            };
        }
    }
}
