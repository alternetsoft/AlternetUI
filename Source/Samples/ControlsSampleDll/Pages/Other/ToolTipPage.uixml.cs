using System;
using System.IO;

using Alternet.Drawing;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class ToolTipPage : Panel
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
                new FontAndColor(
                    Color.Red,
                    Color.LightGoldenrodYellow,
                    Control.DefaultFont.Scaled(1.5f)));

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

            tooltipIconComboBox.IncludeValuePredicate = (item) =>
            {
                if(item is MessageBoxIcon icon)
                    return icon == MessageBoxIcon.None || MessageBoxSvg.GetImage(icon) is not null;
                return false;
            };

            tooltipIconComboBox.EnumType = typeof(MessageBoxIcon);
            tooltipIconComboBox.Value = ToolTipIcon;
            tooltipIconComboBox.ValueChanged += (s, e) =>
            {
                var icon = (MessageBoxIcon)tooltipIconComboBox.Value;
                ToolTipIcon = icon;
            };

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

            toolTip.ParentBackColor = false;
            toolTip.BackgroundColor = SystemColors.Window;

            toolTip.Parent = tooltipPreview;
            toolTip.ShowDebugRectangleAtCenter = false;
            toolTip.ContextMenuStrip = popup;
            toolTip.Padding = 10;

            toolTip.SizeChanged += (s, e) =>
            {
            };

            showTemplateButton.Click += (s, e) =>
            {
                toolTip.SetToolTipFromTemplate(controlTemplate).ShowToolTip();
            };

            popup.Add("Log Information", Log);

            void ZoomFont(int incValue)
            {
                toolTip.HideToolTip();
                toolTip.Font = toolTip.RealFont.WithSize(toolTip.RealFont.Size + incValue);
                toolTip.Font.ResetSkiaFont();
                ShowToolTipButton_Click(this, EventArgs.Empty);
            }

            popup.Add("Zoom in font", () =>
            {
                ZoomFont(1);
            });

            popup.Add("Zoom out font", () =>
            {
                ZoomFont(-1);
            });

            popup.Add("Reset SkiaFont and reshow", () =>
            {
                toolTip.HideToolTip();
                toolTip.RealFont.ResetSkiaFont();
                ShowToolTipButton_Click(this, EventArgs.Empty);
            });

            popup.Add("Show SkiaSharp Font defaults", () =>
            {
                SkiaFontSettings.ShowFontSettingsDialog(() =>
                {
                    toolTip.HideToolTip();
                });
            });

            popup.Add("Next Alignment", NextAlignment);

            atCenterCheckBox.CheckedChanged += (s, e) =>
            {
                if (atCenterCheckBox.IsChecked)
                {
                    toolTip.ToolTipAlignment = HVAlignment.Center;
                }
                else
                {
                    toolTip.ToolTipAlignment = HVAlignment.TopLeft;
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

            RunWhenIdle(() =>
            {
                ShowToolTipButton_Click(this, EventArgs.Empty);

                toolTip.ToolTipVisibleChanged += (s, e) =>
                {
                    var prefix = "ToolTip.Visible =";
                    App.LogReplace($"{prefix} {toolTip.ToolTipVisible}", prefix);
                };
            });

            tooltipTitleTextBox.DelayedTextChanged += (s, e) =>
            {
                if (toolTip.ToolTipVisible)
                    RunWhenIdle(() => ShowToolTipButton_Click(this, EventArgs.Empty));
            };

            tooltipMessageTextBox.DelayedTextChanged += (s, e) =>
            {
                if (toolTip.ToolTipVisible)
                    RunWhenIdle(()=>ShowToolTipButton_Click(this, EventArgs.Empty));
            };

            tooltipPreview.Click += (s, e) =>
            {
                ShowToolTipButton_Click(this, EventArgs.Empty);
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
            if (textBox.Text == text)
                textBox.Text = PropertyGridSample.ObjectInit.LoremIpsum;
            else
                textBox.Text = string.Empty;
            ShowToolTipButton_Click(this, EventArgs.Empty);
        }

        internal void NextAlignment()
        {
            toolTip.ToolTipAlignment = toolTip.ToolTipAlignment.NextValueNoStretchOrFill();
        }

        internal void Log()
        {
            LogUtils.LogColor("Info", SystemColors.Info);
            
            LogUtils.LogColor(
                "SystemSettings.Info",
                new(SystemSettings.GetColor(KnownSystemColor.Info)));

            LogUtils.LogColor("InfoText", SystemColors.InfoText);
            
            LogUtils.LogColor(
                "SystemSettings.InfoText",
                new(SystemSettings.GetColor(KnownSystemColor.InfoText)));

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
            ImageSet? GetImage(string resName, int backupColorImageSize)
            {
                var stream = typeof(ToolTipPage).Assembly.GetManifestResourceStream(resName);
                ImageSet? result;
                if (stream is null)
                    result = (ImageSet)(Image)Color.Blue.AsImage(backupColorImageSize);
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