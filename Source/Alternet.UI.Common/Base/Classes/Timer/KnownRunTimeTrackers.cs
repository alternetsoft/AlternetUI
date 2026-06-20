using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides well-known <see cref="RunTimeTracker"/> instances for tracking
    /// runtime durations of the operations.
    /// </summary>
    public static class KnownRunTimeTrackers
    {
        /// <summary>
        /// Tracks the runtime duration of the default control painting operation.
        /// </summary>
        public static readonly RunTimeTracker DefaultPaint = new("Control.Paint.Default");

        /// <summary>
        /// Tracks the runtime duration of Skia-based control painting operations.
        /// </summary>
        public static readonly RunTimeTracker SkiaPaint = new("Control.Paint.Skia");

        /// <summary>
        /// Tracks the runtime duration of OpenGL-based control painting operations.
        /// </summary>
        public static readonly RunTimeTracker OpenGLPaint = new("Control.Paint.OpenGL");

        /// <summary>
        /// Gets the control currently being tracked for painting operations.
        /// </summary>
        public static UserControl? TrackedControl { get; private set; }

        /// <summary>
        /// Starts tracking the default paint operation for the specified control.
        /// </summary>
        /// <param name="control">The <see cref="UserControl"/> to track.</param>
        [Conditional("DEBUG")]
        public static void DefaultPaintStart(AbstractControl control)
        {
            if (TrackedControl != control)
                return;
            DefaultPaint.Start();
        }

        /// <summary>
        /// Stops tracking the default paint operation for the specified control.
        /// </summary>
        /// <param name="control">The <see cref="UserControl"/> to stop tracking.</param>
        [Conditional("DEBUG")]
        public static void DefaultPaintStop(AbstractControl control)
        {
            if (TrackedControl != control)
                return;
            DefaultPaint.Stop();
        }

        /// <summary>
        /// Starts tracking the runtime duration of Skia-based control painting operations
        /// for the specified control.
        /// </summary>
        /// <param name="control">The <see cref="AbstractControl"/> to track.</param>
        [Conditional("DEBUG")]
        public static void SkiaPaintStart(AbstractControl control)
        {
            if (TrackedControl != control)
                return;
            SkiaPaint.Start();
        }

        /// <summary>
        /// Stops tracking the runtime duration of Skia-based control painting operations
        /// for the specified control.
        /// </summary>
        /// <param name="control">The <see cref="AbstractControl"/> to stop tracking.</param>
        [Conditional("DEBUG")]
        public static void SkiaPaintStop(AbstractControl control)
        {
            if (TrackedControl != control)
                return;
            SkiaPaint.Stop();
        }

        /// <summary>
        /// Starts tracking the runtime duration of OpenGL-based control painting operations
        /// for the specified control.
        /// </summary>
        /// <param name="control">The <see cref="AbstractControl"/> to track.</param>
        [Conditional("DEBUG")]
        public static void OpenGLPaintStart(AbstractControl control)
        {
            if (TrackedControl != control)
                return;
            OpenGLPaint.Start();
        }

        /// <summary>
        /// Stops tracking the runtime duration of OpenGL-based control painting operations
        /// for the specified control.
        /// </summary>
        /// <param name="control">The <see cref="AbstractControl"/> to stop tracking.</param>
        [Conditional("DEBUG")]
        public static void OpenGLPaintStop(AbstractControl control)
        {
            if (TrackedControl != control)
                return;
            OpenGLPaint.Stop();
        }

        /// <summary>
        /// Resets the state of all paint trackers to their default values.
        /// </summary>
        /// <remarks>This method resets the internal state of the default paint tracker,
        /// Skia paint tracker, and OpenGL paint tracker.
        /// It is typically used to clear any accumulated state or prepare the
        /// trackers for reuse.</remarks>
        public static void ResetPaintTrackers()
        {
            DefaultPaint.Reset();
            SkiaPaint.Reset();
            OpenGLPaint.Reset();
        }

        /// <summary>
        /// Tracks the painting of a specified user control by refreshing it a given number of times.
        /// </summary>
        /// <remarks>This method sets the specified control as the currently tracked control,
        /// refreshes it the specified number of times, and then clears the tracking state.
        /// During each refresh, the application
        /// processes pending events to ensure UI responsiveness.</remarks>
        /// <param name="control">The <see cref="UserControl"/> to be tracked and refreshed.
        /// Cannot be <see langword="null"/>.</param>
        /// <param name="repeatCount">The number of times the control should be refreshed.
        /// Must be a non-negative integer.</param>
        public static void TrackPainting(
            UserControl control,
            int repeatCount)
        {
            TrackedControl = control;

            for (int i = 0; i < repeatCount; i++)
            {
                control.Refresh();
                App.DoEvents();
            }

            TrackedControl = null;
        }
    }
}
