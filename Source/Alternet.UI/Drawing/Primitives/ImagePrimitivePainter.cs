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

        public Point DestPoint;

        public Size? Size;

        public Rect? SourceRect = null;

        public bool Stretch = true;

        public bool CenterVert = true;

        public bool CenterHorz = true;

        public Rect DestRect
        {
            get
            {
                if(Size is null)
                {
                    return new Rect(DestPoint, Drawing.Size.Empty);
                }
                else
                    return new Rect(DestPoint, Size.Value);
            }

            set
            {
                DestPoint = value.TopLeft;
                Size = value.Size;
            }
        }

        public bool CenterHorzOrVert => CenterHorz || CenterVert;

        public void Draw(Control control, DrawingContext dc)
        {
            if (Image.IsNullOrEmpty(Image) || !Visible)
                return;

            if(Size is null)
            {
                dc.DrawImage(Image, DestPoint);
                return;
            }

            if (Stretch)
            {
                if (SourceRect is null)
                    dc.DrawImage(Image, DestRect);
                else
                    dc.DrawImage(Image, DestRect, SourceRect.Value);
            }
            else
            {
                if (CenterHorzOrVert)
                {
                    var imageRect = SourceRect ?? Image.BoundsDip(control);
                    var destRect = DestRect;
                    var centeredRect = imageRect.CenterIn(destRect, CenterHorz, CenterVert);
                    if (SourceRect is null)
                        dc.DrawImage(Image, centeredRect);
                    else
                        dc.DrawImage(Image, centeredRect, SourceRect.Value);
                }
                else
                    dc.DrawImage(Image, DestPoint);
            }
        }
    }
}