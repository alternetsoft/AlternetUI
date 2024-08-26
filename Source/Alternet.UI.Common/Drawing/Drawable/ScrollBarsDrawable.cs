using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Implements vertical and horizontal scrollbars drawing.
    /// </summary>
    public class ScrollBarsDrawable : BaseDrawable
    {
        /// <summary>
        /// Gets or sets vertical scrollbar element.
        /// </summary>
        public ScrollBarDrawable? VertScrollBar;

        /// <summary>
        /// Gets or sets horizontal scrollbar element.
        /// </summary>
        public ScrollBarDrawable? HorzScrollBar;

        /// <summary>
        /// Gets or sets corner element.
        /// </summary>
        public RectangleDrawable? Corner;

        private static ScrollBarsDrawable? defaultDark;
        private static ScrollBarsDrawable? defaultLight;

        private ScrollBar.MetricsInfo? metrics;

        /// <summary>
        /// Gets or sets default drawable used to paint scrollbars when dark color theme is selected.
        /// </summary>
        public static ScrollBarsDrawable DefaultDark
        {
            get
            {
                if(defaultDark is null)
                {
                    defaultDark = new();
                    defaultDark.InitDarkTheme();
                }

                return defaultDark;
            }

            set
            {
                defaultDark = value;
            }
        }

        /// <summary>
        /// Gets or sets default drawable used to paint scrollbars when light color theme is selected.
        /// </summary>
        public static ScrollBarsDrawable DefaultLight
        {
            get
            {
                if (defaultLight is null)
                {
                    defaultLight = new();
                    defaultLight.InitLightTheme();
                }

                return defaultLight;
            }

            set
            {
                defaultLight = value;
            }
        }

        /// <inheritdoc/>
        public override RectD Bounds
        {
            get
            {
                return base.Bounds;
            }

            set
            {
                if (base.Bounds == value)
                    return;
                base.Bounds = value;
            }
        }

        /// <summary>
        /// Gets or sets scrollbar metrics.
        /// </summary>
        public virtual ScrollBar.MetricsInfo? Metrics
        {
            get
            {
                return metrics;
            }

            set
            {
                metrics = value;
                if (VertScrollBar is not null)
                    VertScrollBar.Metrics = value;
                if(HorzScrollBar is not null)
                    HorzScrollBar.Metrics = value;
            }
        }

        /// <inheritdoc/>
        public override void Draw(Control control, Graphics dc)
        {
            if (!Visible)
                return;

            var vertVisible = VertScrollBar?.Visible ?? false;
            var horzVisible = HorzScrollBar?.Visible ?? false;
            var bothVisible = vertVisible && horzVisible;

            if (vertVisible)
            {
                VertScrollBar!.Draw(control, dc);
            }

            if (horzVisible)
            {
                HorzScrollBar!.Draw(control, dc);
            }

            if (bothVisible && Corner is not null)
            {
                SizeD size = (VertScrollBar!.Size.Width, HorzScrollBar!.Size.Height);
                PointD location = (Bounds.Right - size.Width, Bounds.Bottom - size.Height);
                Corner.Bounds = (location, size);
                Corner.Draw(control, dc);
            }
        }

        /// <summary>
        /// Initialized this drawable with default settings for the dark color theme.
        /// </summary>
        public virtual void InitDarkTheme()
        {
        }

        /// <summary>
        /// Initialized this drawable with default settings for the light color theme.
        /// </summary>
        public virtual void InitLightTheme()
        {
        }
    }
}
