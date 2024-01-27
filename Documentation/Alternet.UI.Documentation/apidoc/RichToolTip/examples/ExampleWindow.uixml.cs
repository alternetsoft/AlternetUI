using System;
using Alternet.UI;
using Alternet.Drawing;

namespace Alternet.UI.Documentation.Examples
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region CSharpCreation
        public void ShowToolTip_Click(object? sender, EventArgs e)
        {
            RichToolTip.Show(
                "Title",
                "Some text",
                label1,
                RichToolTipKind.Top,
                MessageBoxIcon.Exclamation);
        }

        public void HideToolTip_Click(object? sender, EventArgs e)
        {
            RichToolTip.Default = null;
        }

        public void ShowToolTip2_Click(object? sender, EventArgs e)
        {
            var tooltip = new RichToolTip("Another Title", "Some text");
            tooltip.SetIcon(MessageBoxIcon.Error);
            tooltip.SetTipKind(RichToolTipKind.None);
            tooltip.SetBackgroundColor(Color.Red, null);
            tooltip.SetForegroundColor(Color.White);
            tooltip.SetTitleFont(Control.DefaultFont.Larger().AsBold);
            tooltip.SetTitleForegroundColor(Color.White);

            // This call hides previous tooltip after call to RichToolTip.Show
            // and assigns new tooltip
            RichToolTip.Default = tooltip;

            tooltip.Show(label1);
        }
        #endregion
    }
}