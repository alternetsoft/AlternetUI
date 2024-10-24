﻿using System;
using System.IO;

using Alternet.Drawing;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class ToolTipPage : Control
    {
        private readonly RichToolTip toolTip = new();

        private ImageSet? customImage;
        private ImageSet? largeImage;
        private MessageBoxIcon toolTipIcon = MessageBoxIcon.Warning;
        private readonly TemplateControl controlTemplate;

        public ToolTipPage()
        {
            InitializeComponent();

            controlTemplate = TemplateUtils.CreateTemplateWithBoldText(
                "This text has ",
                "bold",
                " fragment",
                new FontAndColor(Color.Red, Color.LightGoldenrodYellow, Control.DefaultFont.Scaled(1.5)));

            controlTemplate.Parent = this;
            controlTemplate.SetSizeToContent();

            tabControl.MinSizeGrowMode = WindowSizeToContentMode.Height;

            dontHideCheckBox.CheckedChanged += HandleCheckedChanged;
            customImageCheckBox.CheckedChanged += HandleCheckedChanged;
            largeImageCheckBox.CheckedChanged += HandleCheckedChanged;

            void HandleCheckedChanged(object? sender, EventArgs e)
            {
                toolTip.HideToolTip();
            };

            Group(
                tooltipTitleLabel,
                tooltipMessageLabel,
                tooltipIconLabel)
            .SuggestedWidthToMax();

            tooltipIconComboBox.BindEnumProp(
                this,
                nameof(ToolTipIcon),
                (item) =>
                {
                    var icon = (MessageBoxIcon)item;
                    return icon == MessageBoxIcon.None || MessageBoxSvg.GetImage(icon) is not null;
                });

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
            toolTip.Padding = 10;

            showTemplateButton.Click += (s, e) =>
            {
                toolTip.SetToolTipFromTemplate(controlTemplate).ShowToolTip();
            };

            popup.Add("Log Information", Log);
            popup.Add("Next Alignment", NextAligment);

            atCenterCheckBox.CheckedChanged += (s, e) =>
            {
                if (atCenterCheckBox.IsChecked)
                {
                    toolTip.ToolTipPicture.Alignment = HVAlignment.Center;
                }
                else
                {
                    toolTip.ToolTipPicture.Alignment = HVAlignment.TopLeft;
                }
            };

            otherSchemeCheckBox.IsEnabled = false;
            otherSchemeCheckBox.CheckedChanged += (s, e) =>
            {
                var isDark = otherSchemeCheckBox.IsChecked ? !IsDarkBackground : IsDarkBackground;

                toolTip.BackgroundColor = isDark ? (44, 44, 44) : Color.White;
                toolTip.HideToolTip();
                ShowToolTipButton_Click(null, EventArgs.Empty);
            };
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
            toolTip.HideToolTip();
        }

        internal void NextAligment()
        {
            toolTip.ToolTipPicture.Alignment = toolTip.ToolTipPicture.Alignment.NextValue();
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
            LoadImages();
            toolTip.OnlyImage(largeImage).SetToolTipBackgroundColor(Color.ForestGreen).ShowToolTip();
        }

        private void LoadImages()
        {
            ImageSet? GetImage(string resname, int backgupColorImageSize)
            {
                var stream = typeof(ToolTipPage).Assembly.GetManifestResourceStream(resname);
                ImageSet? result;
                if (stream is null)
                    result = (ImageSet)(Image)Color.Blue.AsImage(backgupColorImageSize);
                else
                    result = new(stream);
                return result;
            }

            largeImage ??= GetImage("ControlsSampleDll.Resources.EmployeePhoto.jpg", 250);
            customImage ??= GetImage("ControlsSampleDll.Resources.icon-48x48.png", 32);
        }

        private void ShowToolTipButton_Click(object? sender, EventArgs e)
        {
            if (customImageCheckBox.IsChecked)
            {
                LoadImages();

                if (!largeImageCheckBox.IsChecked)
                {
                    toolTip.SetToolTip(
                        tooltipTitleTextBox.Text,
                        tooltipMessageTextBox.Text,
                        ToolTipIcon,
                        dontHideCheckBox.IsChecked ? 0 : null);
                    toolTip.SetIcon(customImage);
                }
                else
                {
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

            void ShowToolTip()
            {
                void PrivateShow()
                {
                    if (dontHideCheckBox.IsChecked)
                        toolTip.SetTimeout(0);
                    else
                        toolTip.ResetTimeout();

                    toolTip.ShowToolTip();
                }

                if (otherSchemeCheckBox.IsChecked)
                {
                    LightDarkColor.DoInsideTempIsDarkOverride(!IsDarkBackground, () =>
                    {
                        PrivateShow();
                    });
                }
                else
                {
                    PrivateShow();
                }
            }

        }

        private void ShowSimpleButton_Click(object? sender, EventArgs e)
        {
            toolTip.SetToolTip(tooltipMessageTextBox.Text ?? "This is a simple tooltip")
                .SetIcon(ToolTipIcon)
                .ShowToolTip();
        }

        private void HideToolTipButton_Click(object? sender, EventArgs e)
        {
            toolTip.HideToolTip();
        }

        public MessageBoxIcon ToolTipIcon
        {
            get => toolTipIcon;
            set
            {
                toolTipIcon = value;
                toolTip.HideToolTip();
            }
        }
    }
}