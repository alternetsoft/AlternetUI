#nullable disable

using Alternet.Drawing;
using Alternet.UI;
using System;
using System.ComponentModel;

namespace CustomControlsSample
{
    public class KnobHandler : SliderHandler
    {
        protected override bool NeedsPaint => true;

        public override SliderOrientation Orientation { get; set; }
        public override SliderTickStyle TickStyle { get; set; }

        public override void OnPaint(DrawingContext dc)
        {
            // we need to copy these so we can adjust them
            float knobMinAngle = _minimumAngle;
            float knobMaxAngle = _maximumAngle;
            // adjust them to be within bounds
            if (knobMinAngle < 0)
                knobMinAngle = 360 + knobMinAngle;
            if (knobMaxAngle <= 0)
                knobMaxAngle = 360 + knobMaxAngle;

            double offset = 0.0;
            int min = Minimum, max = Maximum;
            var knobRange = (knobMaxAngle - knobMinAngle);
            double valueRange = max - min;
            double valueRatio = knobRange / valueRange;
            if (0 > min)
                offset = -min;
            var knobRect = ClientRectangle;
            // adjust the client rect so it doesn't overhang
            knobRect.Inflate(-1, -1);
            var orr = knobRect;
            if (TicksVisible)
            {
                // we have to make the knob smaller to make room
                // for the ticks
                knobRect.Inflate(new Size(-_tickHeight - 2, -_tickHeight - 2));
            }
            var size = (float)Math.Min(knobRect.Width - 4, knobRect.Height - 4);
            // give it a bit of a margin:
            knobRect.X += 2;
            knobRect.Y += 2;

            var radius = size / 2f;
            var origin = new Point(knobRect.Left + radius, knobRect.Top + radius);
            var borderRect = _GetCircleRect(origin.X, origin.Y, (radius - (_borderWidth / 2)));
            var knobInnerRect = _GetCircleRect(origin.X, origin.Y, (radius - (_borderWidth)));
            // compute our angle
            double q = ((Value + offset) * valueRatio) + knobMinAngle;
            double angle = (q + 90d);
            if (angle > 360.0)
                angle -= 360.0;
            // now in radians
            double angrad = angle * (Math.PI / 180d);
            // pointer adjustment
            double adj = 1;
            // adjust for endcap
            //if (_pointerEndCap != LineCap.NoAnchor)
            //    adj += (_pointerWidth) / 2d;
            // compute the pointer line coordinates
            var x1 = (origin.X + (_pointerOffset - adj) * Math.Cos(angrad));
            var y1 = (origin.Y + (_pointerOffset - adj) * Math.Sin(angrad));
            var x2 = (origin.X + (radius - adj) * Math.Cos(angrad));
            var y2 = (origin.Y + (radius - adj) * Math.Sin(angrad));

            using (var backBrush = new SolidBrush(SystemColors.Control))
            {
                using (var bgBrush = new SolidBrush(_knobColor))
                {
                    using (var borderPen = new Pen(_borderColor, _borderWidth))
                    {
                        using (var pointerPen = new Pen(_pointerColor, _pointerWidth))
                        {
                            //g.SmoothingMode = SmoothingMode.AntiAlias;

                            //pointerPen.StartCap = _pointerStartCap;
                            //pointerPen.EndCap = _pointerEndCap;
                            pointerPen.LineCap = _pointerStartCap;


                            // erase the background so it antialiases properly
                            dc.FillRectangle(backBrush, new Rect(orr.Left - 1, orr.Top - 1, orr.Width + 2, orr.Height + 2));
                            // draw the border
                            dc.DrawEllipse(borderPen, borderRect);
                            // draw the knob
                            dc.FillEllipse(bgBrush, knobInnerRect);
                            // draw the pointer
                            dc.DrawLine(pointerPen, new Point(x1, y1), new Point(x2, y2));
                        }
                    }
                }
            }

            if (TicksVisible)
            {
                // draw the ticks
                using (var pen = new Pen(_tickColor, _tickWidth))
                {
                    // for each tick line, compute its coordinates
                    // and then draw it
                    for (var i = 0; i < _tickPositions.Length; ++i)
                    {
                        // get the angle from our tick position
                        angle = ((_tickPositions[i] + offset) * valueRatio) + knobMinAngle + 90d;
                        if (angle > 360.0)
                            angle -= 360.0;
                        angrad = angle * (Math.PI / 180d);
                        x1 = origin.X + (radius + 2) * Math.Cos(angrad);
                        y1 = origin.Y + (radius + 2) * Math.Sin(angrad);
                        x2 = origin.X + (radius + _tickHeight + 2) * Math.Cos(angrad);
                        y2 = origin.Y + (radius + _tickHeight + 2) * Math.Sin(angrad);
                        dc.DrawLine(pen, new Point(x1, y1), new Point(x2, y2));
                    }
                }
            }
            
            //// draw the focus rectangle if needed
            //if (Focused)
            //    ControlPaint.DrawFocusRectangle(g, new Rectangle(0, 0, Math.Min(Width, Height), Math.Min(Width, Height)));
        }

