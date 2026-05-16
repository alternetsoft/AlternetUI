using System;
using System.IO;

using Alternet.Drawing;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class ToolTipPage : Panel
    {
        private static ImageSet? customImage;
        private static ImageSet? largeImage;

        private readonly RichToolTip toolTip = new();

        private Action<object?, EventArgs>? showMethod;
        private MessageBoxIcon toolTipIcon = MessageBoxIcon.Warning;
        private bool hasMediumImage = true;
        private readonly TemplateControl controlTemplate;

        public ToolTipPage()
        {
            var logConstructor = false;

            App.LogIf("Start ToolTipPage constructor", logConstructor);

            InitializeComponent();

            controlTemplate = CreateTemplateWithBoldText(this);

            tabControl.MinSizeGrowMode = WindowSizeToContentMode.Height;

            dontHideCheckBox.CheckedChanged += HandleCheckedChanged;
            customImageCheckBox.CheckedChanged += HandleCheckedChanged;
            largeImageCheckBox.CheckedChanged += HandleCheckedChanged;

            void HandleCheckedChanged(object? sender, EventArgs e)
            {
                toolTip.HideToolTip();
                ShowToolTipButton_Click(this, EventArgs.Empty);
            }

            Group(
                tooltipTitleLabel,
                tooltipMessageLabel,
                tooltipIconLabel)
            .SuggestedWidthToMax();

            tooltipIconComboBox.IncludeValuePredicate = (item) =>
            {
                if (item is MessageBoxIcon icon)
                    return icon == MessageBoxIcon.None || MessageBoxSvg.GetImage(icon) is not null;
                return false;
            };

            tooltipIconComboBox.EnumType = typeof(MessageBoxIcon);
            tooltipIconComboBox.Value = ToolTipIcon;
            tooltipIconComboBox.ValueChanged += (s, e) =>
            {
                var icon = (MessageBoxIcon)tooltipIconComboBox.Value;
                ToolTipIcon = icon;
                ShowToolTipButton_Click(this, EventArgs.Empty);
            };

            showToolTipButton.Click += ShowToolTipButton_Click;
            hideToolTipButton.Click += HideToolTipButton_Click;
            resetTitleButton.Click += ResetTitleButton_Click;
            showSimpleButton.Click += ShowSimpleButton_Click;
            showImageButton.Click += ShowImageButton_Click;
            resetTextButton.Click += ResetTextButton_Click;

            borderCheckBox.CheckedChanged += (s, e) =>
            {
                toolTip.HasToolTipBorder = borderCheckBox.IsChecked;
                ShowToolTipButton_Click(this, EventArgs.Empty);
            };

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

            void ShowFromTemplate(object? sender, EventArgs e)
            {
                showMethod = ShowFromTemplate;
                toolTip.SetToolTipFromTemplate(controlTemplate).ShowToolTip();
            }

            showTemplateButton.Click += ShowFromTemplate;

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

            popup.Add("Set ToolTip container back to green", () =>
            {
                tooltipPreview.ParentBackColor = false;
                tooltipPreview.BackColor = Color.LightGreen;
            });

            popup.Add("Set ToolTip back to red", () =>
            {
                toolTip.ParentBackColor = false;
                toolTip.BackColor = Color.IndianRed;
            });

            popup.Add("Next Alignment", NextAlignment);

            void ToggleLargeImage()
            {
                if (hasMediumImage)
                {
                    var resFolder = "Resources.Backgrounds.";
                    var resPrefix = AssemblyUtils.GetImageUrlInAssembly(GetType().Assembly, resFolder);

                    var url = $"{resPrefix}GirlAndBoyFlower.png";

                    LargeImageSet = ImageSet.FromUrl(url);
                }
                else
                {
                    LargeImageSet = null;
                }

                hasMediumImage = !hasMediumImage;

                HideToolTipButton_Click(this, EventArgs.Empty);
            }

            popup.Add("Toggle Large Image", () =>
            {
                ToggleLargeImage();
            });

            popup.Add("Show ToolTip with Large Image", () =>
            {
                hasMediumImage = true;
                ToggleLargeImage();

                toolTip.BeginUpdate();

                try
                {
                    customImageCheckBox.Checked = true;
                    largeImageCheckBox.Checked = true;

                    ShowToolTipButton_Click(this, EventArgs.Empty);
                }
                finally
                {
                    toolTip.EndUpdate();
                }
            });

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
                    RunWhenIdle(() => ShowToolTipButton_Click(this, EventArgs.Empty));
            };

            tooltipPreview.Click += (s, e) =>
            {
                ShowToolTipButton_Click(this, EventArgs.Empty);
            };

            App.LogIf("End ToolTipPage constructor", logConstructor);
        }

        public static ImageSet? CustomImageSet
        {
            get
            {
                if (customImage is null)
                    LoadImages();
                return customImage;
            }

            set
            {
                customImage = value;
            }
        }

        public static ImageSet? LargeImageSet
        {
            get
            {
                if (largeImage is null)
                    LoadImages();
                return largeImage;
            }

            set
            {
                largeImage = value;
            }
        }

        public static void ShowWithLargeImage(IRichToolTip? toolTip)
        {
            if (toolTip is null)
                return;
            toolTip.OnlyImage(LargeImageSet).SetToolTipBackgroundColor(Color.ForestGreen).PostShowToolTip();
        }

        public static void ShowWithBoldText(IRichToolTip? toolTip, AbstractControl? templateParent)
        {
            if (toolTip is null)
                return;

            var template = CreateTemplateWithBoldText(templateParent);
            toolTip.SetToolTipFromTemplate(template).PostShowToolTip();
        }

        public static TemplateControls.BoldText<Label> CreateTemplateWithBoldText(AbstractControl? parent)
        {
            var controlTemplate = TemplateUtils.CreateTemplateWithBoldText(
                "This text has ",
                "bold",
                " fragment",
                new FontAndColor(
                    Color.Red,
                    Color.LightGoldenrodYellow,
                    Control.DefaultFont.Scaled(1.5f)));

            controlTemplate.ParentBackColor = false;
            controlTemplate.BackColor = Color.Transparent;
            controlTemplate.Parent = parent;
            controlTemplate.SetSizeToContent();

            return controlTemplate;
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
            toolTip.DoInsideUpdate(() =>
            {
                if (textBox.Text == string.Empty)
                    textBox.Text = text;
                else
                    if (textBox.Text == text)
                        textBox.Text = PropertyGridSample.ObjectInit.LoremIpsum;
                    else
                        textBox.Text = string.Empty;
                ShowToolTipButton_Click(this, EventArgs.Empty);
            });
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
            showMethod = ShowImageButton_Click;
            toolTip.OnlyImage(largeImage).SetToolTipBackgroundColor(Color.ForestGreen).ShowToolTip();
        }

        private static void LoadImages()
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
            showMethod = ShowToolTipButton_Click;
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
            showMethod = ShowSimpleButton_Click;
            toolTip.SetToolTip(tooltipMessageTextBox.Text ?? "This is a simple tooltip")
                .SetIcon(ToolTipIcon)
                .ShowToolTip();
        }

        private void HideToolTipButton_Click(object? sender, EventArgs e)
        {
            toolTip.HideToolTip();
            showMethod = null;
        }

        protected override void OnSystemColorsChanged(EventArgs e)
        {
            base.OnSystemColorsChanged(e);
            showMethod?.Invoke(this, EventArgs.Empty);
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