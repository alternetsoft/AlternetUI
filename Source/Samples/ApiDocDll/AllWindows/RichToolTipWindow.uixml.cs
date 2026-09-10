using System;
using Alternet.UI;
using Alternet.Drawing;

namespace ApiDoc
{
    public partial class RichToolTipWindow : Window
    {
        public RichToolTipWindow()
        {
            InitializeComponent();
        }

        #region CSharpCreation
        public void ShowToolTip_Click(object? sender, EventArgs e)
        {
            tooltip.ShowToolTip(
                "Title",
                "Some text",
                MessageBoxIcon.Information);
        }

        public void HideToolTip_Click(object? sender, EventArgs e)
        {
            tooltip.HideToolTip();
        }

        public void ShowToolTip2_Click(object? sender, EventArgs e)
        {
            tooltip.Title = "Another Title";
            tooltip.Text = "Some text";
            tooltip.SetIcon(MessageBoxIcon.Error);
            tooltip.SetToolTipBackgroundColor(LightDarkColors.Red);
            tooltip.SetToolTipForegroundColor(LightDarkColors.White);
            tooltip.SetTitleFont(Control.DefaultFont.Larger().AsBold);
            tooltip.SetTitleForegroundColor(LightDarkColors.White);
            tooltip.ShowToolTip();
        }
        #endregion
    }
}