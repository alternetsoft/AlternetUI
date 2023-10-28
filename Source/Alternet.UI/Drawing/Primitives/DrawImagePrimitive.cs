using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    internal class DrawImagePrimitive : DrawPrimitive
    {
        public Image? Image { get; set; }

        public Point DestPoint { get; set; }

        public Size? Size { get; set; }

        public Rect? SourceRect { get; set; }

        public bool Stretch { get; set; } = true;

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

        public void Draw(DrawingContext dc)
        {
            if (Image is null || !Visible)
                return;

            if(!Stretch || Size is null)
                dc.DrawImage(Image, DestPoint);
            else
            {
                if (SourceRect is null)
                    dc.DrawImage(Image, DestRect);
                else
                    dc.DrawImage(Image, DestRect, SourceRect.Value);
            }
        }
    }
}