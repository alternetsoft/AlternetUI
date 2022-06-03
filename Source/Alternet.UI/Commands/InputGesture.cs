using System;

namespace Alternet.UI
{
    /// <summary>
    /// An abstract base class for individual input device gestures.
    /// </summary>
    public abstract class InputGesture
    {
        /// <summary>
        /// Sees if the <see cref="InputGesture"/> matches the input associated with the inputEventArgs
        /// </summary>
        /// <remarks>
        /// Compares an <see cref="InputEventArgs"/> value to Gesture inside.
        /// This method when overriden by derived classes, will match
        /// <see cref="InputEventArgs"/> with its internal values and return a <see langword="true"/>/<see langword="false"/>.
        /// </remarks>
        /// <param name="targetElement">the element to receive the command</param>
        /// <param name="inputEventArgs">inputEventArgs to compare to</param>
        /// <returns>True if matched, false otherwise.
        /// </returns>
        public abstract bool Matches(object targetElement, InputEventArgs inputEventArgs);
    }
}