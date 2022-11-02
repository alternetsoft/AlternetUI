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
            "ControlsSample.Resources.ImageListIcons.Small.Pencil16.png") ?? throw new Exception());

        NotifyIcon notifyIcon = new NotifyIcon { Icon = Image, Text = "MYTEXT^" };

        private void NotifyIconVisibleCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            notifyIcon.Visible = notifyIconVisibleCheckBox.IsChecked;
        }
    }
}