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
        private BrushType brushType = BrushType.Solid;
        private Color color = Color.Black;
        private Point linearGradientStart;
        private Point linearGradientEnd;
        private Point radialGradientCenter;
        private Point radialGradientOrigin;
        private double radialGradientRadius;
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
                return BrushType.Solid;
            }

            set
            {
                if (BrushType == value)
                    return;
                brushType = value;
                OnInstancePropertyChanged();
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
                if (Color == value)
                    return;
                color = value;
                OnInstancePropertyChanged();
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
                if (LinearGradientStart == value)
                    return;
                linearGradientStart = value;
                OnInstancePropertyChanged();
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
                if (LinearGradientEnd == value)
                    return;
                linearGradientEnd = value;
                OnInstancePropertyChanged();
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
                if (RadialGradientCenter == value)
                    return;
                radialGradientCenter = value;
                OnInstancePropertyChanged();
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
                if (RadialGradientOrigin == value)
                    return;
                radialGradientOrigin = value;
                OnInstancePropertyChanged();
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
                if (RadialGradientRadius == value)
                    return;
                radialGradientRadius = value;
                OnInstancePropertyChanged();
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
                if (GradientStops == value)
                    return;
                gradientStops = value;
                OnInstancePropertyChanged();
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
                if (HatchStyle == value)
                    return;
                hatchStyle = value;
                OnInstancePropertyChanged();
            }
        }

        private void OnInstancePropertyChanged()
        {
        }
    }
}