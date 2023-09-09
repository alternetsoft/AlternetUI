using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Helper class for using <see cref="Brush"/> properties in the <see cref="PropertyGrid"/>.
    /// </summary>
    public class PropertyGridAdapterBrush : PropertyGridAdapterGeneric
    {
        private BrushType brushType = BrushType.None;
        private Color color = Color.Black;
        private Point linearGradientStart;
        private Point linearGradientEnd = new(1, 1);
        private Point radialGradientCenter = Point.Empty;
        private Point radialGradientOrigin;
        private Color endColor = Color.White;
        private double radialGradientRadius = 10;
        private BrushHatchStyle hatchStyle;

        // (Color color, double offset)
        private GradientStop[] gradientStops = Array.Empty<GradientStop>();

        /// <summary>
        /// Returns <see cref="PropertyGridAdapterGeneric.Value"/> as <see cref="Brush"/>.
        /// </summary>
        public Brush? Brush
        {
            get => Value as Brush;
            set => Value = value;
        }

        /// <summary>
        /// Gets or sets type of the brush.
        /// </summary>
        public BrushType BrushType
        {
            get
            {
                return brushType;
            }

            set
            {
                brushType = value;
                Save();
            }
        }

        /// <summary>
        /// Gets or sets brush color.
        /// </summary>
        public Color Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
                Save();
            }
        }

        /// <summary>
        /// Gets or sets gradient brush end color.
        /// </summary>
        public Color EndColor
        {
            get
            {
                return endColor;
            }

            set
            {
                endColor = value;
                Save();
            }
        }

        /// <inheritdoc cref="LinearGradientBrush.StartPoint"/>
        public Point LinearGradientStart
        {
            get
            {
                return linearGradientStart;
            }

            set
            {
                linearGradientStart = value;
                Save();
            }
        }

        /// <inheritdoc cref="LinearGradientBrush.EndPoint"/>
        public Point LinearGradientEnd
        {
            get
            {
                return linearGradientEnd;
            }

            set
            {
                linearGradientEnd = value;
                Save();
            }
        }

        /// <inheritdoc cref="RadialGradientBrush.Center"/>
        public Point RadialGradientCenter
        {
            get
            {
                return radialGradientCenter;
            }

            set
            {
                radialGradientCenter = value;
                Save();
            }
        }

        /// <inheritdoc cref="RadialGradientBrush.GradientOrigin"/>
        public Point RadialGradientOrigin
        {
            get
            {
                return radialGradientOrigin;
            }

            set
            {
                radialGradientOrigin = value;
                Save();
            }
        }

        /// <inheritdoc cref="RadialGradientBrush.Radius"/>
        public double RadialGradientRadius
        {
            get
            {
                return radialGradientRadius;
            }

            set
            {
                radialGradientRadius = value;
                Save();
            }
        }

        /// <inheritdoc cref="LinearGradientBrush.GradientStops"/>
        public GradientStop[] GradientStops
        {
            get
            {
                return gradientStops;
            }

            set
            {
                gradientStops = value;
                Save();
            }
        }

        /// <inheritdoc cref="HatchBrush.HatchStyle"/>
        public BrushHatchStyle HatchStyle
        {
            get
            {
                return hatchStyle;
            }

            set
            {
                hatchStyle = value;
                Save();
            }
        }

        /// <inheritdoc/>
        protected override void Save()
        {
            Brush CreateRadialGradientBrush()
            {
                if (gradientStops != null && gradientStops.Length > 0)
                {
                    return new RadialGradientBrush(
                        radialGradientCenter,
                        radialGradientRadius,
                        radialGradientOrigin,
                        gradientStops);
                }
                else
                {
                    return new RadialGradientBrush(
                        radialGradientCenter,
                        radialGradientRadius,
                        radialGradientOrigin,
                        LinearGradientBrush.GetGradientStopsFromEdgeColors(color, endColor));
                }
            }

            Brush CreateLinearGradientBrush()
            {
                if (gradientStops != null && gradientStops.Length > 0)
                {
                    return new LinearGradientBrush(
                        linearGradientStart,
                        linearGradientEnd,
                        gradientStops);
                }
                else
                {
                    return new LinearGradientBrush(
                        linearGradientStart,
                        linearGradientEnd,
                        LinearGradientBrush.GetGradientStopsFromEdgeColors(color, endColor));
                }
            }

            switch (brushType)
            {
                case BrushType.None:
                    Brush = null;
                    break;
                case BrushType.Solid:
                    Brush = new SolidBrush(color);
                    break;
                case BrushType.Hatch:
                    Brush = new HatchBrush(hatchStyle, color);
                    break;
                case BrushType.LinearGradient:
                    Brush = CreateLinearGradientBrush();
                    break;
                case BrushType.RadialGradient:
                    Brush = CreateRadialGradientBrush();
                    break;
                default:
                    break;
            }
        }

        /// <inheritdoc/>
        protected override void Load()
        {
            if (Brush == null)
                return;

            brushType = Brush.BrushType;
            color = Brush.BrushColor;

            if (GradientStops.Length > 0)
#pragma warning disable
                endColor = GradientStops[GradientStops.Length - 1].Color;
#pragma warning restore
            else
                endColor = Color.White;

            if (Brush is LinearGradientBrush b)
            {
                linearGradientStart = b.StartPoint;
                linearGradientEnd = b.EndPoint;
                gradientStops = b.GradientStops;
            }
            else
            if (Brush is RadialGradientBrush b2)
            {
                radialGradientCenter = b2.Center;
                radialGradientOrigin = b2.GradientOrigin;
                radialGradientRadius = b2.Radius;
                gradientStops = b2.GradientStops;
            }
            else
            if (Brush is HatchBrush b3)
                hatchStyle = b3.HatchStyle;
        }
    }
}