using System;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class NotifyIconPage : Control
    {
        public static readonly Image Image = new Bitmap(typeof(NotifyIconPage).Assembly.GetManifestResourceStream(
            "ControlsSample.Resources.Logo16x16.png") ?? throw new Exception());

        private IPageSite? site;

        private readonly NotifyIcon? notifyIcon;

        public NotifyIconPage()
        {
            InitializeComponent();

            notifyPanel.Visible = NotifyIcon.IsAvailable;

            if (notifyPanel.Visible)
            {
                notifyIcon = new NotifyIcon { Icon = Image, Text = notifyIconTextTextBox.Text };
                notifyIcon.Click += NotifyIcon_Click;
                notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
                notifyIcon.Menu = new ExampleContextMenu();
            }

            ControlSet.New(
                iconTextLabel,
                tooltipTitleLabel,
                tooltipMessageLabel,
                tooltipIconLabel,
                tooltipKindLabel)
                .SuggestedWidthToMax();

            tooltipKindComboBox.BindEnumProp(this, nameof(ToolTipKind));
            tooltipIconComboBox.BindEnumProp(this, nameof(ToolTipIcon));

            showToolTipButton.Click += ShowToolTipButton_Click;
            hideToolTipButton.Click += HideToolTipButton_Click;

            GetColumnGroup(3, true).SuggestedWidthToMax();

        }

        internal void LogColors()
        {
            LogUtils.LogColor("Info", SystemColors.Info);
            LogUtils.LogColor("SystemSettings.Info", SystemSettings.GetColor(SystemSettingsColor.Info));
            LogUtils.LogColor("InfoText", SystemColors.InfoText);
            LogUtils.LogColor("SystemSettings.InfoText", SystemSettings.GetColor(SystemSettingsColor.InfoText));
            LogUtils.LogColor("BkColor", RealBackgroundColor);
            LogUtils.LogColor("FgColor", RealForegroundColor);
        }

        private void ShowToolTipButton_Click(object? sender, EventArgs e)
        {
            RichToolTip.Show(
                tooltipTitleTextBox.Text,
                tooltipMessageTextBox.Text,
                tooltipPreview,
                ToolTipKind,
                ToolTipIcon);
        }

        private void HideToolTipButton_Click(object? sender, EventArgs e)
        {
            // RichToolTip.Show assigns RichToolTip.Default,
            // so we can use this variable for hiding the tooltip.
            var toolTip = RichToolTip.Default;
            RichToolTip.Default = null;
            toolTip?.Dispose();
        }

        public RichToolTipKind ToolTipKind { get; set; } = RichToolTipKind.None;

        public MessageBoxIcon ToolTipIcon { get; set; } = MessageBoxIcon.Warning;

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