        public override Size GetPreferredSize(Size availableSize)
        {
            return new Size(50, 50);
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            UserPaint = true;
            Control.ValueChanged += Control_ValueChanged;
            Control.MouseMove += Control_MouseMove;
            Control.MouseEnter += Control_MouseEnter;
            Control.MouseLeave += Control_MouseLeave;
            Control.MouseLeftButtonDown += Control_MouseLeftButtonDown;
            Control.MouseLeftButtonUp += Control_MouseLeftButtonUp;
            Control.MouseWheel += Control_MouseWheel;

            _RecomputeTicks();
        }

        protected override void OnDetach()
        {
            Control.ValueChanged -= Control_ValueChanged;
            Control.MouseMove -= Control_MouseMove;
            Control.MouseEnter -= Control_MouseEnter;
            Control.MouseLeave -= Control_MouseLeave;
            Control.MouseLeftButtonDown -= Control_MouseLeftButtonDown;
            Control.MouseLeftButtonUp -= Control_MouseLeftButtonUp;
            Control.MouseWheel -= Control_MouseWheel;

            base.OnDetach();
        }

        private void Control_MouseLeave(object sender, EventArgs e)
        {
            Refresh();
        }

        private void Control_MouseEnter(object sender, EventArgs e)
        {
            Refresh();
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            // TODO: Improve Ctrl+Drag
            if (_dragging)
            {
                var location = e.GetPosition(Control);
                var controlDown = Keyboard.IsKeyDown(Key.Control);
                int opos = Value;
                int pos = opos;
                var delta = _dragHit.Y - location.Y;
                if (controlDown)
                    delta *= LargeChange;
                pos += (int)delta;
                int min = Minimum;
                int max = Maximum;
                if (pos < min) pos = min;
                if (pos > max) pos = max;
                if (pos != opos)
                {
                    if (controlDown)
                    {
                        var t = _tickPositions[0];
                        var setVal = false;
                        for (var i = 1; i < _tickPositions.Length; i++)
                        {
                            var t2 = _tickPositions[i] - 1;
                            if (pos >= t && pos <= t2)
                            {
                                var l = pos - t;
                                var l2 = t2 - pos;
                                if (l <= l2)
                                    Value = t;
                                else
                                    Value = t2;
                                setVal = true;
                                break;
                            }
                            t = _tickPositions[i];
                        }
                        if (!setVal)
                            Value = Maximum;

                    }
                    else
                        Value = pos;
                    _dragHit = location;
                }
            }
        }

        private void Control_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CaptureMouse();

            var location = e.GetPosition(Control);

            Focus();
            var knobRect = ClientRectangle;
            // adjust the client rect so it doesn't overhang
            knobRect.Inflate(-1, -1);
            if (TicksVisible)
                knobRect.Inflate(-_tickHeight - 2, -_tickHeight - 2);
            var size = (float)Math.Min(knobRect.Width - 4, knobRect.Height - 4);
            knobRect.X += 2;
            knobRect.Y += 2;
            var radius = size / 2f;
            var origin = new Point(knobRect.Left + radius, knobRect.Top + radius);
            if (radius > _GetLineDistance(origin, location))
            {
                _dragHit = location;
                _dragging = true;
            }
        }

        private void Control_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();

