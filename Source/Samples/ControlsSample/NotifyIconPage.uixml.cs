using Alternet.Drawing;
using Alternet.UI;
using System;

namespace ControlsSample
{
    internal partial class NotifyIconPage : Control
    {
        public static readonly Image Image = new Image(typeof(NotifyIconPage).Assembly.GetManifestResourceStream(
            "ControlsSample.Resources.Logo16x16.png") ?? throw new Exception());

        private IPageSite? site;

        private NotifyIcon notifyIcon;

        public NotifyIconPage()
        {
            InitializeComponent();

            notifyIcon = new NotifyIcon { Icon = Image, Text = notifyIconTextTextBox.Text };
            notifyIcon.Click += NotifyIcon_Click;
            notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
            notifyIcon.Menu = new ExampleContextMenu();
        }

        public IPageSite? Site
        {
            get => site;

            set
            {
                site = value;
            }
        }

        private void NotifyIcon_DoubleClick(object? sender, EventArgs e)
        {
            site?.LogEvent("NotifyIcon: DoubleClick");
        }

        private void NotifyIcon_Click(object? sender, EventArgs e)
        {
            site?.LogEvent("NotifyIcon: Click");
        }

        private void NotifyIconVisibleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            notifyIcon.Visible = notifyIconVisibleCheckBox.IsChecked;
        }

        private void ApplyTextButton_Click(object sender, System.EventArgs e)
        {
            notifyIcon.Text = notifyIconTextTextBox.Text;
        }
    }
}