using Alternet.UI;
using System;

namespace Alternet.UI.Documentation.Examples.Application
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public void ApplicationExample1()
        {
            #region ApplicationCSharpCreation
            var application = new Alternet.UI.Application();
            var window = new MainWindow();
            application.VisualThemeChanged += Application_VisualThemeChanged;
            #endregion

        }


        #region ApplicationEventHandler
        private void Application_VisualThemeChanged(object? sender, EventArgs e)
        {
            MessageBox.Show("Visual Theme changed", string.Empty);
        }
        #endregion
    }
}