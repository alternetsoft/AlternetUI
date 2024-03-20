using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace ControlsSample
{
    internal class PreviewSampleWindow : Window
    {
        private readonly SplittedPanel panel = new()
        {
            TopVisible = false,
            RightVisible = false,
        };

        private readonly FileListBox fileListBox = new()
        {
            HasBorder = false,
        };

        private readonly PreviewUixml preview = new()
        {

        };

        private readonly LogListBox logListBox = new()
        {
            HasBorder = false,
        };

        public PreviewSampleWindow()
        {
            Margin = 10;
            Icon = Application.DefaultIcon;
            Title = "Alternet.UI Preview File Sample";
            Size = (900, 700);
            StartLocation = WindowStartLocation.CenterScreen;
            panel.LeftPanel.Width = 300;
            panel.BottomPanel.Height = 200;
            panel.Parent = this;
            fileListBox.Parent = panel.LeftPanel;
            preview.Parent = panel.FillPanel;
            logListBox.Parent = panel.BottomPanel;
            fileListBox.AddSpecialFolders();
            logListBox.Log("Select uixml or other supported file and it will be previewed");
            logListBox.Log("This demo is under development.");
        }
    }
}
