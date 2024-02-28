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
            button1.ImageSet =
                KnownSvgImages.GetForSize(button1.GetSvgColor(KnownSvgColor.Normal), 32).ImgOk;
            button2 = CreateSpeedButton();
        }

        #region CSharpCreation
        public SpeedButton CreateSpeedButton()
        {
            SpeedButton result = new();

            // This call sets image and disabled image to the known svg image with the
            // specified size and color
            result.ImageSet
                = KnownSvgImages.GetForSize(result.GetSvgColor(KnownSvgColor.Normal), 32).ImgCancel;
            result.DisabledImageSet
                = KnownSvgImages.GetForSize(result.GetSvgColor(KnownSvgColor.Disabled), 32).ImgCancel;
            
            result.Click += Button_Click;
            result.ToolTip = "Some hint";
            result.HorizontalAlignment = HorizontalAlignment.Center;
            result.Parent = mainPanel;
            result.ShortcutKeys = Keys.Control | Keys.A;
            result.Name = "cancelBtn";
            return result;
        }
        #endregion

        private void Button_Click(object? sender, EventArgs e)
        {
            Application.Log($"SpeedButton '{(sender as Control)?.Name}' Click");
        }
    }
}