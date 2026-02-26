using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;
using Alternet.Base.Collections;
using Alternet.Drawing;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;
using SkiaSharp;

namespace PropertyGridSample
{
    public partial class MainWindow
    {
        void InitTestsPictureBox()
        {
            AddControlAction<PictureBox>(
                "Set MessageBoxIcon.Error",
                TestPictureBoxSetMessageBoxIconError);
            AddControlAction<PictureBox>("Set ToolTip image", TestPictureBoxSetToolTipImage);

            AddControlAction<PictureBox>("Load ErrorPngICCP", TestPictureBoxErrorPngICCP);

            AddControlAction<PictureBox>("Set border and parent background", TestPictureBoxSetBorderAndParentBack);
        }

        void TestPictureBoxSetBorderAndParentBack(PictureBox control)
        {
            control.BorderWidth = 3;
            control.UniformBorderCornerRadius = 10;
            control.UniformBorderRadiusIsPercent = true;
            control.HasBorder = true;

            var parent = control.Parent;
            if (parent == null)
                return;
            parent.UserPaint = true;
            parent.Paint -= ParentPaint;
            parent.Paint += ParentPaint;
            parent.Refresh();

            void ParentPaint(object? sender, PaintEventArgs e)
            {
                e.Graphics.FillRectangle(Color.SandyBrown.AsBrush, e.ClipRectangle);
                FancyBackgroundDrawer.DrawFancyBackground(e.Graphics, e.ClipRectangle);
            }
        }

        void TestPictureBoxErrorPngICCP(PictureBox control)
        {
            var resFolder = "Resources.Tests.ErrorPngICCP.";
            var resPrefix = AssemblyUtils.GetImageUrlInAssembly(GetType().Assembly, resFolder);
            var url = $"{resPrefix}Pencil.png";

            using var stream = ResourceLoader.StreamFromUrl(url);
            var image = new ImageSet(stream);

            control.ImageSet = image;
        }

        void TestPictureBoxSetToolTipImage(PictureBox control)
        {
            var template = new TemplateControls.RichToolTipTemplate();

            template.DoInsideLayout(() =>
            {
                template.Parent = control;

                try
                {
                    template.BackgroundColor = RichToolTip.DefaultToolTipBackgroundColor;
                    template.ForegroundColor = RichToolTip.DefaultToolTipForegroundColor;

                    template.TitleLabel.Text = "This is title";
                    template.TitleLabel.ParentForeColor = false;
                    template.TitleLabel.ParentFont = false;
                    template.TitleLabel.Font = template.RealFont.Scaled(1.5f);
                    template.TitleLabel.ForegroundColor = RichToolTip.DefaultToolTipTitleForegroundColor;

                    template.MessageLabel.Text = "This is message text";

                    var sizeInPixels
                    = GraphicsFactory.PixelFromDip(RichToolTip.DefaultMinImageSize, control.ScaleFactor);

                    template.PictureBox
                    .SetIcon(MessageBoxIcon.Warning, sizeInPixels);

                    var image = TemplateUtils.GetTemplateAsImage(template);
                    control.ImageSet = null;
                    control.Image = image;

                    control.SuggestedSize = image.Size.PixelToDip(control.ScaleFactor);
                }
                finally
                {
                    template.Parent = null;
                }
            });

        }

        void TestPictureBoxSetMessageBoxIconError(PictureBox control)
        {
            control.SetIcon(MessageBoxIcon.Error, 64);
        }
    }
}