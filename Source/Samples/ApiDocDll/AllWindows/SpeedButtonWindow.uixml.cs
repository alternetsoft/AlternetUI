using Alternet.UI;
using System;

namespace ApiDoc
{
    public partial class SpeedButtonWindow : Window
    {
        public SpeedButtonWindow()
        {
            InitializeComponent();
            logListBox.BindApplicationLog();

            button1.ImageSet = KnownSvgImages.ImgOk.AsNormal(32, button1.IsDarkBackground);
            button1.DisabledImageSet = KnownSvgImages.ImgOk.AsDisabled(32, button1.IsDarkBackground);
            CreateSpeedButton();
        }

        #region CSharpCreation
        public SpeedButton CreateSpeedButton()
        {
            SpeedButton result = new()
            {
                Text = "Cancel",
                TextVisible = true,
                ToolTip = "Some hint",
                Parent = buttonPanel,
                ShortcutKeys = Keys.Control | Keys.A,
                Name = "cancelBtn",
                Enabled = false,
                HorizontalAlignment = HorizontalAlignment.Right,
            };
            result.ImageSet = KnownSvgImages.ImgCancel.AsNormal(32, result.IsDarkBackground);
            result.DisabledImageSet = KnownSvgImages.ImgCancel.AsDisabled(32, result.IsDarkBackground);
            result.Click += Button_Click;
            return result;
        }
        #endregion

        private void Button_Click(object? sender, EventArgs e)
        {
            App.Log($"SpeedButton '{(sender as Control)?.Name}' Click");
        }
    }
}