using Alternet.UI;
using System;

namespace Alternet.UI.Documentation.Examples.TabControl
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TabControlExample1();
        }

        public void TabControlExample1()
        {
            #region TabControlCSharpCreation
            var tc = new Alternet.UI.TabControl();
            tc.SuggestedSize = (250, 300);
            tc.Pages.Add(new TabPage { Title = "Page 1"});
            tc.Pages.Add(new TabPage { Title = "Page 2" });
            tc.Pages.Add(new TabPage { Title = "Page 3" });
            tc.Pages.Add(new TabPage { Title = "Page 4" });
            tc.Pages.Add(new TabPage { Title = "Page 5" });
            tc.Parent = mainPanel;
            #endregion
        }
    }
}