﻿using System;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class NotifyIconPage : Control
    {
        public static readonly Image Image =
            new Bitmap(typeof(NotifyIconPage).Assembly.GetManifestResourceStream(
            "ControlsSampleDll.Resources.Logo16x16.png") ?? throw new Exception());

        private readonly NotifyIcon? notifyIcon;

        public NotifyIconPage()
        {
            InitializeComponent();

                notifyIcon = new NotifyIcon { Icon = Image, Text = notifyIconTextTextBox.Text };
                notifyIcon.Click += NotifyIcon_Click;
                notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
                notifyIcon.Menu = new ExampleContextMenu();
        }

        private void NotifyIcon_DoubleClick(object? sender, EventArgs e)
        {
            App.Log("NotifyIcon: DoubleClick");
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