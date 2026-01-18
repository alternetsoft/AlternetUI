using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI.Extensions;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the method that will handle the event when the splitter colors are resolved.
    /// </summary>
    /// <param name="backColor">The background color for the splitter.</param>
    /// <param name="foreColor">The foreground color for the splitter.</param>
    public delegate void ResolveSplitterColorsDelegate(out Color? backColor, out Color? foreColor);

    /// <summary>
    /// Provides resizing of docked elements. You can dock some control to an edge of a
    /// container using <see cref="AbstractControl.Dock"/> property,
    /// and then dock the splitter to the same edge.
    /// The splitter resizes the control that is previous in the docking order.
    /// </summary>
    [DefaultEvent("SplitterMoved")]
    [DefaultProperty("Dock")]
    public partial class Splitter : HiddenBorderedGraphicControl
    {
        /// <summary>
        /// Specifies the default amount by which the size is adjusted in resizing operations.
        /// This value is used to initialize the <see cref="SizeDelta"/> property of <see cref="Splitter"/> class.
        /// </summary>
        public static int DefaultSizeDelta = 10;

        /// <summary>
        /// Gets or sets a default value of the
        /// <see cref="AbstractControl.ParentBackColor"/> property.
        /// </summary>
        public static bool DefaultParentBackColor = false;

        /// <summary>
        /// Gets or sets default splitter width.
        /// </summary>
        public static Coord DefaultWidth = 5;

        /// <summary>
        /// Gets or sets default splitter background and line color for the light color scheme.
        /// </summary>
        public static IReadOnlyFontAndColor? DefaultLightColors;

        /// <summary>
        /// Gets or sets default splitter background and line color for the dark color theme.
        /// </summary>
        public static IReadOnlyFontAndColor? DefaultDarkColors;

        private Coord minTargetSize = 25;
        private Coord minExtra = 25;
        private PointD anchor = PointD.Empty;
        private AbstractControl? splitTarget;
        private Coord splitSize = -1;
        private Coord splitterThickness;
        private Coord initTargetSize;
        private Coord lastDrawSplit = -1;
        private Coord maxSize;
        private Cursor? defaultCursor;
        private SplitterTargetMode targetMode = SplitterTargetMode.Auto;
        private bool isBackgroundPainted = true;
        private bool isForegroundPainted = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="Splitter"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public Splitter(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Splitter"/> class.
        /// </summary>
        public Splitter()
        {
            var defaultWidth = GetDefaultWidth();

            splitterThickness = defaultWidth;
            TabStop = false;
            CanSelect = false;
            Size = (defaultWidth, defaultWidth);
            Dock = DockStyle.Left;
            ParentBackColor = DefaultParentBackColor;

            // We do not use parent's fore color
            ParentForeColor = false;
        }

        /// <summary>
        /// Occurs when the splitter control is in the process of moving.
        /// </summary>
        [Category("Behavior")]
        public event SplitterEventHandler? SplitterMoving;

        /// <summary>
        /// Occurs when the splitter control is moved.
        /// </summary>
        [Category("Behavior")]
        public event SplitterEventHandler? SplitterMoved;

        /// <summary>
        /// Specifies the possible options for the <see cref="DrawSplitBar"/> method.
        /// </summary>
        /// <remarks>This enumeration is used to indicate the stage of the split
        /// bar drawing process, such
        /// as starting, moving, or ending the drawing operation.</remarks>
        public enum DrawSplitBarKind
        {
            /// <summary>
            /// Splitter is starting to move.
            /// </summary>
            Start = 1,

            /// <summary>
            /// Splitter is moving.
            /// </summary>
            Move = 2,

            /// <summary>
            /// Splitter is ending the move operation.
            /// </summary>
            End = 3,
        }

        /// <summary>
        /// Gets or sets a delegate that resolves the splitter colors.
        /// Use this property to override the default splitter colors resolve method.
        /// </summary>
        public ResolveSplitterColorsDelegate? ResolveSplitterColorsOverride { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the background is painted.
        /// Default is true.
        /// </summary>
        public virtual bool IsBackgroundPainted
        {
            get => isBackgroundPainted;

            set
            {
                if(value == isBackgroundPainted)
                    return;
                isBackgroundPainted = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the foreground is painted.
        /// Default is true.
        /// </summary>
        public virtual bool IsForegroundPainted
        {
            get => isForegroundPainted;

            set
            {
                if(value == isForegroundPainted)
                    return;
                isForegroundPainted = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets splitter background and line color when control state
        /// is <see cref="VisualControlState.Normal"/>.
        /// </summary>
        /// <remarks>
        /// If this property is not assigned, <see cref="DefaultDarkColors"/>
        /// and <see cref="DefaultLightColors"/> are used in order to get colors.
        /// </remarks>
        public virtual IReadOnlyFontAndColor? NormalColors
        {
            get
            {
                return StateObjects?.Colors?.Normal;
            }

            set
            {
                StateObjects ??= new();
                StateObjects.Colors ??= new();
                StateObjects.Colors.Normal = value;
            }
        }

        /// <inheritdoc/>
        [Localizable(true)]
        [DefaultValue(DockStyle.Left)]
        public override DockStyle Dock
        {
            get => base.Dock;

            set
            {
                var valid = value.IsLeftOrRight() || value.IsTopOrBottom();
                if (!valid)
                    value = DockStyle.Left;

                var requestedSize = splitterThickness;

                base.Dock = value;

                if (Dock.IsTopOrBottom())
                {
                    if (splitterThickness != -1)
                        Height = requestedSize;
                }
                else
                {
                    if (splitterThickness != -1)
                        Width = requestedSize;
                }

                Cursor = DefaultCursor;
            }
        }

        /// <summary>
        /// Gets or sets number of dips on which splitter needs to be moved in order to
        /// update attached control size.
        /// </summary>
        /// <remarks>
        /// Default value is stored in <see cref="DefaultSizeDelta"/> static field.
        /// </remarks>
        public virtual int SizeDelta { get; set; } = DefaultSizeDelta;

        /// <summary>
        /// Gets whether the splitter is horizontal.
        /// </summary>
        [Browsable(false)]
        public bool Horizontal => Dock.IsLeftOrRight();

        /// <summary>
        /// Gets or sets minimum size (in dips) of the remaining
        /// area of the container. This area is located in the center of the container that
        /// is not occupied by edge docked controls. This is the area that
        /// would be used for any <see cref="DockStyle.Fill"/> docked control.
        /// </summary>
        [Category("Behavior")]
        [Localizable(true)]
        [DefaultValue(25)]
        public virtual Coord MinExtra
        {
            get
            {
                return minExtra;
            }

            set
            {
                if (value < 0) value = 0;
                minExtra = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum size (in dips) of the target of the splitter.
        /// The target of a splitter is the control that is previous in the docking order
        /// </summary>
        [Category("Behavior")]
        [Localizable(true)]
        [DefaultValue(25)]
        public virtual Coord MinSize
        {
            get
            {
                return minTargetSize;
            }

            set
            {
                if (value < 0) value = 0;
                minTargetSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the mode that determines how the splitter targets its associated elements.
        /// </summary>
        public virtual SplitterTargetMode TargetMode
        {
            get => targetMode;

            set
            {
                targetMode = value;
            }
        }

        /// <inheritdoc/>
        public override RectD Bounds
        {
            get => base.Bounds;
            set
            {
                if (Horizontal)
                {
                    if (value.Width < 1)
                        value.Width = GetDefaultWidth();
                    splitterThickness = value.Width;
                }
                else
                {
                    if (value.Height < 1)
                        value.Height = GetDefaultWidth();
                    splitterThickness = value.Height;
                }

                if (Parent is null)
                {
                    base.Bounds = value;
                    return;
                }

                value.X = Math.Max(value.X, Parent.Padding.Left + Margin.Left);
                value.Y = Math.Max(value.Y, Parent.Padding.Top + Margin.Top);
                value.Right = Math.Min(
                    value.Right,
                    Parent.ClientSize.Width - Parent.Padding.Right - Margin.Right);
                value.Bottom = Math.Min(
                    value.Bottom,
                    Parent.ClientSize.Height - Parent.Padding.Bottom - Margin.Bottom);

                base.Bounds = value;
            }
        }

        /// <summary>
        /// Gets or sets the position of the splitter.
        /// </summary>
        /// <remarks>
        /// If the splitter is not bound to a control, <see cref="SplitPosition"/> equals -1.
        /// </remarks>
        [Category("Layout")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Coord SplitPosition
        {
            get
            {
                if (splitSize == -1) splitSize = CalcSplitSize();
                return splitSize;
            }

            set
            {
                var spd = CalcSplitBounds();

                value = MathUtils.ApplyMinMax(value, minTargetSize, maxSize);

                splitSize = value;
                DrawSplitBar(DrawSplitBarKind.End);

                if (spd.Target == null || Parent is null)
                {
                    splitSize = -1;
                    return;
                }

                var bounds = spd.Target.Bounds;
                switch (Dock)
                {
                    case DockStyle.Top:
                        bounds.Height = value;
                        break;
                    case DockStyle.Bottom:
                        bounds.Y += bounds.Height - splitSize;
                        bounds.Height = value;
                        break;
                    case DockStyle.Left:

                        var mw = Parent.ClientRectangle.Width - Parent.Padding.Right
                            - Margin.Right - Width - MinExtra;

                        bounds.Width = Math.Min(value, mw);
                        break;
                    case DockStyle.Right:
                        bounds.X += bounds.Width - splitSize;
                        bounds.Width = value;
                        break;
                }

                RectI boundsI = bounds.ToRect();
                RectI oldBoundsI = spd.Target.Bounds.ToRect();

                if (boundsI == oldBoundsI)
                    return;

                Parent?.DoInsideUpdate(() =>
                {
                    spd.Target.Bounds = bounds;
                });

                Parent?.Refresh();
                var args = new SplitterEventArgs(
                    Left,
                    Top,
                    Left + (bounds.Width / 2),
                    Top + (bounds.Height / 2));
                OnSplitterMoved(args);
            }
        }

        /// <summary>
        /// Gets default splitter cursor.
        /// </summary>
        [Browsable(false)]
        public virtual Cursor DefaultCursor
        {
            get
            {
                if(defaultCursor != null)
                    return defaultCursor;

                if (Dock.IsTopOrBottom())
                    return Cursors.HSplit;
                else
                    return Cursors.VSplit;
            }

            set
            {
                if(defaultCursor == value)
                    return;
                defaultCursor = value;
                Cursor = DefaultCursor;
            }
        }

        /// <summary>
        /// Resolves the splitter colors based on the current theme and background.
        /// </summary>
        /// <param name="backColor">The background color to be set.</param>
        /// <param name="foreColor">The foreground color to be set.</param>
        public virtual void ResolveSplitterColors(out Color? backColor, out Color? foreColor)
        {
            if(ResolveSplitterColorsOverride is not null)
            {
                ResolveSplitterColorsOverride(out backColor, out foreColor);
                return;
            }

            var colors = NormalColors;

            Color defaultColor;
            if (IsDarkBackground)
            {
                colors ??= DefaultDarkColors;
                if (ParentBackColor)
                    defaultColor = RealBackgroundColor;
                else
                    defaultColor = KnownOSColorConsts.WindowsDark.ExplorerSplitter;
            }
            else
            {
                colors ??= DefaultLightColors;
                if (ParentBackColor)
                    defaultColor = RealBackgroundColor;
                else
                    defaultColor = KnownOSColorConsts.WindowsLight.ExplorerSplitter;
            }

            backColor = colors?.BackgroundColor ?? defaultColor;
            foreColor = colors?.ForegroundColor;
        }

        /// <summary>
        /// Draws the foreground of the splitter.
        /// </summary>
        /// <param name="e">The paint event arguments.</param>
        /// <param name="color">The color to be used for drawing.</param>
        public virtual void DrawSplitterForeground(PaintEventArgs e, Color? color)
        {
            if (color is null)
                return;

            if (Horizontal)
            {
                var vertLine = DrawingUtils.GetCenterLineVert(e.ClientRectangle);
                e.Graphics.FillRectangle(color.AsBrush, vertLine);
            }
            else
            {
                var horzLine = DrawingUtils.GetCenterLineHorz(e.ClientRectangle);
                e.Graphics.FillRectangle(color.AsBrush, horzLine);
            }
        }

        /// <summary>
        /// Draws the background of the splitter.
        /// </summary>
        /// <param name="e">The paint event arguments.</param>
        /// <param name="color">The color to be used for drawing.</param>
        public virtual void DrawSplitterBackground(PaintEventArgs e, Color? color)
        {
            if (color is null)
                return;
            e.Graphics.FillRectangle(color.AsBrush, e.ClientRectangle);
        }

        /// <summary>
        /// Draws the border of the splitter control.
        /// </summary>
        /// <remarks>This method is responsible for rendering the border of the splitter control.
        /// It uses the provided <see cref="PaintEventArgs"/>
        /// to perform the drawing operation.</remarks>
        /// <param name="e">A <see cref="PaintEventArgs"/> object that contains data
        /// for the paint operation.</param>
        public virtual void DrawSplitterBorder(PaintEventArgs e)
        {
            DrawDefaultBackground(e, DrawDefaultBackgroundFlags.DrawBorder);
        }

        /// <inheritdoc/>
        public override void DefaultPaint(PaintEventArgs e)
        {
            ResolveSplitterColors(out var backColor, out var foreColor);
            if(IsBackgroundPainted)
                DrawSplitterBackground(e, backColor);
            if(IsForegroundPainted)
                DrawSplitterForeground(e, foreColor);
            if(HasBorder)
            {
                DrawSplitterBorder(e);
            }
        }

        /// <summary>
        /// Returns a string representation for this control.
        /// </summary>
        public override string ToString()
        {
            var s = base.ToString();
            var sMinExtra = MinExtra.ToString(CultureInfo.CurrentCulture);
            var sMinSize = MinSize.ToString(CultureInfo.CurrentCulture);
            return $"{s}, MinExtra: {sMinExtra}, MinSize: {sMinSize}";
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (splitTarget != null && e.KeyCode == Keys.Escape)
            {
                SplitEnd(false);
                e.Handled = true;
            }
        }

        /// <inheritdoc/>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                var pos = Mouse.GetPosition(this);
                SplitBegin(pos.X, pos.Y);
            }
        }

        /// <inheritdoc/>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (splitTarget != null)
            {
                var pos = Mouse.GetPosition(this);
                var x = pos.X + Left;
                var y = pos.Y + Top;
                var r = CalcSplitLine(GetSplitSize(pos.X, pos.Y), 0);
                var xSplit = r.X;
                var ySplit = r.Y;
                OnSplitterMoving(new SplitterEventArgs(x, y, xSplit, ySplit));
            }
        }

        /// <inheritdoc/>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (splitTarget != null)
                SplitEnd(true);
        }

        /// <summary>
        /// Raises the <see cref="SplitterMoving" /> event. This event occurs while the splitter is
        /// being moved by the user.
        /// </summary>
        /// <param name="e">A <see cref="SplitterEventArgs" /> that contains the event data.</param>
        protected virtual void OnSplitterMoving(SplitterEventArgs e)
        {
            SplitterMoving?.Invoke(this, e);
            if (splitTarget != null)
            {
                if(SplitMove(e.SplitX, e.SplitY))
                    ApplySplitPosition();
            }
        }

        /// <summary>
        /// Gets the default width of the splitter.
        /// </summary>
        /// <returns>The default width of the splitter.</returns>
        protected virtual Coord GetDefaultWidth()
        {
            return DefaultWidth;
        }

        /// <inheritdoc/>
        protected override void OnMouseCaptureLost(EventArgs e)
        {
            base.OnMouseCaptureLost(e);
            if (splitTarget != null)
                SplitEnd(true);
        }

        /// <summary>
        /// Raises the <see cref="SplitterMoved" /> event.
        /// This event occurs when the user finishes moving the splitter.
        /// </summary>
        /// <param name="e">A <see cref="SplitterEventArgs" /> that contains the event data.</param>
        protected virtual void OnSplitterMoved(SplitterEventArgs e)
        {
            SplitterMoved?.Invoke(this, e);
            if (splitTarget != null)
                SplitMove(e.SplitX, e.SplitY);
        }

        /// <summary>
        /// Finds the target of the splitter. For example, if the splitter
        /// is docked right, the target is the control that is just to the right
        /// of the splitter.
        /// </summary>
        protected virtual AbstractControl? FindTarget()
        {
            if(Parent is null)
                return null;

            switch (TargetMode)
            {
                case SplitterTargetMode.Auto:
                default:
                    if (Parent.LayoutFlags.HasFlag(LayoutFlags.IterateBackward))
                        return PreviousVisibleSibling;
                    return NextVisibleSibling;
                case SplitterTargetMode.NextVisibleSibling:
                    return NextVisibleSibling;
                case SplitterTargetMode.PreviousVisibleSibling:
                    return PreviousVisibleSibling;
            }
        }

        /// <summary>
        /// Calculates the current size of the splitter-target.
        /// </summary>
        /// <returns></returns>
        protected virtual Coord CalcSplitSize()
        {
            var target = FindTarget();
            if (target is null) return -1;
            var r = target.Bounds;
            switch (Dock)
            {
                case DockStyle.Top:
                case DockStyle.Bottom:
                    return r.Height;
                case DockStyle.Left:
                case DockStyle.Right:
                    return r.Width;
                default:
                    return -1;
            }
        }

        /// <summary>
        /// Draws the splitter bar at the current location. Will automatically
        /// cleanup anyplace the splitter was drawn previously.
        /// </summary>
        protected virtual void DrawSplitBar(DrawSplitBarKind mode)
        {
            if (mode != DrawSplitBarKind.Start && lastDrawSplit != -1)
            {
                DrawSplitHelper(lastDrawSplit);
                lastDrawSplit = -1;
            }
            else
            if (mode != DrawSplitBarKind.Start && lastDrawSplit == -1)
                return;

            if (mode != DrawSplitBarKind.End)
            {
                DrawSplitHelper(splitSize);
                lastDrawSplit = splitSize;
            }
            else
            {
                if (lastDrawSplit != -1)
                {
                    DrawSplitHelper(lastDrawSplit);
                }

                lastDrawSplit = -1;
            }
        }

        /// <summary>
        /// Calculates the bounding rect of the split line. minWeight refers
        /// to the minimum height or width of the split line.
        /// </summary>
        protected virtual RectD CalcSplitLine(Coord splitSize, Coord minWeight)
        {
            if (splitTarget is null)
                return RectD.Empty;

            RectD r = Bounds;
            RectD bounds = splitTarget.Bounds;
            switch (Dock)
            {
                case DockStyle.Top:
                    if (r.Height < minWeight) r.Height = minWeight;
                    r.Y = bounds.Y + splitSize;
                    break;
                case DockStyle.Bottom:
                    if (r.Height < minWeight) r.Height = minWeight;
                    r.Y = bounds.Y + bounds.Height - splitSize - r.Height;
                    break;
                case DockStyle.Left:
                    if (r.Width < minWeight) r.Width = minWeight;
                    r.X = bounds.X + splitSize;
                    break;
                case DockStyle.Right:
                    if (r.Width < minWeight) r.Width = minWeight;
                    r.X = bounds.X + bounds.Width - splitSize - r.Width;
                    break;
            }

            return r;
        }

        /// <summary>
        /// Gets the bounding criteria for the splitter.
        /// </summary>
        protected virtual SplitData CalcSplitBounds()
        {
            SplitData spd = new();
            var parent = Parent;
            if (parent is null)
                return spd;
            var target = FindTarget();
            spd.Target = target;

            if (target != null)
            {
                if (target.Dock.IsLeftOrRight())
                    initTargetSize = target.Bounds.Width;
                else
                    initTargetSize = target.Bounds.Height;
                var children = parent.GetVisibleChildren();
                int count = children.Count;
                Coord dockWidth = 0, dockHeight = 0;
                for (int i = 0; i < count; i++)
                {
                    AbstractControl ctl = children[i];

                    if (ctl != target)
                    {
                        if (ctl.IgnoreLayout)
                            continue;

                        switch (ctl.Dock)
                        {
                            case DockStyle.Left:
                            case DockStyle.Right:
                                dockWidth += ctl.Width;
                                break;
                            case DockStyle.Top:
                            case DockStyle.Bottom:
                                dockHeight += ctl.Height;
                                break;
                        }
                    }
                }

                SizeD clientSize = parent.ClientSize;
                if (Horizontal)
                    maxSize = clientSize.Width - dockWidth - minExtra - parent.Padding.Horizontal;
                else
                    maxSize = clientSize.Height - dockHeight - minExtra - parent.Padding.Vertical;
                spd.DockWidth = dockWidth;
                spd.DockHeight = dockHeight;
            }

            return spd;
        }

        /// <summary>
        /// Draws the splitter line at the requested location.
        /// </summary>
        /// <param name="splitSize"></param>
        protected virtual void DrawSplitHelper(Coord splitSize)
        {
        }

        /// <summary>
        /// Calculates the split size based on the mouse position (x, y).
        /// </summary>
        protected virtual Coord GetSplitSize(Coord x, Coord y)
        {
            if (splitTarget is null)
                return 0;

            Coord delta;
            if (Horizontal)
                delta = x - anchor.X;
            else
                delta = y - anchor.Y;
            Coord size = 0;
            switch (Dock)
            {
                case DockStyle.Top:
                    size = splitTarget.Height + delta;
                    break;
                case DockStyle.Bottom:
                    size = splitTarget.Height - delta;
                    break;
                case DockStyle.Left:
                    size = splitTarget.Width + delta;
                    break;
                case DockStyle.Right:
                    size = splitTarget.Width - delta;
                    break;
            }

            return Math.Max(Math.Min(size, maxSize), minTargetSize);
        }

        /// <summary>
        /// Begins the splitter moving.
        /// </summary>
        protected virtual void SplitBegin(Coord x, Coord y)
        {
            var spd = CalcSplitBounds();
            if (spd.Target != null && (minTargetSize < maxSize))
            {
                anchor = new PointD(x, y);
                splitTarget = spd.Target;
                splitSize = GetSplitSize(x, y);

                /* here we can hook to global Esc */

                CaptureMouse();
                DrawSplitBar(DrawSplitBarKind.Start);
            }
        }

        /// <summary>
        /// Finishes the split movement.
        /// </summary>
        protected virtual void SplitEnd(bool accept)
        {
            DrawSplitBar(DrawSplitBarKind.End);
            splitTarget = null;
            ReleaseMouseCapture();

            /* here we can unhook to global Esc */

            if (accept)
            {
                /* no need to call ApplySplitPosition() as splitter is live */
            }
            else
            if (splitSize != initTargetSize)
            {
                SplitPosition = initTargetSize;
            }

            anchor = PointD.Empty;
        }

        /// <summary>
        /// Sets the split position to be the current split size.
        /// </summary>
        protected virtual void ApplySplitPosition()
        {
            SplitPosition = splitSize;
        }

        /// <summary>
        /// Moves the splitter line to the splitSize for the mouse position (x, y).
        /// </summary>
        protected virtual bool SplitMove(Coord x, Coord y)
        {
            var size = GetSplitSize(x - Left + anchor.X, y - Top + anchor.Y);
            if (Math.Abs(splitSize - size) >= SizeDelta)
            {
                splitSize = size;
                DrawSplitBar(DrawSplitBarKind.Move);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Represents the dimensions and target control for a docking operation.
        /// </summary>
        /// <remarks>This class is used to store the width and height of a docking area,
        /// as well as the target control involved in the docking process.
        /// The dimensions are represented by the <see cref="Coord"/>
        /// type, and the target control is an instance of <see cref="AbstractControl"/>.</remarks>
        protected class SplitData
        {
            /// <summary>
            /// Gets or sets the width of the splitter area in coordinate units.
            /// </summary>
            public Coord DockWidth = -1;

            /// <summary>
            /// Gets or sets the height of the splitter area.
            /// </summary>
            public Coord DockHeight = -1;

            /// <summary>
            /// Gets or sets the target control that this instance is associated with.
            /// </summary>
            public AbstractControl? Target;
        }
    }
}
