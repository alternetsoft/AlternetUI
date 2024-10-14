using System;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class ToolTipPage : Control
    {
        private readonly RichToolTip toolTip = new();

        private ImageSet? customImage;
        private ImageSet? largeImage;
        private TemplateControl controlTemplate;

        public ToolTipPage()
        {
            InitializeComponent();

            controlTemplate = TemplateUtils.CreateTemplateWithBoldText(
                "This text has ",
                "bold",
                " fragment",
                new FontAndColor(Color.Red, Color.LightGoldenrodYellow, Font.Default.Scaled(1.5)));

            controlTemplate.Parent = this;
            controlTemplate.SetSizeToContent();

            tabControl.MinSizeGrowMode = WindowSizeToContentMode.Height;

            Group(
                tooltipTitleLabel,
                tooltipMessageLabel,
                tooltipIconLabel)
            .SuggestedWidthToMax();

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

            tooltipPreview.VerticalAlignment = VerticalAlignment.Fill;
            tooltipPreview.Layout = LayoutStyle.Vertical;
            toolTip.VerticalAlignment = VerticalAlignment.Fill;
            toolTip.BackgroundColor = SystemColors.Window;
            toolTip.Parent = tooltipPreview;
            toolTip.ShowDebugRectangleAtCenter = false;
            toolTip.ContextMenuStrip = popup;

            showTemplateButton.Click += (s, e) =>
            {
                toolTip.SetToolTipFromTemplate(controlTemplate);
                ShowToolTip();
            };

            popup.Add("Log Information", Log);
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
            toolTip.OnlyImage(largeImage, Color.Black);
            ShowToolTip();
        }

        private void ShowToolTip()
        {
            HideToolTip();
            if (dontHideCheckBox.IsChecked)
                toolTip.SetTimeout(0);

            toolTip.ShowToolTip();
        }

        private void LoadLargeImage()
        {
            if (largeImage is null)
            {
                var stream = typeof(ToolTipPage).Assembly.GetManifestResourceStream(
                    "ControlsSampleDll.Resources.EmployeePhoto.jpg");
                if (stream is null)
                    largeImage = (ImageSet)(Image)Color.Blue.AsImage(250);
                else
                    largeImage = new(stream);
            }
        }

        private void ShowToolTipButton_Click(object? sender, EventArgs e)
        {
            if (customImageCheckBox.IsChecked)
            {
                if (!largeImageCheckBox.IsChecked)
                {
                    toolTip.SetToolTip(
                        tooltipTitleTextBox.Text,
                        tooltipMessageTextBox.Text,
                        ToolTipIcon,
                        dontHideCheckBox.IsChecked ? 0 : null);
                    customImage ??= (ImageSet)NotifyIconPage.Image;
                    toolTip.SetIcon(customImage);
                }
                else
                {
                    LoadLargeImage();
                    toolTip.SetToolTip(
                        null,
                        tooltipMessageTextBox.Text,
                        ToolTipIcon,
                        dontHideCheckBox.IsChecked ? 0 : null);
                    toolTip.SetIcon(largeImage);
                }
            }
            else
            {
                toolTip.SetToolTip(
                    tooltipTitleTextBox.Text,
                    tooltipMessageTextBox.Text,
                    ToolTipIcon,
                    dontHideCheckBox.IsChecked ? 0 : null);
            }

            ShowToolTip();
        }

        private void ShowSimpleButton_Click(object? sender, EventArgs e)
        {
            toolTip.SetToolTip(tooltipMessageTextBox.Text);
            ShowToolTip();
        }

        private void HideToolTipButton_Click(object? sender, EventArgs e)
        {
            toolTip.HideToolTip();
        }

        public RichToolTipKind ToolTipKind { get; set; } = RichToolTipKind.Top;

        public MessageBoxIcon ToolTipIcon { get; set; } = MessageBoxIcon.Warning;
    }
}