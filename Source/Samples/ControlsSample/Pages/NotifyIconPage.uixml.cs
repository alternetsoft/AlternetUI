using System;

using Alternet.Drawing;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class NotifyIconPage : Window
    {
        public static readonly Image Image =
            new Bitmap(typeof(ButtonPage).Assembly.GetManifestResourceStream(
            "ControlsSampleDll.Resources.Logo16x16.png") ?? throw new Exception("Image not found"));

        private readonly NotifyIcon? notifyIcon;
        private readonly StackPanel mainStackPanel;
        private readonly XCheckBox notifyIconVisibleCheckBox;
        private readonly Label iconTextLabel;
        private readonly TextBox notifyIconTextTextBox;
        private readonly XButton applyTextButton;

        public NotifyIconPage()
        {
            InitializeComponent();

            Title = "Notify Icon example";

            // Root vertical stack
            mainStackPanel = new StackPanel
            {
                Orientation = StackPanelOrientation.Vertical,
                Padding = new Thickness(10),
                Name = "mainStackPanel"
            };

            // Checkbox
            notifyIconVisibleCheckBox = new XCheckBox
            {
                Text = "Visible In Taskbar",
                IsTextLocalized = true,
                Name = "notifyIconVisibleCheckBox"
            };
            mainStackPanel.Children.Add(notifyIconVisibleCheckBox);

            // Horizontal stack for label + textbox + button
            var horizontalStack = new StackPanel
            {
                Orientation = StackPanelOrientation.Vertical,
                Margin = new Thickness(0, 10, 0, 0)
            };

            iconTextLabel = new Label
            {
                Text = "Icon Text",
                Margin = new Thickness(0, 0, 0, 10),
                IsTextLocalized = true,
                VerticalAlignment = VerticalAlignment.Center,
                Name = "iconTextLabel"
            };
            horizontalStack.Children.Add(iconTextLabel);

            notifyIconTextTextBox = new TextBox
            {
                Text = "AlterNET UI Notify Icon example",
                IsTextLocalized = true,
                Name = "notifyIconTextTextBox",
                Margin = new Thickness(0, 0, 0, 10),
                SuggestedWidth = 250,
                VerticalAlignment = VerticalAlignment.Center
            };
            horizontalStack.Children.Add(notifyIconTextTextBox);

            applyTextButton = new XButton
            {
                Text = "Apply Text",
                Name = "applyTextButton",
                VerticalAlignment = VerticalAlignment.Center,
                IsTextLocalized = true
            };
            applyTextButton.Click += ApplyTextButton_Click;
            horizontalStack.Children.Add(applyTextButton);

            mainStackPanel.Children.Add(horizontalStack);

            mainStackPanel.Parent = this;

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

            notifyIconVisibleCheckBox.CheckedChanged += NotifyIconVisibleCheckBox_CheckedChanged;
        }

        private void NotifyIcon_Click(object? sender, EventArgs e)
        {
            App.Log("NotifyIcon: Click");
        }

        private void NotifyIconVisibleCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            if (notifyIcon is not null)
                notifyIcon.Visible = notifyIconVisibleCheckBox.IsChecked;
        }

        private void ApplyTextButton_Click(object? sender, System.EventArgs e)
        {
            if (notifyIcon is not null)
                notifyIcon.Text = notifyIconTextTextBox.Text;
        }
    }
}