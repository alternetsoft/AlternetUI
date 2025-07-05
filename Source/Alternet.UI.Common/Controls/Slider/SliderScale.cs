using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a UI control for displaying a slider scale with ticks and labels.
    /// </summary>
    /// <remarks>The <see cref="SliderScale"/> class provides functionality for
    /// rendering a slider scale. It is intended to be used as part of a
    /// <see cref="Container"/> and can be customized or extended as needed.</remarks>
    public partial class SliderScale : Spacer
    {
        /// <summary>
        /// Gets or sets the default tick size for the slider scale.
        /// </summary>
        public static Coord DefaultTickSize = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="SliderScale"/> class.
        /// </summary>
        /// <remarks>This constructor creates a default instance of the
        /// <see cref="SliderScale"/> class.
        /// Use this to set up a slider scale with default settings.</remarks>
        public SliderScale()
        {
            ParentForeColor = true;
            ParentBackColor = true;
        }

        /// <summary>
        /// Gets the container that holds the slider scale.
        /// In the default implementation, this property returns the parent control
        /// that implements the <see cref="ISliderScaleContainer"/> interface.
        /// </summary>
        [Browsable(false)]
        public virtual ISliderScaleContainer? Container
        {
            get
            {
                return Parent as ISliderScaleContainer;
            }
        }

        /// <summary>
        /// Gets the current value of the slider.
        /// </summary>
        [Browsable(false)]
        public int Value
        {
            get
            {
                return Container?.Value ?? 0;
            }
        }

        /// <summary>
        /// Gets the orientation of the slider.
        /// </summary>
        [Browsable(false)]
        public SliderOrientation Orientation
        {
            get
            {
                return Container?.Orientation ?? SliderOrientation.Horizontal;
            }
        }

        /// <summary>
        /// Gets the minimum value of the slider.
        /// </summary>
        [Browsable(false)]
        public int Minimum
        {
            get
            {
                return Container?.Minimum ?? 0;
            }
        }

        /// <summary>
        /// Gets the maximum value that the slider can represent.
        /// </summary>
        [Browsable(false)]
        public int Maximum
        {
            get
            {
                return Container?.Maximum ?? 100;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the slider's orientation is horizontal.
        /// </summary>
        public bool IsHorizontal
        {
            get
            {
                return Orientation == SliderOrientation.Horizontal;
            }
        }

        /// <summary>
        /// Gets the frequency of ticks on the slider scale.
        /// </summary>
        [Browsable(false)]
        public int TickFrequency
        {
            get
            {
                return Container?.TickFrequency ?? 1;
            }
        }

        /// <inheritdoc/>
        public override void DefaultPaint(PaintEventArgs e)
        {
            if(Container == null)
            {
                base.DefaultPaint(e);
                return;
            }

            var min = Minimum;
            var max = Maximum;
            var value = Value;
            var frequency = TickFrequency;

            var firstIndex = Container.Minimum;
            if (!Container.IsFirstTickVisible)
                firstIndex++;

            var lastIndex = Container.Maximum;
            if (!Container.IsLastTickVisible)
                lastIndex--;

            if (IsHorizontal)
            {
                HorizontalDefaultPaint();
            }
            else
            {
                VerticalDefaultPaint();
            }

            void HorizontalDefaultPaint()
            {
                var thumbSize = Container.ThumbSize.Width;
                var scaleHeight = ClientRectangle.Height - Padding.Vertical;
                var isEven = scaleHeight % 2 == 0;
                var tickSize = DefaultTickSize;
                var tickSizeIsEven = tickSize % 2 == 0;
                if(isEven != tickSizeIsEven)
                {
                    tickSize++;
                }

                var top = Padding.Top + ((scaleHeight - tickSize) / 2);

                for (
                    int i = firstIndex;
                    i <= lastIndex;
                    i += Container.TickFrequency)
                {
                    var tickPosition = Container.ScaleValueToPosition(i);

                    e.Graphics.DrawVertLine(
                        RealForegroundColor.AsBrush,
                        (tickPosition + (thumbSize / 2), top),
                        tickSize,
                        1);
                }
            }

            void VerticalDefaultPaint()
            {
                var thumbSize = Container.ThumbSize.Height;
            }
        }
    }
}
