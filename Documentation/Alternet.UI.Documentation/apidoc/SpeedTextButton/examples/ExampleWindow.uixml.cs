using Alternet.UI;
using System;

namespace Alternet.UI.Documentation.Examples
{
    public partial class MainWindow : Window
    {
        private readonly SpeedButton button2;

        public MainWindow()
        {
            InitializeComponent();
            logListBox.BindApplicationLog();
            button2 = CreateSpeedButton();
            KeyDown += MainWindow_KeyDown;
        }

        #region CSharpCreation
        public SpeedButton CreateSpeedButton()
        {
            SpeedTextButton result = new();
            result.Click += Button_Click;
            result.Text = "Cancel";
            result.ToolTip = "Some hint";
            result.HorizontalAlignment = HorizontalAlignment.Center;
            result.Parent = mainPanel;
            result.ShortcutKeys = Keys.Escape;
            result.Margin = 10;
            return result;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            // Shortcuts in SpeedButton are not handled by default, they are only shown in hint,
            // so here we need to handle speedbutton shortcut.
            if (e.KeyData == button2.ShortcutKeys)
            {
                Application.Log("button2 shortcut pressed");
                e.Handled = true;
            }

            if (e.KeyData == button1.ShortcutKeys)
            {
                Application.Log("button1 shortcut pressed");
                e.Handled = true;
            }
        }
        #endregion

        private void Button_Click(object? sender, EventArgs e)
        {
            Application.Log("SpeedButton.Click");
        }
    }
}