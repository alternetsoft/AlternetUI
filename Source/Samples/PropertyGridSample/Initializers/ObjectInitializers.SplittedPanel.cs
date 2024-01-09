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

            ListBox LeftLabel = new()
            {
                Parent = panel.LeftPanel,
            };
            LeftLabel.Add("Left");

            ListBox RightLabel = new()
            {
                Parent = panel.RightPanel,
            };
            RightLabel.Add("Right");

            GenericToolBar toolbar = new();
            toolbar.Parent = panel.TopPanel;
            InitGenericToolBar(toolbar);
            panel.TopSplitter.Visible = false;
            panel.TopPanel.Height = toolbar.ItemSize + 6;

            ListBox BottomLabel = new()
            {
                Parent = panel.BottomPanel,
            };
            BottomLabel.Add("Bottom");

            ListBox FillLabel = new()
            {
                Parent = panel.FillPanel,
            };
            FillLabel.Add("Fill");
        }
    }
}
