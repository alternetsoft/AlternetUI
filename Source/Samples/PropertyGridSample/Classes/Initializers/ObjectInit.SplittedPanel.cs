using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI;

namespace PropertyGridSample
{
    public partial class ObjectInit
    {
        public static void InitSplittedPanel(object control)
        {
            if (control is not SplittedPanel panel)
                return;
            panel.SuggestedSize = 300;

            StdListBox LeftLabel = new()
            {
                Parent = panel.LeftPanel,
            };
            LeftLabel.Add("Left");

            StdListBox RightLabel = new()
            {
                Parent = panel.RightPanel,
            };
            RightLabel.Add("Right");

            ToolBar toolbar = new();
            toolbar.Parent = panel.TopPanel;
            InitGenericToolBar(toolbar);
            panel.TopSplitter.Visible = false;
            panel.TopPanel.MinHeight = toolbar.ItemSize + 6;

            StdListBox BottomLabel = new()
            {
                Parent = panel.BottomPanel,
            };
            BottomLabel.Add("Bottom");

            StdListBox FillLabel = new()
            {
                Parent = panel.FillPanel,
            };
            FillLabel.Add("Fill");

            panel.RightPanel.Width = 80;
            panel.LeftPanel.Width = 80;
        }
    }
}
