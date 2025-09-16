using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Tracks and logs the rendering performance of a
        /// <see cref="UserControl"/> across all supported rendering modes.
        /// </summary>
        /// <remarks>This method evaluates the rendering performance of the specified
        /// <paramref name="control"/> in three rendering modes: SoftwareDoubleBuffered, SkiaSharp,
        /// and SkiaSharpWithOpenGL. For
        /// each mode, the method sets the rendering mode, processes pending application events,
        /// performs the rendering
        /// operation, and logs the results.</remarks>
        /// <param name="control">The <see cref="UserControl"/> to be
        /// tested for rendering performance.</param>
        /// <param name="repeatCount">The number of times to repeat the rendering operation for each mode.
        /// Must be a non-negative integer.</param>
        public static void TrackAllModesPainting(
            UserControl control,
            int repeatCount)
        {
            ControlUtils.SetRenderingMode(control, ControlRenderingMode.SoftwareDoubleBuffered);
            App.DoEvents();
            control.Refresh();
            App.DoEvents();
            ControlUtils.SetRenderingMode(control, ControlRenderingMode.SkiaSharp);
            App.DoEvents();
            control.Refresh();
            App.DoEvents();
            ControlUtils.SetRenderingMode(control, ControlRenderingMode.SkiaSharpWithOpenGL);
            App.DoEvents();
            control.Refresh();
            App.DoEvents();

            ResetPaintTrackers();
            ControlUtils.SetRenderingMode(control, ControlRenderingMode.SoftwareDoubleBuffered);
            App.DoEvents();
            TrackPainting(control, repeatCount);
            App.DoEvents();
            ControlUtils.SetRenderingMode(control, ControlRenderingMode.SkiaSharp);
            App.DoEvents();
            TrackPainting(control, repeatCount);
            App.DoEvents();
            ControlUtils.SetRenderingMode(control, ControlRenderingMode.SkiaSharpWithOpenGL);
            App.DoEvents();
            TrackPainting(control, repeatCount);
            App.DoEvents();
            RunTimeTracker.Log();
        }
    }
}
