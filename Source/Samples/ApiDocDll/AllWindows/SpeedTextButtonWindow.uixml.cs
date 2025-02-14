using Alternet.UI;
using System;

namespace ApiDoc
{
    public partial class SpeedTextButtonWindow : Window
    {
        private readonly SpeedButton button2;

        public SpeedTextButtonWindow()
        {
            InitializeComponent();
            logListBox.BindApplicationLog();
            button2 = CreateSpeedButton();
            KeyDown += MainWindow_KeyDown;

            button1.UseTheme = SpeedButton.KnownTheme.StaticBorder;

            Group(button1, button2).Padding(10).MinWidth(50)
                .HorizontalAlignment(HorizontalAlignment.Center);
        }

        #region CSharpCreation
        public SpeedButton CreateSpeedButton()
        {
            SpeedTextButton result = new();
            result.Click += Button_Click;
            result.Text = "Cancel";
            result.ToolTip = "Some hint";
            result.Parent = buttonPanel;
            result.ShortcutKeys = Keys.Escape;
            result.Margin = 5;
            result.UseTheme = SpeedButton.KnownTheme.StaticBorder;
            return result;
        }

        private void MainWindow_KeyDown(object? sender, KeyEventArgs e)
        {
            // Shortcuts in SpeedButton are not handled by default, they are only shown in hint,
            // so here we need to handle speedbutton shortcut.
            if (e.KeyData == button2.ShortcutKeys)
            {
                App.Log("button2 shortcut pressed");
                e.Handled = true;
            }

            if (e.KeyData == button1.ShortcutKeys)
            {
                App.Log("button1 shortcut pressed");
                e.Handled = true;
            }
        }
        #endregion

        private void Button_Click(object? sender, EventArgs e)
        {
            App.Log("SpeedButton.Click");
        }
    }
}