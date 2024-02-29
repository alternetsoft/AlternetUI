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

            button1.LoadSvg(KnownSvgUrls.UrlImageOk, 32);
            button2 = CreateSpeedButton();
        }

        #region CSharpCreation
        public SpeedButton CreateSpeedButton()
        {
            SpeedButton result = new()
            {
                Text = "Cancel",
                TextVisible = true,
                ToolTip = "Some hint",
                HorizontalAlignment = HorizontalAlignment.Center,
                Parent = buttonPanel,
                ShortcutKeys = Keys.Control | Keys.A,
                Name = "cancelBtn",
            };
            result.HorizontalAlignment = HorizontalAlignment.Right;
            result.LoadSvg(KnownSvgUrls.UrlImageCancel, 32);
            result.Click += Button_Click;
            return result;
        }
        #endregion

        private void Button_Click(object? sender, EventArgs e)
        {
            Application.Log($"SpeedButton '{(sender as Control)?.Name}' Click");
        }
    }
}