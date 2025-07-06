using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Defines a contract for objects that provide slider-related data to visual components
    /// responsible for displaying tick marks, labels, and scale guides.
    /// </summary>
    public interface ISliderScaleContainer
    {
        /// <summary>
        /// Gets the current value selected by the slider.
        /// </summary>
        int Value { get; }

        /// <summary>
        /// Gets the size of the thumb control.
        /// </summary>
        SizeD ThumbSize { get; }

        /// <summary>
        /// Gets the orientation of the slider, indicating whether it is rendered
        /// horizontally or vertically.
        /// </summary>
        SliderOrientation Orientation { get; }

        /// <summary>
        /// Gets the minimum value that can be selected by the slider.
        /// This defines the lower bound of the slider’s value range.
        /// </summary>
        int Minimum { get; }

        /// <summary>
        /// Gets the maximum value that can be selected by the slider.
        /// This defines the upper bound of the slider’s value range.
        /// </summary>
        int Maximum { get; }

        /// <summary>
        /// Gets the interval between ticks rendered along the slider’s scale.
        /// For example, a value of 5 means ticks appear at every fifth value
        /// between Minimum and Maximum.
        /// </summary>
        int TickFrequency { get; }

        /// <summary>
        /// Gets whether the first tick mark is visible on the slider scale.
        /// </summary>
        bool IsFirstTickVisible { get; }

        /// <summary>
        /// Gets whether the last tick mark is visible on the slider scale.
        /// </summary>
        bool IsLastTickVisible { get; }

        /// <summary>
        /// Gets whether the slider scale is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Scales a position value to a coordinate on the slider scale.
        /// </summary>
        /// <param name="val">The position value to be scaled.</param>
        /// <returns>The corresponding coordinate on the slider scale.</returns>
        Coord ScaleValueToPosition(int val);
    }
}
