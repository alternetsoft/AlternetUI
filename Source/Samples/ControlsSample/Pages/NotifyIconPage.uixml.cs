using System;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class NotifyIconPage : Panel
    {
        public static readonly Image Image =
            new Bitmap(typeof(NotifyIconPage).Assembly.GetManifestResourceStream(
            "ControlsSampleDll.Resources.Logo16x16.png") ?? throw new Exception());

        private readonly NotifyIcon? notifyIcon;

        public NotifyIconPage()
        {
            InitializeComponent();

            notifyIcon = new NotifyIcon
            {
                Icon = Image,
                Text = notifyIconTextTextBox.Text,
            };

            notifyIcon.Click += NotifyIcon_Click;

            notifyIcon.RightMouseButtonDoubleClick
                += (s, e) => App.Log("NotifyIcon: RightMouseButtonDoubleClick");

            notifyIcon.RightMouseButtonDown += (s, e) => App.Log("NotifyIcon: RightMouseButtonDown");
            notifyIcon.RightMouseButtonUp += (s, e) => App.Log("NotifyIcon: RightMouseButtonUp");

            notifyIcon.LeftMouseButtonDoubleClick
                += (s, e) => App.Log("NotifyIcon: LeftMouseButtonDoubleClick");

            notifyIcon.LeftMouseButtonDown += (s, e) => App.Log("NotifyIcon: LeftMouseButtonDown");
            notifyIcon.LeftMouseButtonUp += (s, e) => App.Log("NotifyIcon: LeftMouseButtonUp");

        notifyIcon.Menu = new ExampleContextMenu();

            mainStackPanel.UseInternalContextMenu = true;

            mainStackPanel.ContextMenuStrip.Add("Toggle first context menu item enabled", () =>
            {
                if (notifyIcon?.Menu?.Items.Count > 0)
                    notifyIcon.Menu.Items[0].Enabled = !notifyIcon.Menu.Items[0].Enabled;
            });
        }

        private void NotifyIcon_Click(object? sender, EventArgs e)
        {
            App.Log("NotifyIcon: Click");
        }

        private void NotifyIconVisibleCheckBox_CheckedChanged(object sender, EventArgs e)
        {   
            if(notifyIcon is not null)
                notifyIcon.Visible = notifyIconVisibleCheckBox.IsChecked;
        }

        private void ApplyTextButton_Click(object sender, System.EventArgs e)
        {
            if (notifyIcon is not null)
                notifyIcon.Text = notifyIconTextTextBox.Text;
        }
    }
}