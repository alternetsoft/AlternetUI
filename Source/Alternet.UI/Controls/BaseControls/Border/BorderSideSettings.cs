using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies <see cref="Border"/> drawing settings for the individual side.
    /// </summary>
    public class BorderSideSettings : BaseObject, INotifyPropertyChanged
    {
#pragma warning disable
        public static Color DefaultColor = SystemColors.GrayText;
#pragma warning restore

        private Pen? pen;
        private Brush? brush;
        private double width = 1;
        private Color color = DefaultColor;
        private bool incStartPoint;
        private bool decLength;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets or sets whether to draw border from the start point.
        /// </summary>
        public bool IncStartPoint
        {
            get
            {
                return incStartPoint;
            }

            internal set
            {
                if (incStartPoint == value)
                    return;
                incStartPoint = value;
                RaisePropertyChanged(nameof(IncStartPoint));
            }
        }

        /// <summary>
        /// Gets or sets whether to draw border with full length.
        /// </summary>
        public bool DecLength
        {
            get
            {
                return decLength;
            }

            internal set
            {
                if (decLength == value)
                    return;
                decLength = value;
                RaisePropertyChanged(nameof(DecLength));
            }
        }

        /// <summary>
        /// Gets or sets border color.
        /// </summary>
        public Color Color
        {
            get
            {
                return color;
            }

            set
            {
                if (color == value)
                    return;
                color = value;
                pen = null;
                brush = null;
                RaisePropertyChanged(nameof(Color));
            }
        }

        /// <summary>
        /// Gets or sets border width.
        /// </summary>
        public double Width
        {
            get
            {
                return width;
            }

            set
            {
                if (width == value)
                    return;
                pen = null;
                width = value;
                RaisePropertyChanged(nameof(Width));
            }
        }

        /// <summary>
        /// Gets <see cref="Brush"/> which can be used to draw the border.
        /// </summary>
        public Brush GetBrush(Color defaultColor)
        {
            var c = color ?? defaultColor;

            brush ??= c.AsBrush;
            return brush;
        }

        /// <summary>
        /// Gets <see cref="Pen"/> which can be used to draw the border.
        /// </summary>
        public Pen GetPen(Color defaultColor)
        {
            var c = color ?? defaultColor;

            pen ??= c.GetAsPen(Math.Max(1, width));
            return pen;
        }

        /// <summary>
        /// Assign properties from another object.
        /// </summary>
        /// <param name="value">Source of the properties to assign.</param>
        public void Assign(BorderSideSettings value)
        {
            Width = value.width;
            Color = value.color;
        }

        private void RaisePropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new(propName));
        }
    }
}
