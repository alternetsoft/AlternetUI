using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class CaretInfo
    {
        private readonly List<RectI> region = new();
        private SizeI size;
        private PointI position;
        private bool visible;
        private bool focused;

        public List<RectI> Region => region;

        public bool IsDisposed { get; set; }

        public RectI Rect
        {
            get
            {
                return new(Position, Size);
            }
        }

        public virtual SizeI Size
        {
            get
            {
                return size;
            }

            set
            {
                if (size == value)
                    return;
                var oldRect = Rect;
                size = value;
                if (!Visible)
                    return;
                AddToUpdateRegion(oldRect, Rect);
            }
        }

        public virtual PointI Position
        {
            get
            {
                return position;
            }

            set
            {
                if (position == value)
                    return;
                var oldRect = Rect;
                position = value;
                if (!Visible)
                    return;
                AddToUpdateRegion(oldRect, Rect);
            }
        }

        public virtual bool Visible
        {
            get
            {
                return visible;
            }

            set
            {
                if (visible == value)
                    return;
                visible = value;

                if (!visible)
                {
                }

                AddToUpdateRegion(Rect);
            }
        }

        public virtual bool ControlFocused
        {
            get
            {
                return focused;
            }

            set
            {
                if (focused == value)
                    return;
                focused = value;
                if (!Visible)
                    return;
                AddToUpdateRegion(Rect);
            }
        }

        public void AddToUpdateRegion(RectI rect1, RectI rect2)
        {
            AddToUpdateRegion(rect1);
            AddToUpdateRegion(rect2);
        }

        public void AddToUpdateRegion(RectI rect)
        {
            region.Add(rect);
        }

        public void ResetUpdateRegion()
        {
            region.Clear();
        }
    }
}
