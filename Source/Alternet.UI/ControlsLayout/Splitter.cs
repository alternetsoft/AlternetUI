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
    /// Provides resizing of docked elements. You can dock some control to an edge of a
    /// container, and then dock the splitter to the same edge.
    /// The splitter resizes the control that is previous in the docking order.
    /// </summary>
    [DefaultEvent("SplitterMoved")]
    [DefaultProperty("Dock")]
    public class Splitter : Control
    {
        /// <summary>
        /// Gets or sets default splitter width.
        /// </summary>
        public static double DefaultWidth = 5;

        private double minSize = 25;
        private double minExtra = 25;
        private PointD anchor = PointD.Empty;
        private Control? splitTarget;
        private double splitSize = -1;
        private double splitterThickness = DefaultWidth;
        private double initTargetSize;
        private double lastDrawSplit = -1;
        private double maxSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="Splitter"/> class.
        /// </summary>
        public Splitter()
        {
            TabStop = false;
            Size = (DefaultWidth, DefaultWidth);
            Dock = DockStyle.Left;
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

        private enum DrawSplitBarKind
        {
            Start = 1,
            Move = 2,
            End = 3,
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
        public double MinExtra
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
        public double MinSize
        {
            get
            {
                return minSize;
            }

            set
            {
                if (value < 0) value = 0;
                minSize = value;
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
        public double SplitPosition
        {
            get
            {
                if (splitSize == -1) splitSize = CalcSplitSize();
                return splitSize;
            }

            set
            {
                var spd = CalcSplitBounds();

                value = MathUtils.ApplyMinMax(value, minSize, maxSize);

                splitSize = value;
                DrawSplitBar(DrawSplitBarKind.End);

                if (spd.Target == null)
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
                        bounds.Width = value;
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

                spd.Target.Bounds = boundsI;
                Application.DoEvents();
                var args = new SplitterEventArgs(
                    Left,
                    Top,
                    Left + (bounds.Width / 2),
                    Top + (bounds.Height / 2));
                OnSplitterMoved(args);
            }
        }

        /// <inheritdoc/>
        public override RectD Bounds
        {
            get
            {
                return base.Bounds;
            }

            set
            {
                if (Horizontal)
                {
                    if (value.Width < 1)
                        value.Width = DefaultWidth;
                    splitterThickness = value.Width;
                }
                else
                {
                    if (value.Height < 1)
                        value.Height = DefaultWidth;
                    splitterThickness = value.Height;
                }

                base.Bounds = value;
            }
        }

        /// <summary>
        /// Gets default splitter cursor.
        /// </summary>
        protected virtual Cursor DefaultCursor
        {
            get
            {
                if (Dock.IsTopOrBottom())
                    return Cursors.HSplit;
                else
                    return Cursors.VSplit;
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
                var pos = e.GetPosition(this);
                SplitBegin(pos.X, pos.Y);
            }
        }

        /// <inheritdoc/>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (splitTarget != null)
            {
                var pos = e.GetPosition(this);
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
                SplitMove(e.SplitX, e.SplitY);
                ApplySplitPosition();
            }
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

        /// <devdoc>
        ///     Draws the splitter bar at the current location. Will automatically
        ///     cleanup anyplace the splitter was drawn previously.
        /// </devdoc>
        /// <internalonly/>
        private void DrawSplitBar(DrawSplitBarKind mode)
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
        /// to the minimum height or width of the splitline.
        /// </summary>
        private RectD CalcSplitLine(double splitSize, double minWeight)
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
        /// Calculates the current size of the splitter-target.
        /// </summary>
        /// <returns></returns>
        private double CalcSplitSize()
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
        /// Gets the bounding criteria for the splitter.
        /// </summary>
        private SplitData CalcSplitBounds()
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
                var children = parent.Children;
                int count = children.Count;
                double dockWidth = 0, dockHeight = 0;
                for (int i = 0; i < count; i++)
                {
                    Control ctl = children[i];
                    if (ctl != target)
                    {
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
                    maxSize = clientSize.Width - dockWidth - minExtra;
                else
                    maxSize = clientSize.Height - dockHeight - minExtra;
                spd.DockWidth = dockWidth;
                spd.DockHeight = dockHeight;
            }

            return spd;
        }

        /// <summary>
        /// Draws the splitter line at the requested location.
        /// </summary>
        /// <param name="splitSize"></param>
        private void DrawSplitHelper(double splitSize)
        {
        }

        /// <summary>
        /// Finds the target of the splitter. For example, if the splitter
        /// is docked right, the target is the control that is just to the right
        /// of the splitter.
        /// </summary>
        private Control? FindTarget()
        {
            return NextSibling;
        }

        /// <summary>
        /// Calculates the split size based on the mouse position (x, y).
        /// </summary>
        private double GetSplitSize(double x, double y)
        {
            if (splitTarget is null)
                return 0;

            double delta;
            if (Horizontal)
                delta = x - anchor.X;
            else
                delta = y - anchor.Y;
            double size = 0;
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

            return Math.Max(Math.Min(size, maxSize), minSize);
        }

        /// <summary>
        /// Begins the splitter moving.
        /// </summary>
        private void SplitBegin(double x, double y)
        {
            var spd = CalcSplitBounds();
            if (spd.Target != null && (minSize < maxSize))
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
        private void SplitEnd(bool accept)
        {
            DrawSplitBar(DrawSplitBarKind.End);
            splitTarget = null;
            ReleaseMouseCapture();

            /* here we can unhook to global Esc */

            if (accept)
            {
                /* no need to call ApplySplitPosition() as splitter is live */
            }
            else if (splitSize != initTargetSize)
            {
                SplitPosition = initTargetSize;
            }

            anchor = PointD.Empty;
        }

        /// <summary>
        /// Sets the split position to be the current split size.
        /// </summary>
        private void ApplySplitPosition()
        {
            SplitPosition = splitSize;
        }

        /// <summary>
        /// Moves the splitter line to the splitSize for the mouse position (x, y).
        /// </summary>
        private void SplitMove(double x, double y)
        {
            var size = GetSplitSize(x - Left + anchor.X, y - Top + anchor.Y);
            if (splitSize != size)
            {
                splitSize = size;
                DrawSplitBar(DrawSplitBarKind.Move);
            }
        }

        private class SplitData
        {
            public double DockWidth = -1;
            public double DockHeight = -1;
            public Control? Target;
        }
    }
}
