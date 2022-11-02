using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Linq;

namespace ControlsSample
{
    partial class NotifyIconPage : Control
    {
        private IPageSite? site;

        public NotifyIconPage()
        {
            InitializeComponent();

            notifyIcon = new NotifyIcon { Icon = Image, Text = "AlterNET UI Notify Icon example." };
            notifyIcon.Click += NotifyIcon_Click;
            notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
        }

        private void NotifyIcon_DoubleClick(object? sender, EventArgs e)
        {
            site?.LogEvent("NotifyIcon.DoubleClick");
        }

        private void NotifyIcon_Click(object? sender, EventArgs e)
        {
            site?.LogEvent("NotifyIcon.Click");
        }

        public IPageSite? Site
        {
            get => site;

            set
            {
                site = value;
            }
        }

        public static readonly Image Image = new Image(typeof(NotifyIconPage).Assembly.GetManifestResourceStream(
            "ControlsSample.Resources.Logo16x16.png") ?? throw new Exception());

        NotifyIcon notifyIcon;

        private void NotifyIconVisibleCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            notifyIcon.Visible = notifyIconVisibleCheckBox.IsChecked;
        }
    }
}