            _dragging = false;
        }

        private void Control_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            int pos;
            int m;
            var delta = e.Delta;
            if (0 < delta)
            {
                delta = 1;
                pos = Value;
                pos += delta;
                m = Maximum;
                if (pos > m)
                    pos = m;
                Value = pos;
            }
            else if (0 > delta)
            {
                delta = -1;
                pos = Value;
                pos += delta;
                m = Minimum;
                if (pos < m)
                    pos = m;
                Value = pos;
            }
        }

        private void Control_ValueChanged(object sender, EventArgs e)
        {
            Refresh();
        }

        int _largeChange = 20;
        int _minimum = 0;
        int _maximum = 100;
        Color _borderColor = SystemColors.ControlDarkDark;
        Color _knobColor = SystemColors.Control;
        Color _pointerColor = SystemColors.ControlText;
        Color _tickColor = SystemColors.ControlDarkDark;
        int _pointerWidth = 1;
        int _pointerOffset = 0;
        int _borderWidth = 1;
        int _minimumAngle = 30;
        int _maximumAngle = 330;
        LineCap _pointerStartCap = LineCap.Round;
        LineCap _pointerEndCap = LineCap.Flat;
        bool _dragging = false;
        Point _dragHit;
        bool _hasTicks = false;
        int _tickHeight = 2;
        int _tickWidth = 1;
        int[] _tickPositions;

        int Value
        {
            get => Control.Value;
            set => Control.Value = value;
        }

        /// <summary>
        /// Indicates the amount the control increments when the large modifiers are used
        /// </summary>
        [Description("Indicates the amount the control increments when the large modifiers are used")]
        [Category("Behavior")]
        [DefaultValue(20)]
        public int LargeChange
        {
            get { return _largeChange; }
            set
            {
                if (_largeChange != value)
                {
                    _largeChange = value;
                    _RecomputeTicks();
                    Invalidate();
                    OnLargeChangeChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Raised with the value of LargeChange changes
        /// </summary>
        [Description("Raised with the value of LargeChange changes")]
        [Category("Behavior")]
        public event EventHandler LargeChangeChanged;

        /// <summary>
        /// Called when the value of LargeChange changes
        /// </summary>
        /// <param name="args">The event args to use</param>
        protected virtual void OnLargeChangeChanged(EventArgs args)
        {
            LargeChangeChanged?.Invoke(this, args);
        }

        /// <summary>
        /// Indicates the minimum value for the control
        /// </summary>
        [Description("Indicates the minimum value for the control")]
        [Category("Behavior")]
        [DefaultValue(0)]
        public int Minimum
        {
            get { return _minimum; }
            set
            {
                if (_minimum != value)
                {
                    _minimum = value;
                    _RecomputeTicks();
                    Invalidate();
                    OnMinimumChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Raised with the value of Minimum changes
        /// </summary>
        [Description("Raised with the value of Minimum changes")]
        [Category("Behavior")]
        public event EventHandler MinimumChanged;
        /// <summary>
        /// Called when the value of Minimum changes
        /// </summary>
        /// <param name="args">The event args to use</param>
        protected virtual void OnMinimumChanged(EventArgs args)
        {
            MinimumChanged?.Invoke(this, args);
        }

        /// <summary>
        /// Indicates the maximum value for the control
        /// </summary>
        [Description("Indicates the maximum value for the control")]
        [Category("Behavior")]
        [DefaultValue(100)]
        public int Maximum
        {
            get { return _maximum; }
            set
            {
                if (_maximum != value)
                {
                    _maximum = value;
                    _RecomputeTicks();
                    Invalidate();
                    OnMaximumChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Raised with the value of Maximum changes
        /// </summary>
        [Description("Raised with the value of Maximum changes")]
        [Category("Behavior")]
        public event EventHandler MaximumChanged;
        /// <summary>
        /// Called when the value of Maximum changes
        /// </summary>
        /// <param name="args">The event args to use</param>
        protected virtual void OnMaximumChanged(EventArgs args)
        {
            MaximumChanged?.Invoke(this, args);
        }
        /// <summary>
        /// Indicates the border color
        /// </summary>
        [Description("Indicates the border color of the control")]
        [Category("Appearance")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                if (value != _borderColor)
                {
                    _borderColor = value;
                    Invalidate();
                    OnBorderColorChanged(EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// Raised when the value of BorderColor changes
        /// </summary>
        [Description("Raised when the value of BorderColor changes")]
        [Category("Behavior")]
        public event EventHandler BorderColorChanged;
        /// <summary>
        /// Called when the value of BorderColor changes
        /// </summary>
        /// <param name="args">The event args (not used)</param>
        protected virtual void OnBorderColorChanged(EventArgs args)
        {
            BorderColorChanged?.Invoke(this, args);
        }
        /// <summary>
        /// Indicates the knob color
        /// </summary>
        [Description("Indicates the knob color of the control")]
        [Category("Appearance")]
        public Color KnobColor
        {
            get { return _knobColor; }
            set
            {
                if (value != _knobColor)
                {
                    _knobColor = value;
                    Invalidate();
                    OnKnobColorChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Raised when the value of KnobColor changes
        /// </summary>
        [Description("Raised when the value of KnobColor changes")]
        [Category("Behavior")]
        public event EventHandler KnobColorChanged;
        /// <summary>
        /// Called when the value of KnobColor changes
        /// </summary>
        /// <param name="args">The event args (not used)</param>
        protected virtual void OnKnobColorChanged(EventArgs args)
        {
            KnobColorChanged?.Invoke(this, args);
        }
        /// <summary>
        /// Indicates the pointer color
        /// </summary>
        [Description("Indicates the pointer color of the control")]
        [Category("Appearance")]
        public Color PointerColor
        {
            get { return _pointerColor; }
            set
            {
                if (value != _pointerColor)
                {
                    _pointerColor = value;
                    Invalidate();
                    OnPointerColorChanged(EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// Raised when the value of PointerColor changes
        /// </summary>
        [Description("Raised when the value of PointerColor changes")]
        [Category("Behavior")]
        public event EventHandler PointerColorChanged;
        /// <summary>
        /// Called when the value of PointerColor changes
        /// </summary>
        /// <param name="args">The event args (not used)</param>
        protected virtual void OnPointerColorChanged(EventArgs args)
        {
            PointerColorChanged?.Invoke(this, args);
        }
        /// <summary>
        /// Indicates the pointer width of the control
        /// </summary>
        [Description("Indicates the pointer width of the control")]
        [Category("Appearance")]
        [DefaultValue(1)]
        public int PointerWidth
        {
            get { return _pointerWidth; }
            set
            {
                if (_pointerWidth != value)
                {
                    _pointerWidth = value;
                    Invalidate();
                    OnPointerWidthChanged(EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// Raised with the value of PointerWidth changes
        /// </summary>
        [Description("Raised with the value of PointerWidth changes")]
        [Category("Behavior")]
        public event EventHandler PointerWidthChanged;
        /// <summary>
        /// Called when the value of PointerWidth changes
        /// </summary>
        /// <param name="args">The event args to use</param>
        protected virtual void OnPointerWidthChanged(EventArgs args)
        {
            PointerWidthChanged?.Invoke(this, args);
        }
        /// <summary>
        /// Indicates the pointer offset for the control
        /// </summary>
        [Description("Indicates the pointer offset for the control")]
        [Category("Appearance")]
        [DefaultValue(0)]
        public int PointerOffset
        {
            get { return _pointerOffset; }
            set
            {
                if (_pointerOffset != value)
                {
                    _pointerOffset = value;
                    Invalidate();
                    OnPointerOffsetChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Raised with the value of PointerOffset changes
        /// </summary>
        [Description("Raised with the value of PointerOffset changes")]
        [Category("Behavior")]
        public event EventHandler PointerOffsetChanged;
        /// <summary>
        /// Called when the value of PointerOffset changes
        /// </summary>
        /// <param name="args">The event args to use</param>
        protected virtual void OnPointerOffsetChanged(EventArgs args)
        {
            PointerOffsetChanged?.Invoke(this, args);
        }
        /// <summary>
        /// Indicates the border width of the control
        /// </summary>
        [Description("Indicates the border width of the control")]
        [Category("Appearance")]
        [DefaultValue(1)]
        public int BorderWidth
        {
            get { return _borderWidth; }
            set
            {
                if (_borderWidth != value)
                {
                    _borderWidth = value;
                    Invalidate();
                    OnBorderWidthChanged(EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// Raised with the value of BorderWidth changes
        /// </summary>
        [Description("Raised with the borderWidth of BorderWidth changes")]
        [Category("Behavior")]
        public event EventHandler BorderWidthChanged;
        /// <summary>
        /// Called when the value of BorderWidth changes
        /// </summary>
        /// <param name="args">The event args to use</param>
        protected virtual void OnBorderWidthChanged(EventArgs args)
        {
            BorderWidthChanged?.Invoke(this, args);
        }
        /// <summary>
        /// Indicates the pointer start line cap of the control
        /// </summary>
        [Description("Indicates the pointer start line cap of the control")]
        [Category("Appearance")]
        [DefaultValue(LineCap.Round)]
        public LineCap PointerStartCap
        {
            get { return _pointerStartCap; }
            set
            {
                if (_pointerStartCap != value)
                {
                    _pointerStartCap = value;
                    Invalidate();
                    OnPointerStartCapChanged(EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// Raised with the value of PointerStartCap changes
        /// </summary>
        [Description("Raised with the value of PointerStartCap changes")]
        [Category("Behavior")]
        public event EventHandler PointerStartCapChanged;
        /// <summary>
        /// Called when the value of PointerStartCap changes
        /// </summary>
        /// <param name="args">The event args to use</param>
        protected virtual void OnPointerStartCapChanged(EventArgs args)
        {
            PointerStartCapChanged?.Invoke(this, args);
        }
        /// <summary>
        /// Indicates the pointer end line cap of the control
        /// </summary>
        [Description("Indicates the pointer end line cap of the control")]
        [Category("Appearance")]
        [DefaultValue(LineCap.Flat)]
        public LineCap PointerEndCap
        {
            get { return _pointerEndCap; }
            set
            {
                if (_pointerEndCap != value)
                {
                    _pointerEndCap = value;
                    Invalidate();
                    OnPointerEndCapChanged(EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// Raised with the value of PointerEndCap changes
        /// </summary>
        [Description("Raised with the value of PointerEndCap changes")]
        [Category("Behavior")]
        public event EventHandler PointerEndCapChanged;
        /// <summary>
        /// Called when the value of PointerEndCap changes
        /// </summary>
        /// <param name="args">The event args to use</param>
        protected virtual void OnPointerEndCapChanged(EventArgs args)
        {
            PointerEndCapChanged?.Invoke(this, args);
        }
        /// <summary>
        /// Indicates the minimum value for the control
        /// </summary>
        [Description("Indicates the minimum angle for the control")]
        [Category("Appearance")]
        [DefaultValue(30)]
        public int MinimumAngle
        {
            get { return _minimumAngle; }
            set
            {
                if (_minimumAngle != value)
                {
                    _minimumAngle = value;
                    Invalidate();
                    OnMinimumAngleChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Raised with the value of MinimumAngle changes
        /// </summary>
        [Description("Raised with the value of MinimumAngle changes")]
        [Category("Behavior")]
        public event EventHandler MinimumAngleChanged;
        /// <summary>
        /// Called when the value of MinimumAngle changes
        /// </summary>
        /// <param name="args">The event args to use</param>
        protected virtual void OnMinimumAngleChanged(EventArgs args)
        {
            MinimumAngleChanged?.Invoke(this, args);
        }

        /// <summary>
        /// Indicates the maximum angle for the control
        /// </summary>
        [Description("Indicates the maximum angle for the control")]
        [Category("Appearance")]
        [DefaultValue(330)]
        public int MaximumAngle
        {
            get { return _maximumAngle; }
            set
            {
                if (_maximumAngle != value)
                {
                    _maximumAngle = value;
                    Invalidate();
                    OnMaximumAngleChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Raised with the value of MaximumAngle changes
        /// </summary>
        [Description("Raised with the value of MaximumAngle changes")]
        [Category("Behavior")]
        public event EventHandler MaximumAngleChanged;
        /// <summary>
        /// Called when the value of MaximumAngle changes
        /// </summary>
        /// <param name="args">The event args to use</param>
        protected virtual void OnMaximumAngleChanged(EventArgs args)
        {
            MaximumAngleChanged?.Invoke(this, args);
        }
        internal bool TicksVisible
        {
            get { return _hasTicks && 0 < _tickHeight && 0 < _tickWidth; }
        }
        /// <summary>
        /// Indicates whether or not the control displays tick marks
        /// </summary>
        [Description("Indicates whether or not the control displays tick marks")]
        [Category("Appearance")]
        [DefaultValue(false)]
        public bool HasTicks
        {
            get { return _hasTicks; }
            set
            {
                if (_hasTicks != value)
                {
                    _hasTicks = value;
                    Invalidate();
                    OnHasTicksChanged(EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// Raised with the value of HasTicks changes
        /// </summary>
        [Description("Raised with the value of HasTicks changes")]
        [Category("Behavior")]
        public event EventHandler HasTicksChanged;
        /// <summary>
        /// Called when the value of HasTicks changes
        /// </summary>
        /// <param name="args">The event args to use</param>
        protected virtual void OnHasTicksChanged(EventArgs args)
        {
            HasTicksChanged?.Invoke(this, args);
        }

        /// <summary>
        /// Indicates the height of the tick marks
        /// </summary>
        [Description("Indicates the height of the tick marks")]
        [Category("Appearance")]
        [DefaultValue(2)]
        public int TickHeight
        {
            get { return _tickHeight; }
            set
            {
                if (_tickHeight != value)
                {
                    _tickHeight = value;
                    Invalidate();
                    OnTickHeightChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Raised with the value of TickHeight changes
        /// </summary>
        [Description("Raised with the value of TickHeight changes")]
        [Category("Behavior")]
        public event EventHandler TickHeightChanged;
        /// <summary>
        /// Called when the value of TickHeight changes
        /// </summary>
        /// <param name="args">The event args to use</param>
        protected virtual void OnTickHeightChanged(EventArgs args)
        {
            TickHeightChanged?.Invoke(this, args);
        }
        /// <summary>
        /// Indicates the width of the tick marks
        /// </summary>
        [Description("Indicates the width of the tick marks")]
        [Category("Appearance")]
        [DefaultValue(1)]
        public int TickWidth
        {
            get { return _tickWidth; }
            set
            {
                if (_tickWidth != value)
                {
                    _tickWidth = value;
                    Invalidate();
                    OnTickWidthChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Raised with the value of TickWidth changes
        /// </summary>
        [Description("Raised with the value of TickWidth changes")]
        [Category("Behavior")]
        public event EventHandler TickWidthChanged;
        /// <summary>
        /// Called when the value of TickWidth changes
        /// </summary>
        /// <param name="args">The event args to use</param>
        protected virtual void OnTickWidthChanged(EventArgs args)
        {
            TickWidthChanged?.Invoke(this, args);
        }

        /// <summary>
        /// Indicates the color of the tick marks
        /// </summary>
        [Description("Indicates the color of the tick marks")]
        [Category("Appearance")]
        public Color TickColor
        {
            get { return _tickColor; }
            set
            {
                if (_tickColor != value)
                {
                    _tickColor = value;
                    Invalidate();
                    OnTickColorChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Raised with the value of TickColor changes
        /// </summary>
        [Description("Raised with the value of TickColor changes")]
        [Category("Behavior")]
        public event EventHandler TickColorChanged;
        /// <summary>
        /// Called when the value of TickColor changes
        /// </summary>
        /// <param name="args">The event args to use</param>
        protected virtual void OnTickColorChanged(EventArgs args)
        {
            TickColorChanged?.Invoke(this, args);
        }

        static Rect _GetCircleRect(double x, double y, double r)
        {
            return new Rect(x - r, y - r, r * 2, r * 2);
        }
        static float _GetLineDistance(Point p1, Point p2)
        {
            var xdist = p1.X - p2.X;
            var ydist = p1.Y - p2.Y;
            return (float)Math.Sqrt(xdist * xdist + ydist * ydist);
        }
        void _RecomputeTicks()
        {
            var tickCount = (int)Math.Ceiling((double)((Maximum - Minimum + 1) / _largeChange));
            var ticks = new int[tickCount + 1];
            ticks[0] = Minimum;
            var t = Minimum;
            for (var i = 1; i < ticks.Length; i++)
            {
                t += _largeChange;
                t = Math.Min(t, Maximum);
                ticks[i] = t;
            }
            _tickPositions = ticks;
        }
    }
}