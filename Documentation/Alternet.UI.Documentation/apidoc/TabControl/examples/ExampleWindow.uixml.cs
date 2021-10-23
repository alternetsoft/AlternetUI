using Alternet.UI;
using System;

namespace Alternet.UI.Documentation.Examples.TabControl
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void TabControlExample1()
        {
            #region TabControlCSharpCreation
            var tc = new Alternet.UI.TabControl();
            tc.Pages.Add(new TabPage { Title = "Grid"});
            tc.Pages.Add(new TabPage { Title = "Tree View" });
            tc.Pages.Add(new TabPage { Title = "List View" });
            tc.Pages.Add(new TabPage { Title = "List Box" });
            tc.Pages.Add(new TabPage { Title = "Combo Box" });
            #endregion
        }

        #region TabControlEventHandler
        private void TabControl_EnabledChanged(object? sender, EventArgs e)
        {
            var text = tabControl.Enabled.ToString();
            MessageBox.Show(text, string.Empty);
        }

        #endregion    
    }
}