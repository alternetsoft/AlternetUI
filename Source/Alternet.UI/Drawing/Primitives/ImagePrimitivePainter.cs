using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace Alternet.Drawing
{
    internal class ImagePrimitivePainter : PrimitivePainter
    {
        public Image? Image;

        public ImageSet? ImageSet;

        public PointD DestPoint;

        public SizeD? Size;

        public RectD? SourceRect = null;

        public bool Stretch = true;

        public bool CenterVert = true;

        public bool CenterHorz = true;

        public RectD DestRect
        {
            get
            {
                if(Size is null)
                {
                    return new RectD(DestPoint, Drawing.SizeD.Empty);
                }
                else
                    return new RectD(DestPoint, Size.Value);
            }

            set
            {
                DestPoint = value.TopLeft;
                Size = value.Size;
            }
        }

        public bool CenterHorzOrVert => CenterHorz || CenterVert;

        public void Draw(Control control, Graphics dc)
        {
            var image = Image;
            image ??= ImageSet?.AsImage(ImageSet.DefaultSize);

            if (Image.IsNullOrEmpty(image) || !Visible)
                return;

            if(Size is null)
            {
                dc.DrawImage(image, DestPoint);
                return;
            }

            if (Stretch)
            {
                if (SourceRect is null)
                    dc.DrawImage(image, DestRect);
                else
                    dc.DrawImage(image, DestRect, SourceRect.Value);
            }
            else
            {
                if (CenterHorzOrVert)
                {
                    var imageRect = SourceRect ?? image.BoundsDip(control);
                    var destRect = DestRect;
                    var centeredRect = imageRect.CenterIn(destRect, CenterHorz, CenterVert);
                    if (SourceRect is null)
                        dc.DrawImage(image, centeredRect);
                    else
                        dc.DrawImage(image, centeredRect, SourceRect.Value);
                }
                else
                    dc.DrawImage(image, DestPoint);
            }
        }
    }
}