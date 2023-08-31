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
        private Point linearGradientEnd = new Point(1, 1);
        private Point radialGradientCenter = Point.Empty;
        private Point radialGradientOrigin;
        private Color endColor = Color.White;
        private double radialGradientRadius = 10;
        private BrushHatchStyle hatchStyle;
        private bool loaded = false;

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
                if (Brush == null)
                    return BrushType.None;
                if (Brush is SolidBrush)
                    return BrushType.Solid;
                if (Brush is HatchBrush)
                    return BrushType.Hatch;
                if (Brush is LinearGradientBrush)
                    return BrushType.LinearGradient;
                if (Brush is RadialGradientBrush)
                    return BrushType.RadialGradient;
                return brushType;
            }

            set
            {
                LoadFromBrush();
                brushType = value;
                UpdateInstanceProperty();
            }
        }

        /// <summary>
        /// Gets or sets brush color.
        /// </summary>
        public Color Color
        {
            get
            {
                if (Brush is null)
                    return color;
                return Brush.BrushColor;
            }

            set
            {
                LoadFromBrush();
                color = value;
                UpdateInstanceProperty();
            }
        }

        /// <summary>
        /// Gets or sets gradient brush end color.
        /// </summary>
        public Color EndColor
        {
            get
            {
                if(Brush != null)
                {
                    if (GradientStops.Length > 0)
                        return GradientStops[GradientStops.Length - 1].Color;
                    else
                        return Color.White;
                }

                return endColor;
            }

            set
            {
                LoadFromBrush();
                endColor = value;
                UpdateInstanceProperty();
            }
        }

        /// <inheritdoc cref="LinearGradientBrush.StartPoint"/>
        public Point LinearGradientStart
        {
            get
            {
                if (Brush is not LinearGradientBrush b)
                    return linearGradientStart;
                return b.StartPoint;
            }

            set
            {
                LoadFromBrush();
                linearGradientStart = value;
                UpdateInstanceProperty();
            }
        }

        /// <inheritdoc cref="LinearGradientBrush.EndPoint"/>
        public Point LinearGradientEnd
        {
            get
            {
                if (Brush is not LinearGradientBrush b)
                    return linearGradientEnd;
                return b.EndPoint;
            }

            set
            {
                LoadFromBrush();
                linearGradientEnd = value;
                UpdateInstanceProperty();
            }
        }

        /// <inheritdoc cref="RadialGradientBrush.Center"/>
        public Point RadialGradientCenter
        {
            get
            {
                if (Brush is not RadialGradientBrush b)
                    return radialGradientCenter;
                return b.Center;
            }

            set
            {
                LoadFromBrush();
                radialGradientCenter = value;
                UpdateInstanceProperty();
            }
        }

        /// <inheritdoc cref="RadialGradientBrush.GradientOrigin"/>
        public Point RadialGradientOrigin
        {
            get
            {
                if (Brush is not RadialGradientBrush b)
                    return radialGradientOrigin;
                return b.GradientOrigin;
            }

            set
            {
                LoadFromBrush();
                radialGradientOrigin = value;
                UpdateInstanceProperty();
            }
        }

        /// <inheritdoc cref="RadialGradientBrush.Radius"/>
        public double RadialGradientRadius
        {
            get
            {
                if (Brush is not RadialGradientBrush b)
                    return radialGradientRadius;
                return b.Radius;
            }

            set
            {
                LoadFromBrush();
                radialGradientRadius = value;
                UpdateInstanceProperty();
            }
        }

        /// <inheritdoc cref="LinearGradientBrush.GradientStops"/>
        public GradientStop[] GradientStops
        {
            get
            {
                if (Brush is RadialGradientBrush b1)
                    return b1.GradientStops;
                if (Brush is LinearGradientBrush b2)
                    return b2.GradientStops;
                return gradientStops;
            }

            set
            {
                LoadFromBrush();
                gradientStops = value;
                UpdateInstanceProperty();
            }
        }

        /// <inheritdoc cref="HatchBrush.HatchStyle"/>
        public BrushHatchStyle HatchStyle
        {
            get
            {
                if (Brush is not HatchBrush b)
                    return hatchStyle;
                return b.HatchStyle;
            }

            set
            {
                LoadFromBrush();
                hatchStyle = value;
                UpdateInstanceProperty();
            }
        }

        /// <inheritdoc/>
        protected override void UpdateInstanceProperty()
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

        private void LoadFromBrush()
        {
            if (loaded)
                return;
            color = Color;
            endColor = EndColor;
            linearGradientStart = LinearGradientStart;
            linearGradientEnd = LinearGradientEnd;
            radialGradientCenter = RadialGradientCenter;
            radialGradientOrigin = RadialGradientOrigin;
            radialGradientRadius = RadialGradientRadius;
            gradientStops = GradientStops;
            hatchStyle = HatchStyle;
            loaded = true;
        }
    }
}