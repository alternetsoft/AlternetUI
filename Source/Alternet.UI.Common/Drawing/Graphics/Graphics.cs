using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Alternet.UI;
using Alternet.UI.Localization;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines a custom drawing surface.
    /// </summary>
    public abstract partial class Graphics : DisposableObject, IGraphics, IDisposable
    {
        /// <summary>
        /// Gets half of the <see cref="int"/> maximum value.
        /// </summary>
        internal const Coord HalfOfMaxValue = int.MaxValue / 2;

        private readonly Stack<TransformMatrix> stack = new();

        private TransformMatrix transform = new();
        private GraphicsDocument? document;

        /// <summary>
        /// Gets the current depth of the transform stack.
        /// </summary>
        public int TransformStackDepth => stack?.Count ?? 0;

        /// <summary>
        /// Gets or sets name of the <see cref="Graphics"/> for the debug purposes.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets a copy of the geometric world transformation for this
        /// <see cref="Graphics"/>.
        /// </summary>
        public TransformMatrix Transform
        {
            get
            {
                return transform;
            }

            set
            {
                if (transform == value)
                    return;

                transform = value;

                SetHandlerTransform(value);
            }
        }

        /// <summary>
        /// Gets the displacement values (DX and DY) from the current transformation.
        /// Returns an empty point if no transformation is applied.
        /// </summary>
        /// <value>
        /// A <see cref="PointD"/> representing the translation offset.
        /// </value>
        public PointD TransformDXDY
        {
            get
            {
                if (HasTransform)
                {
                    return (Transform.DX, Transform.DY);
                }
                else
                    return PointD.Empty;
            }
        }

        /// <summary>
        /// Gets whether <see cref="Transform"/> is assigned with transform matrix
        /// which is not the identity matrix.
        /// </summary>
        public bool HasTransform
        {
            get
            {
                return !transform.IsIdentity;
            }
        }

        /// <summary>
        /// Gets used scale factor.
        /// </summary>
        public Coord ScaleFactor
        {
            get
            {
                var dpi = GetDPI().Width;
                var result = GraphicsFactory.ScaleFactorFromDpi(dpi);
                return result;
            }
        }

        /// <summary>
        /// Gets horizontal scale factor.
        /// </summary>
        public Coord HorizontalScaleFactor
        {
            get
            {
                return GraphicsFactory.ScaleFactorFromDpi(GetDPI().Width);
            }
        }

        /// <summary>
        /// Gets vertical scale factor.
        /// </summary>
        public Coord VerticalScaleFactor
        {
            get
            {
                return GraphicsFactory.ScaleFactorFromDpi(GetDPI().Height);
            }
        }

        private GraphicsDocument SafeDocument
        {
            get
            {
                if (document is null)
                {
                    document = new();
                    document.ScaleFactorOverride = ScaleFactor;
                }

                return document;
            }
        }

        /// <summary>
        /// Checks whether <see cref="Brush"/> parameter is ok.
        /// </summary>
        /// <param name="value">Parameter value.</param>
        /// <exception cref="Exception">Raised if parameter is not ok.</exception>
        [Conditional("DEBUG")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DebugBrushAssert(Brush value)
        {
            if (value is null)
                throw new Exception("Brush is null");
            if (value.IsDisposed)
                throw new Exception("Brush was disposed");
        }

        /// <summary>
        /// Updates <paramref name="storage"/> with measurement canvas which can be used
        /// in order to measure text sizes.
        /// </summary>
        /// <param name="storage">Updated measurement canvas.</param>
        /// <param name="prm">The parameters for creating the measurement canvas.</param>
        /// <returns>True if <paramref name="storage"/> contains permanent canvas that
        /// can be kept in memory; False if canvas is temporary and should not be saved.</returns>
        public static bool RequireMeasure(
            [NotNull] ref Graphics? storage,
            CanvasCreateParams prm)
        {
            if (GraphicsFactory.MeasureCanvasOverride is null)
            {
                if(storage is null)
                {
                    storage = GraphicsFactory.GetOrCreateMemoryCanvas(prm);
                }
                else
                {
                    var changed = storage.ScaleFactor != prm.ScaleFactor;

                    if (!changed)
                    {
                        changed = storage.BackendType != prm.GraphicsBackendType;
                    }

                    if (changed)
                    {
                        storage = GraphicsFactory.GetOrCreateMemoryCanvas(prm);
                    }
                }

                return true;
            }
            else
            {
                storage = GraphicsFactory.MeasureCanvasOverride;
                return false;
            }
        }

        /// <summary>
        /// Checks whether <see cref="SolidBrush"/> parameter is ok.
        /// </summary>
        /// <param name="brush">Parameter value.</param>
        /// <exception cref="Exception">Raised if parameter is not ok.</exception>
        [Conditional("DEBUG")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DebugSolidBrushAssert(Brush brush)
        {
            DebugBrushAssert(brush);
            if (brush is not SolidBrush)
            {
                throw new ArgumentException(
                    ErrorMessages.Default.OnlySolidBrushInstancesSupported,
                    nameof(brush));
            }
        }

        /// <summary>
        /// Checks whether <see cref="Pen"/> parameter is ok.
        /// </summary>
        /// <param name="value">Parameter value.</param>
        /// <exception cref="Exception">Raised if parameter is not ok.</exception>
        [Conditional("DEBUG")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DebugPenAssert(Pen value)
        {
            if (value is null)
                throw new Exception("Pen is null");
            if (value.IsDisposed)
                throw new Exception("Pen was disposed");
        }

        /// <summary>
        /// Checks whether <see cref="Pen"/> and <see cref="Brush"/> parameters are ok.
        /// </summary>
        /// <param name="pen">Pen parameter value.</param>
        /// <param name="brush">Brush parameter value.</param>
        /// <exception cref="Exception">Raised if parameters are not ok.</exception>
        [Conditional("DEBUG")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DebugAssert(Pen pen, Brush brush)
        {
            DebugPenAssert(pen);
            DebugBrushAssert(brush);
        }

        /// <summary>
        /// Checks whether array of <see cref="PointD"/> parameter is ok.
        /// </summary>
        /// <param name="points">Parameter value.</param>
        /// <exception cref="Exception">Raised if parameter is not ok.</exception>
        [Conditional("DEBUG")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DebugBezierPointsAssert(PointD[] points)
        {
            var length = points.Length;

            if (length == 0)
                return;

            if ((length - 1) % 3 != 0)
            {
                throw new ArgumentException(
                    "The number of points should be a multiple of 3 plus 1, such as 4, 7, or 10.",
                    nameof(points));
            }
        }

        /// <summary>
        /// Checks whether <see cref="Color"/> parameter is ok.
        /// </summary>
        /// <param name="value">Parameter value.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <exception cref="Exception">Raised if parameter is not ok.</exception>
        [Conditional("DEBUG")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DebugColorAssert(Color value, string? paramName = default)
        {
            if (value is null)
                throw new Exception($"{Fn()} is null");
            if (!value.IsOk)
                throw new Exception($"{Fn()} is not ok");

            string Fn()
            {
                if (paramName is null)
                    return "Color";
                else
                    return $"Color '{paramName}'";
            }
        }

        /// <summary>
        /// Checks whether <see cref="Image"/> parameter is ok.
        /// </summary>
        /// <param name="image">Parameter value.</param>
        /// <exception cref="Exception">Raised if parameter is not ok.</exception>
        [Conditional("DEBUG")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DebugImageAssert(Image image)
        {
            if (image is null)
                throw new Exception("Image is null");
            if (image.IsDisposed)
                throw new Exception("Image was disposed");
            if (image.Width <= 0 || image.Height <= 0)
                throw new Exception("Image has invalid size");
        }

        /// <summary>
        /// Checks whether <see cref="string"/> parameter is ok.
        /// </summary>
        /// <param name="text">Parameter value.</param>
        /// <exception cref="Exception">Raised if parameter is not ok.</exception>
        [Conditional("DEBUG")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DebugTextAssert(string text)
        {
            if (text is null)
                throw new Exception("Text is null");
        }

        /// <summary>
        /// Checks whether <see cref="Font"/> parameter is ok.
        /// </summary>
        /// <param name="font">Parameter value.</param>
        /// <exception cref="Exception">Raised if parameter is not ok.</exception>
        [Conditional("DEBUG")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DebugFontAssert(Font font)
        {
            if (font is null)
                throw new Exception("Font is null");
            if (font.IsDisposed)
                throw new Exception("Font is disposed");
        }

        /// <summary>
        /// Checks whether <see cref="TextFormat"/> parameter is ok.
        /// </summary>
        /// <param name="format">Parameter value.</param>
        /// <exception cref="Exception">Raised if parameter is not ok.</exception>
        [Conditional("DEBUG")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DebugFormatAssert(TextFormat format)
        {
            if (format is null)
                throw new Exception("Text format is null");
        }

        /// <summary>
        /// Creates a new <see cref="Graphics"/> from the specified
        /// <see cref="Image"/>.
        /// </summary>
        /// <param name="image"><see cref="Image"/> from which to create the
        /// new <see cref="Graphics"/>.</param>
        /// <returns>A new <see cref="Graphics"/> for the specified
        /// <see cref="Image"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="image"/>
        /// is <see langword="null"/>.</exception>
        /// <remarks>
        /// Use this method to draw on the specified image.
        /// You should always call the Dispose() method to release
        /// the <see cref="Graphics"/> and
        /// related resources created by the <see cref="FromImage"/> method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Graphics FromImage(Image image)
        {
            DebugImageAssert(image);
            return GraphicsFactory.Handler.CreateGraphicsFromImage(image);
        }

        /// <summary>
        /// Creates <see cref="Graphics"/> that can be used to paint on the screen.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Graphics FromScreen()
        {
            return GraphicsFactory.Handler.CreateGraphicsFromScreen();
        }

        /// <summary>
        /// Draws point at the center of the specified rectangle.
        /// </summary>
        /// <param name="brush">Brush used to fill the rectangle.</param>
        /// <param name="container">Rectangle to use as a container for the point.</param>
        /// <param name="size">Size of the rectangle which is painted at the center.</param>
        public void FillRectangleAtCenter(Brush brush, RectD container, SizeD size)
        {
            RectD rect = ((0, 0), size);
            var alignedRect = AlignUtils.AlignRectInRect(
                rect,
                container,
                HorizontalAlignment.Center,
                VerticalAlignment.Center);
            FillRectangle(brush, alignedRect);
        }

        /// <summary>
        /// Draws point at the center of the specified rectangle.
        /// </summary>
        /// <param name="color">Color of the point.</param>
        /// <param name="container">Rectangle to use as a container for the point.</param>
        public void DrawPointAtCenter(Color color, RectD container)
        {
            FillRectangle(color.AsBrush, container.Center.AsRect(1));
        }

        /// <summary>
        /// Resets transformation matrix in the <see cref="Transform"/> property.
        /// </summary>
        public void ResetTransform()
        {
            Transform = new TransformMatrix();
        }

        /// <summary>
        /// Fills the interior of a rectangle specified by a pair of coordinates,
        /// a width, and a height.
        /// </summary>
        /// <param name="brush">
        /// <see cref="Brush" /> that determines the characteristics of the fill.
        /// </param>
        /// <param name="x">The x-coordinate of the upper-left corner of the rectangle to fill.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the rectangle to fill.</param>
        /// <param name="width">Width of the rectangle to fill.</param>
        /// <param name="height">Height of the rectangle to fill.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FillRectangle(Brush brush, Coord x, Coord y, Coord width, Coord height)
        {
            FillRectangle(brush, (x, y, width, height));
        }

        /// <summary>
        /// Draws debug points on the corners of the specified rectangle.
        /// </summary>
        /// <param name="pen">Color of the debug points. if <c>null</c>, red color is used.</param>
        /// <param name="rect"></param>
        [Conditional("DEBUG")]
        public virtual void DrawDebugPoints(RectD rect, Pen? pen = null)
        {
            pen ??= Pens.Red;

            void DrawDebugPoint(PointD p)
            {
                DrawingUtils.DrawHorzLine(this, pen.AsBrush, p, 1, 1);
            }

            DrawDebugPoint(rect.TopLeft);
            DrawDebugPoint(new PointD(rect.Right - 1, rect.Top));
            DrawDebugPoint(new PointD(rect.Right - 1, rect.Bottom - 1));
            DrawDebugPoint(new PointD(rect.Left, rect.Bottom - 1));
        }

        /// <summary>
        /// Draws a series of rectangles specified by <see cref="RectD"/> structures.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and style
        /// of the outlines of the rectangles.</param>
        /// <param name="rects">Array of <see cref="RectD"/> structures that represent the
        /// rectangles to draw.</param>
        public void DrawRectangles(Pen pen, RectD[] rects)
        {
            for (int i = 0; i < rects.Length; i++)
                DrawRectangle(pen, rects[i]);
        }

        /// <summary>
        /// Fills a series of rectangles specified by <see cref="RectD"/> structures.
        /// </summary>
        /// <param name="brush"><see cref="Brush"/> that determines the characteristics
        /// of the fill.</param>
        /// <param name="rects">Array of <see cref="RectD"/> structures that represent the
        /// rectangles to fill.</param>
        public void FillRectangles(Brush brush, RectD[] rects)
        {
            for (int i = 0; i < rects.Length; i++)
                FillRectangle(brush, rects[i]);
        }

        /// <summary>
        /// Draws a line connecting two points.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and style
        /// of the line.</param>
        /// <param name="x1">X coordinate of the first point.</param>
        /// <param name="y1">Y coordinate of the first point.</param>
        /// <param name="x2">X coordinate of the second point.</param>
        /// <param name="y2">Y coordinate of the second point.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawLine(Pen pen, Coord x1, Coord y1, Coord x2, Coord y2)
        {
            DrawLine(pen, new(x1, y1), new(x2, y2));
        }

        /// <summary>
        /// Draws the specified <see cref="Image"/>, using its original size, at the
        /// specified location.
        /// </summary>
        /// <param name="image"><see cref="Image"/> to draw.</param>
        /// <param name="origin"><see cref="PointD"/> structure that represents the
        /// upper-left corner of the drawn image.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawImageUnscaled(Image image, PointD origin)
            => DrawImage(image, origin);

        /// <summary>
        /// Draws the specified <see cref="Image"/>, using its original size, at the
        /// specified location.
        /// </summary>
        /// <param name="image"><see cref="Image"/> to draw.</param>
        /// <param name="x">Horizontal position of the image.</param>
        /// <param name="y">Vertical position of the image.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawImage(Image image, Coord x, Coord y) => DrawImage(image, (x, y));

        /// <summary>
        /// Creates translation matrix and calls <see cref="PushTransform(TransformMatrix)"/> with it.
        /// </summary>
        /// <param name="offsetX">The X value of the translation matrix.</param>
        /// <param name="offsetY">The Y value of the translation matrix.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PushAndTranslate(Coord offsetX, Coord offsetY)
        {
            if(offsetX == 0 && offsetY == 0)
            {
                PushTransform();
            }
            else
            {
                var transform = TransformMatrix.CreateTranslation(offsetX, offsetY);
                PushTransform(transform);
            }
        }

        /// <summary>
        /// Draws waved line in the specified rectangular area.
        /// </summary>
        /// <param name="rect">Rectangle that bounds the drawing area for the wave.</param>
        /// <param name="color">Color used to draw wave.</param>
        /// <remarks>
        /// This line looks like line drawn by Visual Studio in error position under the text.
        /// Specify rectangle of the text, line is drawn on the bottom. You can pass the rectangle
        /// which is returned by measure text functions.
        /// </remarks>
        public virtual void DrawWave(RectD rect, Color color)
        {
            Draw(this, rect.ToRect(), color);

            static void Draw(Graphics dc, RectI rect, Color color)
            {
                int minSize = 4;
                int offset = 6;

                int left = rect.Left - (rect.Left % offset);
                int i = rect.Right % offset;
                int right = (i != 0) ? rect.Right + (offset - i) : rect.Right;

                int scale = 2;
                int size = (right - left) / scale;

                offset = 3;

                if (size < minSize)
                    size = minSize;
                else
                {
                    i = (int)((size - minSize) / offset);
                    if ((size - minSize) % offset != 0)
                        i++;
                    size = minSize + (i * offset);
                }

                PointD[] pts = new PointD[size];
                for (int index = 0; index < size; index++)
                {
                    pts[index].X = left + (index * scale);
                    pts[index].Y = rect.Bottom - 1;
                    switch (index % 3)
                    {
                        case 0:
                            {
                                pts[index].Y -= scale;
                                break;
                            }

                        case 2:
                            {
                                pts[index].Y += scale;
                                break;
                            }
                    }
                }

                dc.DrawBeziers(color.GetAsPen(1), pts);
            }
        }

        /// <summary>
        /// Measures the specified string when drawn with the specified <see cref="Font"/>.
        /// Calls internally <see cref="GetTextExtent"/>.
        /// </summary>
        /// <param name="text">String to measure.</param>
        /// <param name="font"><see cref="Font"/> that defines the text format of the string.</param>
        /// <returns>
        /// This method returns a <see cref="SizeD"/> structure that represents the size,
        /// in device-independent units, of the
        /// string specified by the <c>text</c> parameter as drawn with the <c>font</c> parameter.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SizeD MeasureText(string text, Font font)
            => GetTextExtent(text, font);

        /// <summary>
        /// Pops a stored transformation matrix from the stack and sets
        /// it to the <see cref="Transform"/> property.
        /// </summary>
        public void PopTransform()
        {
            Transform = stack.Pop();
        }

        /// <summary>
        /// Pushes the current state of the <see cref="Graphics"/> transformation
        /// matrix on a stack
        /// and concatenates the current transform with a new transform.
        /// </summary>
        /// <param name="transform">A transform to concatenate with the current transform.</param>
        public virtual void PushTransform(TransformMatrix transform)
        {
            PushTransform();
            var currentTransform = Transform;
            currentTransform.Multiply(transform);
            Transform = currentTransform;
        }

        /// <summary>
        /// Pushes the current state of the transformation matrix on a stack.
        /// </summary>
        public void PushTransform()
        {
            stack.Push(Transform);
        }

        /// <summary>
        /// Converts point to device-independent units.
        /// </summary>
        /// <param name="point">Point.</param>
        /// <param name="unit">The unit of measure for the point.</param>
        public virtual void ToDip(ref PointD point, GraphicsUnit unit)
        {
            if (unit != GraphicsUnit.Dip)
            {
                var dpi = GetDPI();
                var graphicsType = GraphicsUnitConverter.GraphicsType.Undefined;
                point = GraphicsUnitConverter.ConvertPoint(
                    unit,
                    GraphicsUnit.Dip,
                    dpi,
                    point,
                    graphicsType);
            }
        }

        /// <summary>
        /// Converts point to pixels.
        /// </summary>
        /// <param name="point">Point.</param>
        /// <param name="unit">The unit of measure for the point.</param>
        public virtual void ToPixels(ref PointD point, GraphicsUnit unit)
        {
            if (unit != GraphicsUnit.Pixel)
            {
                var dpi = GetDPI();
                var graphicsType = GraphicsUnitConverter.GraphicsType.Undefined;
                point = GraphicsUnitConverter.ConvertPoint(
                    unit,
                    GraphicsUnit.Pixel,
                    dpi,
                    point,
                    graphicsType);
            }
        }

        /// <summary>
        /// Converts <see cref="SizeD"/> to device-independent units.
        /// </summary>
        /// <param name="size">Size.</param>
        /// <param name="unit">The unit of measure for the size.</param>
        public virtual void ToDip(ref SizeD size, GraphicsUnit unit)
        {
            if (unit != GraphicsUnit.Dip)
            {
                var dpi = GetDPI();
                var graphicsType = GraphicsUnitConverter.GraphicsType.Undefined;
                size = GraphicsUnitConverter.ConvertSize(
                    unit,
                    GraphicsUnit.Dip,
                    dpi,
                    size,
                    graphicsType);
            }
        }

        /// <summary>
        /// Converts rectangle to device-independent units.
        /// </summary>
        /// <param name="rect">Rectangle.</param>
        /// <param name="unit">The unit of measure for the rectangle.</param>
        public virtual void ToDip(ref RectD rect, GraphicsUnit unit)
        {
            if (unit != GraphicsUnit.Dip)
            {
                var dpi = GetDPI();
                var graphicsType = GraphicsUnitConverter.GraphicsType.Undefined;
                rect = GraphicsUnitConverter.ConvertRect(
                    unit,
                    GraphicsUnit.Dip,
                    dpi,
                    rect,
                    graphicsType);
            }
        }

        /// <summary>
        /// Saves the current state of the canvas.
        /// </summary>
        /// <remarks>
        /// This call saves the current state (matrix, clip, and draw filter), and pushes a copy onto
        /// a private stack. Subsequent calls to translate, scale, rotate, skew, concatenate
        /// or clipping path or drawing filter all operate on this copy. When the call
        /// to <see cref="Restore()"/> is made, the previous settings are restored.
        /// This method should be overridden in a derived
        /// class to implement the specific save logic.
        /// </remarks>
        public virtual void Save()
        {
            PushTransform();
        }

        /// <summary>
        /// Restore the saved canvas state.
        /// </summary>
        /// <remarks>
        /// This call balances a previous call to <see cref="Save()"/>, and is used to remove
        /// all modifications to the matrix, clip and draw filter state since the last save call.
        /// It is an error to restore more times than was previously saved.
        /// This method should be overridden in a derived class to implement the specific restore logic.
        /// </remarks>
        public virtual void Restore()
        {
            PopTransform();
        }

        /// <summary>
        /// Ensures that the transform stack depth remains balanced before
        /// and after the specified action is executed.
        /// </summary>
        /// <param name="action">The action to execute. This action is expected to maintain
        /// the balance of the transform stack.</param>
        /// <exception cref="InvalidOperationException">Thrown if the transform stack depth
        /// is unbalanced after the action is executed.</exception>
        public void CheckTransformStackDepth(Action action)
        {
            var initialDepth = stack.Count;
            action();
            if (stack.Count != initialDepth)
                throw new InvalidOperationException("Unbalanced Push/Pop Transform calls.");
        }

        /// <summary>
        /// Represents the parameters required to create a measure canvas.
        /// </summary>
        /// <remarks>This structure is used to encapsulate the configuration options or data necessary
        /// for initializing a measure canvas. The specific parameters should
        /// be defined within this structure to
        /// ensure clarity and maintainability.</remarks>
        public struct CanvasCreateParams : IEquatable<CanvasCreateParams>
        {
            private Coord? scaleFactor;
            private ControlRenderingFlags controlRenderingFlags;
            private int? hashCode;

            /// <summary>
            /// Initializes a new instance of the <see cref="CanvasCreateParams"/> class.
            /// </summary>
            public CanvasCreateParams()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="CanvasCreateParams"/>
            /// class with the specified scale factor.
            /// </summary>
            /// <param name="scaleFactor">The scale factor to be applied to the canvas measurements.
            /// If <see langword="null"/>, a default scale factor will be used.</param>
            public CanvasCreateParams(Coord? scaleFactor)
            {
                this.scaleFactor = scaleFactor;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="CanvasCreateParams"/>
            /// class with the specified scale factor and control rendering flags.
            /// </summary>
            /// <param name="scaleFactor">An optional scaling factor to be applied to the canvas.
            /// If <see langword="null"/>, no scaling is applied.</param>
            /// <param name="controlRenderingFlags">Flags that specify how controls should
            /// be rendered on the canvas.</param>
            public CanvasCreateParams(Coord? scaleFactor, ControlRenderingFlags controlRenderingFlags)
            {
                this.controlRenderingFlags = controlRenderingFlags;
                this.scaleFactor = scaleFactor;
            }

            /// <summary>
            /// Specifies the rendering options for a control.
            /// </summary>
            /// <remarks>This field defines the flags that determine how a control is rendered.
            /// The value is typically a combination of flags from an enumeration,
            /// allowing for fine-grained control over rendering behavior.</remarks>
            public ControlRenderingFlags ControlRenderingFlags
            {
                readonly get => controlRenderingFlags;

                set
                {
                    controlRenderingFlags = value;
                    hashCode = null;
                }
            }

            /// <summary>
            /// Represents a scaling factor for a coordinate system.
            /// </summary>
            /// <remarks>This field can be used to adjust the scale of a coordinate system or
            /// transform. Ensure that the value is appropriately set to avoid
            /// unintended transformations.</remarks>
            public Coord ScaleFactor
            {
                readonly get => GraphicsFactory.ScaleFactorOrDefault(scaleFactor);

                set
                {
                    scaleFactor = value;
                    hashCode = null;
                }
            }

            /// <summary>
            /// Gets the graphics backend type used for rendering.
            /// </summary>
            public readonly GraphicsBackendType GraphicsBackendType
            {
                get
                {
                    if (ControlRenderingFlags.HasFlag(ControlRenderingFlags.UseSkiaSharp))
                        return GraphicsBackendType.SkiaSharp;
                    else
                        return GraphicsBackendType.WxWidgets;
                }
            }

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                return hashCode ??= (ScaleFactor, controlRenderingFlags).GetHashCode();
            }

            /// <inheritdoc/>
            public readonly override string ToString()
            {
                return $"ScaleFactor: {ScaleFactor}, ControlRenderingFlags: {ControlRenderingFlags}";
            }

            /// <inheritdoc/>
            public override bool Equals(object obj)
            {
                return obj is CanvasCreateParams other && Equals(other);
            }

            /// <summary>
            /// Determines whether the current instance is equal to another instance
            /// of <see cref="CanvasCreateParams"/>.
            /// </summary>
            /// <param name="other">The <see cref="CanvasCreateParams"/> instance to compare
            /// with the current instance.</param>
            /// <returns><see langword="true"/> if the current instance is equal
            /// to the <paramref name="other"/> instance; otherwise, <see langword="false"/>.</returns>
            public readonly bool Equals(CanvasCreateParams other)
            {
                return ScaleFactor == other.ScaleFactor &&
                    controlRenderingFlags == other.controlRenderingFlags;
            }
        }
    }
}