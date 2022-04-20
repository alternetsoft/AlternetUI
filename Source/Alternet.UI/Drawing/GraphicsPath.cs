using System;

namespace Alternet.Drawing
{
    /// <summary>
    /// Represents a series of connected lines and curves. This class cannot be inherited.
    /// </summary>
    /// <remarks>
    /// Applications use paths to draw outlines of shapes, fill the interiors of shapes, and create clipping regions.
    /// The graphics engine maintains the coordinates of geometric shapes in a path in world coordinate space.
    /// A path may be composed of any number of figures(subpaths).
    /// Each figure is either composed of a sequence of connected lines and curves or a geometric shape primitive.
    /// The starting point of a figure is the first point in the sequence of connected lines and curves.
    /// The ending point is the last point in the sequence.
    /// The starting and ending points of a geometric shape primitive are defined by the primitive specification.
    /// </remarks>
    internal sealed class GraphicsPath : IDisposable
    {
        // TODO: to be implemented in the future.

        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsPath"/> class.
        /// </summary>
        public GraphicsPath() : this(hasLinesOnly: false)
        {
        }

        internal GraphicsPath(bool hasLinesOnly) : this(new UI.Native.GraphicsPath())
        {
        }

        private GraphicsPath(UI.Native.GraphicsPath nativePath)
        {
            NativePath = nativePath;
        }

        /// <summary>
        /// Appends a series of connected line segments to the end of this <see cref="GraphicsPath"/>.
        /// </summary>
        /// <param name="points">
        /// An array of <see cref="Point"/> structures that represents the points that define the line segments to add.
        /// </param>
        /// <remarks>
        /// If there are previous lines or curves in the figure, a line is added to connect the endpoint
        /// of the previous segment the starting point of the line. The points parameter specifies an array of endpoints.
        /// The first two specify the first line. Each additional point specifies the endpoint of a line segment
        /// whose starting point is the endpoint of the previous line.
        /// </remarks>
        public void AddLines(Point[] points)
        {
            CheckDisposed();

            throw new NotImplementedException();
        }

        internal UI.Native.GraphicsPath NativePath { get; private set; }

        /// <summary>
        /// Releases all resources used by this <see cref="GraphicsPath"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Throws <see cref="ObjectDisposedException"/> if the object has been disposed.
        /// </summary>
        private void CheckDisposed()
        {
            if (isDisposed)
                throw new ObjectDisposedException(null);
        }

        /// <inheritdoc/>
        private void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    NativePath.Dispose();
                    NativePath = null!;
                }

                isDisposed = true;
            }
        }
    }
}