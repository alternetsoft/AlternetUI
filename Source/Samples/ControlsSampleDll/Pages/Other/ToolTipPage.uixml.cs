using System;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class ToolTipPage : Control
    {
        private ImageSet? customImage;
        private ImageSet? largeImage;

        public ToolTipPage()
        {
            InitializeComponent();

            tabControl.MinSizeGrowMode = WindowSizeToContentMode.Height;

            Group(
                tooltipTitleLabel,
                tooltipMessageLabel,
                tooltipIconLabel,
                tooltipKindLabel)
            .SuggestedWidthToMax();

            tooltipKindComboBox.BindEnumProp(this, nameof(ToolTipKind));
            tooltipIconComboBox.BindEnumProp(this, nameof(ToolTipIcon));

            showToolTipButton.Click += ShowToolTipButton_Click;
            hideToolTipButton.Click += HideToolTipButton_Click;
            resetTitleButton.Click += ResetTitleButton_Click;
            showSimpleButton.Click += ShowSimpleButton_Click;
            showImageButton.Click += ShowImageButton_Click;
            resetTextButton.Click += ResetTextButton_Click;

            Group(
                showToolTipButton,
                hideToolTipButton,
                showSimpleButton,
                resetTitleButton,
                resetTextButton)
            .SuggestedWidthToMax();

            toolTipLabel.Click += ToolTipLabel_Click;

            var popup = new ContextMenuStrip();

            tooltipPreview.ContextMenuStrip = popup;

            popup.Add("Log Information", Log);
            popup.Add("Show popup on bottom-right", ShowPopupBottomRight);
        }

        private void ShowPopupBottomRight()
        {
            RichToolTip toolTip = new(
                "Title",
                "This is message text first line." + Environment.NewLine + "This is second line.");
            toolTip.SetTipKind(RichToolTipKind.None)
            .SetIcon(MessageBoxIcon.Information)
            .SetAsDefault()
            .SetLocationDecrement(true, true)
            .ShowAtLocation(
                tooltipPreview,
                (tooltipPreview.Width, tooltipPreview.Height),
                false);
        }

        private void ToolTipLabel_Click(object? sender, EventArgs e)
        {
            App.Log("ToolTipLabel_Click");
            tooltipPreview.BackgroundColor = Color.White;
            toolTipLabel.Text = string.Empty;
            toolTipLabel.BackgroundColor = Color.White;
        }

        private void ResetTitleButton_Click(object? sender, EventArgs e)
        {
            ToggleText(tooltipTitleTextBox, "Message Title");
        }

        private void ResetTextButton_Click(object? sender, EventArgs e)
        {
            ToggleText(tooltipMessageTextBox, "This is sample text");
        }

        private void ToggleText(TextBox textBox, string text)
        {
            if (textBox.Text == string.Empty)
                textBox.Text = text;
            else
                textBox.Text = string.Empty;
        }

        internal void Log()
        {
            var toolTip = RichToolTip.Default;

            if (toolTip is not null)
            {
                App.LogNameValue("ToolTip window size (px)", toolTip.Handler.SizeInPixels);
            }

            LogUtils.LogColor("Info", SystemColors.Info);
            LogUtils.LogColor("SystemSettings.Info", new(SystemSettings.GetColor(KnownSystemColor.Info)));
            LogUtils.LogColor("InfoText", SystemColors.InfoText);
            LogUtils.LogColor("SystemSettings.InfoText", new(SystemSettings.GetColor(KnownSystemColor.InfoText)));
            LogUtils.LogColor("BkColor", RealBackgroundColor);
            LogUtils.LogColor("FgColor", RealForegroundColor);
        }

        private void ShowImageButton_Click(object? sender, EventArgs e)
        {
            LoadLargeImage();

            RichToolTip toolTip = new();
            toolTip.SetTipKind(RichToolTipKind.None);
            toolTip.SetBackgroundColor(Color.Black);
            toolTip.SetIcon(largeImage);
            RichToolTip.Default = toolTip;

            ShowToolTip(toolTip);
        }

        private void ShowToolTip(RichToolTip toolTip)
        {
            if (dontHideCheckBox.IsChecked)
                toolTip.SetTimeout(0);

            if (atCenterCheckBox.IsChecked)
                toolTip.Show(tooltipPreview);
            else
            {
                // Shows at the top-left corner of the tooltipPreview control.
                toolTip.ShowAtLocation(tooltipPreview, (0, 0), adjustPosCheckBox.IsChecked);
            }
        }

        private void LoadLargeImage()
        {
            if (largeImage is null)
            {
                var stream = typeof(ToolTipPage).Assembly.GetManifestResourceStream(
                    "ControlsSampleDll.Resources.panda2.jpg");
                if (stream is null)
                    largeImage = (ImageSet)(Image)Color.Blue.AsImage(250);
                else
                    largeImage = new(stream);
            }
        }

        private void ShowToolTipButton_Click(object? sender, EventArgs e)
        {
            if (customImageCheckBox.IsChecked || !atCenterCheckBox.IsChecked)
            {
                RichToolTip toolTip = new(tooltipTitleTextBox.Text, tooltipMessageTextBox.Text);
                toolTip.SetTipKind(ToolTipKind);

                if (customImageCheckBox.IsChecked)
                {
                    if (!largeImageCheckBox.IsChecked)
                    {
                        customImage ??= (ImageSet)NotifyIconPage.Image;
                        toolTip.SetIcon(customImage);
                    }
                    else
                    {
                        LoadLargeImage();
                        toolTip.SetIcon(largeImage);
                    }
                }
                else
                {
                    toolTip.SetIcon(ToolTipIcon);
                }

                RichToolTip.Default = toolTip;

                ShowToolTip(toolTip);
            }
            else
            {
                RichToolTip.Show(
                    tooltipTitleTextBox.Text,
                    tooltipMessageTextBox.Text,
                    tooltipPreview,
                    ToolTipKind,
                    ToolTipIcon,
                    dontHideCheckBox.IsChecked ? 0 : null,
                    adjustPosCheckBox.IsChecked);
            }
        }

        private void ShowSimpleButton_Click(object? sender, EventArgs e)
        {
            RichToolTip.ShowSimple(tooltipMessageTextBox.Text, tooltipPreview);
        }

        private void HideToolTipButton_Click(object? sender, EventArgs e)
        {
            RichToolTip.HideDefault();
        }

        public RichToolTipKind ToolTipKind { get; set; } = RichToolTipKind.Top;

        public MessageBoxIcon ToolTipIcon { get; set; } = MessageBoxIcon.Warning;
    }
}