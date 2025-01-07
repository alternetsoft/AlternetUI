using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal struct ScrollBarsAndInfo
    {
        public ScrollBarAndInfo Vertical;

        public ScrollBarAndInfo Horizontal;

        public readonly AbstractControl? Container
        {
            get
            {
                return Vertical.ScrollBar?.Parent ?? Horizontal.ScrollBar?.Parent;
            }
        }

        public readonly Coord VertWidth
        {
            get
            {
                if(VertVisible)
                    return Vertical.ScrollBar!.Width;
                return 0;
            }
        }

        public readonly Coord HorzHeight
        {
            get
            {
                if(HorzVisible)
                    return Horizontal.ScrollBar!.Height;
                return 0;
            }
        }

        public readonly bool VertVisible => Vertical.ScrollBar?.Visible ?? false;

        public readonly bool HorzVisible => Horizontal.ScrollBar?.Visible ?? false;

        public ScrollBar GetScrollBarSafe(bool isVert, Action<ScrollBar> initialize)
        {
            var result = GetScrollBar(isVert);

            if (result is null)
            {
                result = new();
                result.Visible = false;
                result.IsVertical = isVert;
                result.IgnoreLayout = true;
                SetScrollBar(isVert, result);
                initialize(result);
            }

            return result;
        }

        public readonly RichTextBoxScrollBars GetCreatedScrollbars()
        {
            bool hasVert = Vertical.ScrollBar is not null;
            bool hasHorz = Horizontal.ScrollBar is not null;
            bool hasBoth = hasVert && hasHorz;

            if (hasBoth)
                return RichTextBoxScrollBars.Both;

            if (hasHorz)
                return RichTextBoxScrollBars.Horizontal;

            if(hasVert)
                return RichTextBoxScrollBars.Vertical;

            return RichTextBoxScrollBars.None;
        }

        public readonly void Layout()
        {
            var container = Container;

            if (container is null)
                return;

            var borderWidth = Border.SafeBorderWidth(container);
            RectD bounds = (PointD.Empty, container.Size);
            var boundsInsideBorder = bounds.DeflatedWithPadding(borderWidth);

            var hs = Horizontal.ScrollBar;
            var vs = Vertical.ScrollBar;

            var horzVisible = (hs is not null) && boundsInsideBorder.Height > hs.Height * 2;
            var vertVisible = (vs is not null) && boundsInsideBorder.Width > vs.Width * 2;

            RectD horzBounds = RectD.Empty;
            RectD vertBounds = RectD.Empty;

            if (horzVisible)
            {
                horzBounds = (
                    boundsInsideBorder.Left,
                    boundsInsideBorder.Bottom - hs!.Height,
                    boundsInsideBorder.Width,
                    hs.Height);
                horzVisible = !horzBounds.IsLocationNegativeOrSizeEmpty;
            }

            if (vertVisible)
            {
                vertBounds = (
                    boundsInsideBorder.Right - vs!.Width,
                    boundsInsideBorder.Top,
                    vs.Width,
                    boundsInsideBorder.Height);
                vertVisible = !vertBounds.IsLocationNegativeOrSizeEmpty;
            }

            if (horzVisible)
            {
                if (vertVisible)
                    horzBounds.Width -= vs!.Width;
                hs!.Bounds = horzBounds;
                hs!.Visible = true;
            }
            else
            {
                hs?.SetVisible(false);
            }

            if (vertVisible)
            {
                if (horzVisible)
                    vertBounds.Height -= hs!.Height;
                App.LogIf($"{vs!.Bounds} / {vertBounds}", vertBounds != vs!.Bounds);
                vs!.Bounds = vertBounds;
                vs!.Visible = true;
            }
            else
            {
                vs?.SetVisible(false);
            }
        }

        public readonly ScrollBarInfo GetInfo(bool isVert)
        {
            if (isVert)
                return Vertical.Info;
            else
                return Horizontal.Info;
        }

        public readonly ScrollBar? GetScrollBar(bool isVert)
        {
            if (isVert)
                return Vertical.ScrollBar;
            else
                return Horizontal.ScrollBar;
        }

        public void SetScrollBar(bool isVert, ScrollBar? value)
        {
            if (isVert)
                Vertical.ScrollBar = value;
            else
                Horizontal.ScrollBar = value;
        }

        public void SetInfo(bool isVert, in ScrollBarInfo info)
        {
            if (isVert)
                Vertical.Info = info;
            else
                Horizontal.Info = info;
        }
    }
}
