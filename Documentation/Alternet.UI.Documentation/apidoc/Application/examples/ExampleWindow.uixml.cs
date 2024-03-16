using System;
using Alternet.UI;

namespace Alternet.UI.Documentation.Examples.Application
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Alternet.UI.Application.LogMessage += Application_LogMessage;
            logItemButton.Click += LogItemButton_Click;
        }

        private void LogItemButton_Click(object? sender, EventArgs e)
        {
            UI.Application.Log("Helllo!");
        }

        public void ApplicationExample1()
        {
            #region ApplicationCSharpCreation
            var application = new Alternet.UI.Application();
            var window = new MainWindow();
            Alternet.UI.Application.LogMessage += Application_LogMessage;
            application.Run(window);
            #endregion

        }

        #region ApplicationEventHandler
        private void Application_LogMessage(object? sender, LogMessageEventArgs e)
        {
            listBox.Log(e.Message);
        }
        #endregion
    }
}