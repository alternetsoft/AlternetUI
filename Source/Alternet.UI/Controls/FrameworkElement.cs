using Alternet.Drawing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace Alternet.UI
{
    public abstract class FrameworkElement : UIElement
    {
        static private readonly Type _typeofThis = typeof(FrameworkElement);

        /// <summary>
        /// Measurement override. Implement your size-to-content logic here.
        /// </summary>
        /// <remarks>
        /// MeasureOverride is designed to be the main customizability point for size control of layout.
        /// Element authors should override this method, call Measure on each child element,
        /// and compute their desired size based upon the measurement of the children.
        /// The return value should be the desired size.<para/>
        /// Note: It is required that a parent element calls Measure on each child or they won't be sized/arranged.
        /// Typical override follows a pattern roughly like this (pseudo-code):
        /// <example>
        ///     <code lang="C#">
        /// <![CDATA[
        ///
        /// protected override Size MeasureOverride(Size availableSize)
        /// {
        ///     foreach (UIElement child in VisualChildren)
        ///     {
        ///         child.Measure(availableSize);
        ///         availableSize.Deflate(child.DesiredSize);
        ///     }
        ///
        ///     Size desired = ... compute sum of children's DesiredSize ...;
        ///     return desired;
        /// }
        /// ]]>
        ///     </code>
        /// </example>
        /// The key aspects of this snippet are:
        ///     <list type="bullet">
        /// <item>You must call Measure on each child element</item>
        /// <item>It is common to cache measurement information between the MeasureOverride and ArrangeOverride method calls</item>
        /// <item>Calling base.MeasureOverride is not required.</item>
        /// <item>Calls to Measure on children are passing either the same availableSize as the parent, or a subset of the area depending
        /// on the type of layout the parent will perform (for example, it would be valid to remove the area
        /// for some border or padding).</item>
        ///     </list>
        /// </remarks>
        /// <param name="availableSize">Available size that parent can give to the child. May be infinity (when parent wants to
        /// measure to content). This is soft constraint. Child can return bigger size to indicate that it wants bigger space and hope
        /// that parent can throw in scrolling...</param>
        /// <returns>Desired Size of the control, given available size passed as parameter.</returns>
        protected abstract Size MeasureOverride(Size availableSize);

        /// <summary>
        /// ArrangeOverride allows for the customization of the positioning of children.
        /// </summary>
        /// <remarks>
        /// Element authors should override this method, call Arrange on each visible child element,
        /// passing final size for each child element via finalSize parameter.
        /// Note: It is required that a parent element calls Arrange on each child or they won't be rendered.
        /// Typical override follows a pattern roughly like this (pseudo-code):
        /// <example>
        ///     <code lang="C#">
        /// <![CDATA[
        ///
        ///
        /// protected override Size ArrangeOverride(Size finalSize)
        /// {
        ///     foreach (UIElement child in VisualChildren)
        ///     {
        ///         child.Arrange(new Rect(childX, childY, childFinalSize));
        ///     }
        ///     return finalSize; //this can be another size if the panel actually takes smaller/larger then finalSize
        /// }
        /// ]]>
        ///     </code>
        /// </example>
        /// </remarks>
        /// <param name="finalSize">The final size that element should use to arrange itself and its children.</param>
        /// <returns>The size that element actually is going to use for rendering. If this size is not the same as finalSize
        /// input parameter, the AlignmentX/AlignmentY properties will position the ink rect of the element
        /// appropriately.</returns>
        protected virtual Size ArrangeOverride(Size finalSize)
        {
            return finalSize;
        }

        private InternalFlags2 _flags2 = InternalFlags2.Default; // Stores Flags (see Flags enum)

        internal bool BypassLayoutPolicies
        {
            get { return ReadInternalFlag2(InternalFlags2.BypassLayoutPolicies); }
            set { WriteInternalFlag2(InternalFlags2.BypassLayoutPolicies, value); }
        }

        internal void WriteInternalFlag2(InternalFlags2 reqFlag, bool set)
        {
            if (set)
            {
                _flags2 |= reqFlag;
            }
            else
            {
                _flags2 &= (~reqFlag);
            }
        }

        internal bool ReadInternalFlag2(InternalFlags2 reqFlag)
        {
            return (_flags2 & reqFlag) != 0;
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Margin"/> property changes.
        /// </summary>
        public event EventHandler? MarginChanged;

        private Thickness margin;

        /// <summary>
        /// Gets or sets the outer margin of an control.
        /// </summary>
        /// <value>Provides margin values for the control. The default value is a <see cref="Thickness"/> with all properties equal to 0 (zero).</value>
        /// <remarks>
        /// The margin is the space between this control and the adjacent control.
        /// Margin is set as a <see cref="Thickness"/> structure rather than as a number so that the margin can be set asymmetrically.
        /// The <see cref="Thickness"/> structure itself supports string type conversion so that you can specify an asymmetric <see cref="Margin"/> in UIXML attribute syntax also.
        /// </remarks>
        public Thickness Margin
        {
            get => margin;
            set
            {
                if (margin == value)
                    return;

                margin = value;

                OnMarginChanged(EventArgs.Empty);
                MarginChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="Margin"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnMarginChanged(EventArgs e)
        {
        }

        /// <summary>
        /// MinHeight Dependency Property
        /// </summary>
        [CommonDependencyProperty]
        public static readonly DependencyProperty MinHeightProperty =
                    DependencyProperty.Register(
                                "MinHeight",
                                typeof(double),
                                _typeofThis,
                                new FrameworkPropertyMetadata(
                                        0d,
                                        FrameworkPropertyMetadataOptions.AffectsMeasure,
                                        new PropertyChangedCallback(OnTransformDirty)),
                                new ValidateValueCallback(IsMinWidthHeightValid));

        private static bool IsMinWidthHeightValid(object value)
        {
            double v = (double)value;
            return (!DoubleUtil.IsNaN(v) && v >= 0.0d && !Double.IsPositiveInfinity(v));
        }

        /// <summary>
        /// MinHeight Property
        /// </summary>
        [TypeConverter(typeof(LengthConverter))]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public double MinHeight
        {
            get { return (double)GetValue(MinHeightProperty); }
            set { SetValue(MinHeightProperty, value); }
        }

        /// <summary>
        /// MaxHeight Dependency Property
        /// </summary>
        [CommonDependencyProperty]
        public static readonly DependencyProperty MaxHeightProperty =
                    DependencyProperty.Register(
                                "MaxHeight",
                                typeof(double),
                                _typeofThis,
                                new FrameworkPropertyMetadata(
                                        Double.PositiveInfinity,
                                        FrameworkPropertyMetadataOptions.AffectsMeasure,
                                        new PropertyChangedCallback(OnTransformDirty)),
                                new ValidateValueCallback(IsMaxWidthHeightValid));

        /// <summary>
        /// MaxHeight Property
        /// </summary>
        [TypeConverter(typeof(LengthConverter))]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public double MaxHeight
        {
            get { return (double)GetValue(MaxHeightProperty); }
            set { SetValue(MaxHeightProperty, value); }
        }

        private static bool IsMaxWidthHeightValid(object value)
        {
            double v = (double)value;
            return (!DoubleUtil.IsNaN(v) && v >= 0.0d);
        }

        private static void OnTransformDirty(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Callback for MinWidth, MaxWidth, Width, MinHeight, MaxHeight, Height, and RenderTransformOffset
            FrameworkElement fe = (FrameworkElement)d;
            fe.AreTransformsClean = false;
        }

        /// <summary>
        /// MaxWidth Dependency Property
        /// </summary>
        [CommonDependencyProperty]
        public static readonly DependencyProperty MaxWidthProperty =
                    DependencyProperty.Register(
                                "MaxWidth",
                                typeof(double),
                                _typeofThis,
                                new FrameworkPropertyMetadata(
                                        Double.PositiveInfinity,
                                        FrameworkPropertyMetadataOptions.AffectsMeasure,
                                        new PropertyChangedCallback(OnTransformDirty)),
                                new ValidateValueCallback(IsMaxWidthHeightValid));


        /// <summary>
        /// MaxWidth Property
        /// </summary>
        [TypeConverter(typeof(LengthConverter))]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public double MaxWidth
        {
            get { return (double)GetValue(MaxWidthProperty); }
            set { SetValue(MaxWidthProperty, value); }
        }


        /// <summary>
        /// MinWidth Dependency Property
        /// </summary>
        [CommonDependencyProperty]
        public static readonly DependencyProperty MinWidthProperty =
                    DependencyProperty.Register(
                                "MinWidth",
                                typeof(double),
                                _typeofThis,
                                new FrameworkPropertyMetadata(
                                        0d,
                                        FrameworkPropertyMetadataOptions.AffectsMeasure,
                                        new PropertyChangedCallback(OnTransformDirty)),
                                new ValidateValueCallback(IsMinWidthHeightValid));

        /// <summary>
        /// MinWidth Property
        /// </summary>
        [TypeConverter(typeof(LengthConverter))]
        [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
        public double MinWidth
        {
            get { return (double)GetValue(MinWidthProperty); }
            set { SetValue(MinWidthProperty, value); }
        }

        private struct MinMax
        {
            internal MinMax(FrameworkElement e)
            {
                maxHeight = e.MaxHeight;
                minHeight = e.MinHeight;
                double l = e.Height;

                double height = (DoubleUtil.IsNaN(l) ? Double.PositiveInfinity : l);
                maxHeight = Math.Max(Math.Min(height, maxHeight), minHeight);

                height = (DoubleUtil.IsNaN(l) ? 0 : l);
                minHeight = Math.Max(Math.Min(maxHeight, height), minHeight);

                maxWidth = e.MaxWidth;
                minWidth = e.MinWidth;
                l = e.Width;

                double width = (DoubleUtil.IsNaN(l) ? Double.PositiveInfinity : l);
                maxWidth = Math.Max(Math.Min(width, maxWidth), minWidth);

                width = (DoubleUtil.IsNaN(l) ? 0 : l);
                minWidth = Math.Max(Math.Min(maxWidth, width), minWidth);
            }

            internal double minWidth;
            internal double maxWidth;
            internal double minHeight;
            internal double maxHeight;
        }

        private static readonly Size DefaultSize = new Size(double.NaN, double.NaN);

        private Size size = DefaultSize;

        /// <summary>
        /// Gets or sets the suggested size of the control.
        /// </summary>
        /// <value>The suggested size of the control, in device-independent units (1/96th inch per unit).
        /// The default value is <see cref="Drawing.Size"/>(<see cref="double.NaN"/>, <see cref="double.NaN"/>)/>.
        /// </value>
        /// <remarks>
        /// This property specifies the suggested size of the control. An actual size is calculated by the layout system.
        /// Set this property to <see cref="Drawing.Size"/>(<see cref="double.NaN"/>, <see cref="double.NaN"/>) to specify auto sizing behavior.
        /// The value of this property is always the same as the value that was set to it and is not changed by the layout system.
        /// </remarks>
        public virtual Size Size
        {
            get
            {
                return size;
            }

            set
            {
                if (size == value)
                    return;

                size = value;
            }
        }

        /// <summary>
        /// Gets or sets the suggested width of the control.
        /// </summary>
        /// <value>The suggested width of the control, in device-independent units (1/96th inch per unit).
        /// The default value is <see cref="double.NaN"/>.
        /// </value>
        /// <remarks>
        /// This property specifies the suggested width of the control. An actual width is calculated by the layout system.
        /// Set this property to <see cref="double.NaN"/> to specify auto sizing behavior.
        /// The value of this property is always the same as the value that was set to it and is not changed by the layout system.
        /// </remarks>
        public virtual double Width
        {
            get => size.Width;

            set
            {
                Size = new Size(value, Size.Height);
            }
        }

        /// <summary>
        /// Gets or sets the suggested height of the control.
        /// </summary>
        /// <value>The suggested height of the control, in device-independent units (1/96th inch per unit).
        /// The default value is <see cref="double.NaN"/>.
        /// </value>
        /// <remarks>
        /// This property specifies the suggested height of the control. An actual height is calculated by the layout system.
        /// Set this property to <see cref="double.NaN"/> to specify auto sizing behavior.
        /// The value of this property is always the same as the value that was set to it and is not changed by the layout system.
        /// </remarks>
        public virtual double Height
        {
            get => size.Height;

            set
            {
                Size = new Size(Size.Width, value);
            }
        }

        /// <summary>
        /// Override for <seealso cref="UIElement.MeasureCore" />.
        /// </summary>
        protected sealed override Size MeasureCore(Size availableSize)
        {

            // If using layout rounding, check whether rounding needs to compensate for high DPI
            bool useLayoutRounding = this.UseLayoutRounding;
            DpiScale dpi = GetDpi();
            if (useLayoutRounding)
            {
                //if (!CheckFlagsAnd(VisualFlags.UseLayoutRounding))
                //{
                //    this.SetFlags(true, VisualFlags.UseLayoutRounding);
                //}
            }

            //build the visual tree from styles first
            //ApplyTemplate();

            if (BypassLayoutPolicies)
            {
                return MeasureOverride(availableSize);
            }
            else
            {
                Thickness margin = Margin;
                double marginWidth = margin.Left + margin.Right;
                double marginHeight = margin.Top + margin.Bottom;

                if (useLayoutRounding && (/*this is ScrollContentPresenter || */!FrameworkAppContextSwitches.DoNotApplyLayoutRoundingToMarginsAndBorderThickness))
                {
                    // Related: WPF popup windows appear in wrong place when
                    // windows is in Medium DPI and a search box changes height
                    // 
                    // ScrollViewer and ScrollContentPresenter depend on rounding their
                    // measurements in a consistent way.  Round the margins first - if we
                    // round the result of (size-margin), the answer might round up or
                    // down depending on size. 
                    marginWidth = RoundLayoutValue(marginWidth, dpi.DpiScaleX);
                    marginHeight = RoundLayoutValue(marginHeight, dpi.DpiScaleY);
                }

                //  parent size is what parent want us to be
                Size frameworkAvailableSize = new Size(
                Math.Max(availableSize.Width - marginWidth, 0),
                Math.Max(availableSize.Height - marginHeight, 0));

                MinMax mm = new MinMax(this);

                if (useLayoutRounding && !FrameworkAppContextSwitches.DoNotApplyLayoutRoundingToMarginsAndBorderThickness)
                {
                    mm.maxHeight = UIElement.RoundLayoutValue(mm.maxHeight, dpi.DpiScaleY);
                    mm.maxWidth = UIElement.RoundLayoutValue(mm.maxWidth, dpi.DpiScaleX);
                    mm.minHeight = UIElement.RoundLayoutValue(mm.minHeight, dpi.DpiScaleY);
                    mm.minWidth = UIElement.RoundLayoutValue(mm.minWidth, dpi.DpiScaleX);
                }

                //LayoutTransformData ltd = LayoutTransformDataField.GetValue(this);
                //{
                //    Transform layoutTransform = this.LayoutTransform;
                //    //  check that LayoutTransform is non-trivial
                //    if (layoutTransform != null && !layoutTransform.IsIdentity)
                //    {
                //        if (ltd == null)
                //        {
                //            //  allocate and store ltd if needed
                //            ltd = new LayoutTransformData();
                //            LayoutTransformDataField.SetValue(this, ltd);
                //        }

                //        ltd.CreateTransformSnapshot(layoutTransform);
                //        ltd.UntransformedDS = new Size();

                //        if (useLayoutRounding)
                //        {
                //            ltd.TransformedUnroundedDS = new Size();
                //        }
                //    }
                //    else if (ltd != null)
                //    {
                //        //  clear ltd storage
                //        ltd = null;
                //        LayoutTransformDataField.ClearValue(this);
                //    }
                //}

                //if (ltd != null)
                //{
                //    // Find the maximal area rectangle in local (child) space that we can fit, post-transform
                //    // in the decorator's measure constraint.
                //    frameworkAvailableSize = FindMaximalAreaLocalSpaceRect(ltd.Transform, frameworkAvailableSize);
                //}

                frameworkAvailableSize.Width = Math.Max(mm.minWidth, Math.Min(frameworkAvailableSize.Width, mm.maxWidth));
                frameworkAvailableSize.Height = Math.Max(mm.minHeight, Math.Min(frameworkAvailableSize.Height, mm.maxHeight));

                // If layout rounding is enabled, round available size passed to MeasureOverride.
                if (useLayoutRounding)
                {
                    frameworkAvailableSize = UIElement.RoundLayoutSize(frameworkAvailableSize, dpi.DpiScaleX, dpi.DpiScaleY);
                }

                //  call to specific layout to measure
                Size desiredSize = MeasureOverride(frameworkAvailableSize);

                //  maximize desiredSize with user provided min size
                desiredSize = new Size(
                    Math.Max(desiredSize.Width, mm.minWidth),
                    Math.Max(desiredSize.Height, mm.minHeight));

                //here is the "true minimum" desired size - the one that is
                //for sure enough for the control to render its content.
                Size unclippedDesiredSize = desiredSize;

                //if (ltd != null)
                //{
                //    //need to store unclipped, untransformed desired size to be able to arrange later
                //    ltd.UntransformedDS = unclippedDesiredSize;

                //    //transform unclipped desired size
                //    Rect unclippedBoundsTransformed = Rect.Transform(new Rect(0, 0, unclippedDesiredSize.Width, unclippedDesiredSize.Height), ltd.Transform.Value);
                //    unclippedDesiredSize.Width = unclippedBoundsTransformed.Width;
                //    unclippedDesiredSize.Height = unclippedBoundsTransformed.Height;
                //}

                bool clipped = false;

                // User-specified max size starts to "clip" the control here.
                //Starting from this point desiredSize could be smaller then actually
                //needed to render the whole control
                if (desiredSize.Width > mm.maxWidth)
                {
                    desiredSize.Width = mm.maxWidth;
                    clipped = true;
                }

                if (desiredSize.Height > mm.maxHeight)
                {
                    desiredSize.Height = mm.maxHeight;
                    clipped = true;
                }

                ////transform desired size to layout slot space
                //if (ltd != null)
                //{
                //    Rect childBoundsTransformed = Rect.Transform(new Rect(0, 0, desiredSize.Width, desiredSize.Height), ltd.Transform.Value);
                //    desiredSize.Width = childBoundsTransformed.Width;
                //    desiredSize.Height = childBoundsTransformed.Height;
                //}

                //  because of negative margins, clipped desired size may be negative.
                //  need to keep it as doubles for that reason and maximize with 0 at the
                //  very last point - before returning desired size to the parent.
                double clippedDesiredWidth = desiredSize.Width + marginWidth;
                double clippedDesiredHeight = desiredSize.Height + marginHeight;

                // In overconstrained scenario, parent wins and measured size of the child,
                // including any sizes set or computed, can not be larger then
                // available size. We will clip the guy later.
                if (clippedDesiredWidth > availableSize.Width)
                {
                    clippedDesiredWidth = availableSize.Width;
                    clipped = true;
                }

                if (clippedDesiredHeight > availableSize.Height)
                {
                    clippedDesiredHeight = availableSize.Height;
                    clipped = true;
                }

                //// Set transformed, unrounded size on layout transform, if any.
                //if (ltd != null)
                //{
                //    ltd.TransformedUnroundedDS = new Size(Math.Max(0, clippedDesiredWidth), Math.Max(0, clippedDesiredHeight));
                //}

                // If using layout rounding, round desired size.
                if (useLayoutRounding)
                {
                    clippedDesiredWidth = UIElement.RoundLayoutValue(clippedDesiredWidth, dpi.DpiScaleX);
                    clippedDesiredHeight = UIElement.RoundLayoutValue(clippedDesiredHeight, dpi.DpiScaleY);
                }

                //  Note: unclippedDesiredSize is needed in ArrangeCore,
                //  because due to the layout protocol, arrange should be called
                //  with constraints greater or equal to child's desired size
                //  returned from MeasureOverride. But in most circumstances
                //  it is possible to reconstruct original unclipped desired size.
                //  In such cases we want to optimize space and save 16 bytes by
                //  not storing it on each FrameworkElement.
                //
                //  The if statement conditions below lists the cases when
                //  it is NOT possible to recalculate unclipped desired size later
                //  in ArrangeCore, thus we save it into Uncommon Fields...
                //
                //  Note 2: use SizeBox to avoid CLR boxing of Size.
                //  measurements show it is better to allocate an object once than
                //  have spurious boxing allocations on every resize
                SizeBox sb = UnclippedDesiredSizeField.GetValue(this);
                if (clipped
                    || clippedDesiredWidth < 0
                    || clippedDesiredHeight < 0)
                {
                    if (sb == null) //not yet allocated, allocate the box
                    {
                        sb = new SizeBox(unclippedDesiredSize);
                        UnclippedDesiredSizeField.SetValue(this, sb);
                    }
                    else //we already have allocated size box, simply change it
                    {
                        sb.Width = unclippedDesiredSize.Width;
                        sb.Height = unclippedDesiredSize.Height;
                    }
                }
                else
                {
                    if (sb != null)
                        UnclippedDesiredSizeField.ClearValue(this);
                }

                return new Size(Math.Max(0, clippedDesiredWidth), Math.Max(0, clippedDesiredHeight));
            }
        }

        private static readonly UncommonField<SizeBox> UnclippedDesiredSizeField = new UncommonField<SizeBox>();

        /// <summary>
        /// This is the method layout parent uses to set a location of the child
        /// relative to parent's visual as a result of layout. Typically, this is called
        /// by the parent inside of its ArrangeOverride implementation after calling Arrange on a child.
        /// Note that this method resets layout tarnsform set by <see cref="InternalSetLayoutTransform"/> method,
        /// so only one of these two should be used by the parent.
        /// </summary>
        private void SetLayoutOffset(Vector offset, Size oldRenderSize)
        {
            //
            // Attempt to avoid changing the transform more often than needed,
            // such as when a parent is arrange dirty but its children aren't.
            //
            // The dependencies for VisualTransform are as follows:
            //     Mirror
            //         RenderSize.Width
            //         FlowDirection
            //         parent.FlowDirection
            //     RenderTransform
            //         RenderTransformOrigin
            //     LayoutTransform
            //         RenderSize
            //         Width, MinWidth, MaxWidth
            //         Height, MinHeight, MaxHeight
            //
            // The AreTransformsClean flag will be false (dirty) when FlowDirection,
            // RenderTransform, LayoutTransform, Min/Max/Width/Height, or
            // RenderTransformOrigin changes.
            //
            // RenderSize is compared here with the previous size to see if it changed.
            //
            //if (!AreTransformsClean || !DoubleUtil.AreClose(RenderSize, oldRenderSize))
            //{
            //    Transform additionalTransform = GetFlowDirectionTransform(); //rtl

            //    Transform renderTransform = this.RenderTransform;
            //    if (renderTransform == Transform.Identity) renderTransform = null;

            //    LayoutTransformData ltd = LayoutTransformDataField.GetValue(this);

            //    TransformGroup t = null;

            //    //arbitrary transform, create a collection
            //    if (additionalTransform != null
            //        || renderTransform != null
            //        || ltd != null)
            //    {
            //        // Create a TransformGroup and make sure it does not participate
            //        // in the InheritanceContext treeness because it is internal operation only.
            //        t = new TransformGroup();
            //        t.CanBeInheritanceContext = false;
            //        t.Children.CanBeInheritanceContext = false;

            //        if (additionalTransform != null)
            //            t.Children.Add(additionalTransform);

            //        if (ltd != null)
            //        {
            //            t.Children.Add(ltd.Transform);

            //            // see if  MaxWidth/MaxHeight limit the element
            //            MinMax mm = new MinMax(this);

            //            //this is in element's local rendering coord system
            //            Size inkSize = this.RenderSize;

            //            double maxWidthClip = (Double.IsPositiveInfinity(mm.maxWidth) ? inkSize.Width : mm.maxWidth);
            //            double maxHeightClip = (Double.IsPositiveInfinity(mm.maxHeight) ? inkSize.Height : mm.maxHeight);

            //            //get the size clipped by the MaxWidth/MaxHeight/Width/Height
            //            inkSize.Width = Math.Min(inkSize.Width, mm.maxWidth);
            //            inkSize.Height = Math.Min(inkSize.Height, mm.maxHeight);

            //            Rect inkRectTransformed = Rect.Transform(new Rect(inkSize), ltd.Transform.Value);

            //            t.Children.Add(new TranslateTransform(-inkRectTransformed.X, -inkRectTransformed.Y));
            //        }

            //        if (renderTransform != null)
            //        {
            //            Point origin = GetRenderTransformOrigin();
            //            bool hasOrigin = (origin.X != 0d || origin.Y != 0d);
            //            if (hasOrigin)
            //            {
            //                TranslateTransform backOrigin = new TranslateTransform(-origin.X, -origin.Y);
            //                backOrigin.Freeze();
            //                t.Children.Add(backOrigin);
            //            }

            //            //can not freeze render transform - it can be animated
            //            t.Children.Add(renderTransform);

            //            if (hasOrigin)
            //            {
            //                TranslateTransform forwardOrigin = new TranslateTransform(origin.X, origin.Y);
            //                forwardOrigin.Freeze();
            //                t.Children.Add(forwardOrigin);
            //            }

            //        }
            //    }

            //    this.VisualTransform = t;
            //    AreTransformsClean = true;
            //}

            Vector oldOffset = this.VisualOffset;
            if (!DoubleUtil.AreClose(oldOffset.X, offset.X) ||
               !DoubleUtil.AreClose(oldOffset.Y, offset.Y))
            {
                this.VisualOffset = offset;
            }
        }

        private VerticalAlignment verticalAlignment = VerticalAlignment.Stretch;
        private HorizontalAlignment horizontalAlignment = HorizontalAlignment.Stretch;

        /// <summary>
        /// Gets or sets the vertical alignment applied to this control when it is positioned within a parent control.
        /// </summary>
        /// <value>A vertical alignment setting. The default is <see cref="VerticalAlignment.Stretch"/>.</value>
        public VerticalAlignment VerticalAlignment
        {
            get => verticalAlignment;
            set
            {
                if (verticalAlignment == value)
                    return;

                verticalAlignment = value;
                VerticalAlignmentChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the horizontal alignment applied to this control when it is positioned within a parent control.
        /// </summary>
        /// <value>A horizontal alignment setting. The default is <see cref="HorizontalAlignment.Stretch"/>.</value>
        public HorizontalAlignment HorizontalAlignment
        {
            get => horizontalAlignment;
            set
            {
                if (horizontalAlignment == value)
                    return;

                horizontalAlignment = value;
                HorizontalAlignmentChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="VerticalAlignment"/> property changes.
        /// </summary>
        public event EventHandler? VerticalAlignmentChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="HorizontalAlignment"/> property changes.
        /// </summary>
        public event EventHandler? HorizontalAlignmentChanged;

        /// <summary>
        /// Gets and sets the offset.
        /// </summary>
        protected internal abstract Vector VisualOffset { get; protected set; }

        /// <summary>
        /// Override for <seealso cref="UIElement.ArrangeCore" />.
        /// </summary>
        protected sealed override void ArrangeCore(Rect finalRect)
        {
            // If using layout rounding, check whether rounding needs to compensate for high DPI
            bool useLayoutRounding = this.UseLayoutRounding;
            DpiScale dpi = GetDpi();
            //LayoutTransformData ltd = LayoutTransformDataField.GetValue(this);
            //Size transformedUnroundedDS = Size.Empty;

            if (useLayoutRounding)
            {
                //if (!CheckFlagsAnd(VisualFlags.UseLayoutRounding))
                //{
                //    SetFlags(true, VisualFlags.UseLayoutRounding);
                //}
            }

            if (BypassLayoutPolicies)
            {
                Size oldRenderSize = RenderSize;
                Size inkSize = ArrangeOverride(finalRect.Size);
                RenderSize = inkSize;
                SetLayoutOffset(new Vector(finalRect.X, finalRect.Y), oldRenderSize);
            }
            else
            {
                // If LayoutConstrained==true (parent wins in layout),
                // we might get finalRect.Size smaller then UnclippedDesiredSize.
                // Stricltly speaking, this may be the case even if LayoutConstrained==false (child wins),
                // since who knows what a particualr parent panel will try to do in error.
                // In this case we will not actually arrange a child at a smaller size,
                // since the logic of the child does not expect to receive smaller size
                // (if it coudl deal with smaller size, it probably would accept it in MeasureOverride)
                // so lets replace the smaller arreange size with UnclippedDesiredSize
                // and then clip the guy later.
                // We will use at least UnclippedDesiredSize to compute arrangeSize of the child, and
                // we will use layoutSlotSize to compute alignments - so the bigger child can be aligned within
                // smaller slot.

                // This is computed on every ArrangeCore. Depending on LayoutConstrained, actual clip may apply or not
                // NeedsClipBounds = false; // yezo

                // Start to compute arrange size for the child.
                // It starts from layout slot or deisred size if layout slot is smaller then desired,
                // and then we reduce it by margins, apply Width/Height etc, to arrive at the size
                // that child will get in its ArrangeOverride.
                Size arrangeSize = finalRect.Size;

                Thickness margin = Margin;
                double marginWidth = margin.Left + margin.Right;
                double marginHeight = margin.Top + margin.Bottom;
                if (useLayoutRounding && !FrameworkAppContextSwitches.DoNotApplyLayoutRoundingToMarginsAndBorderThickness)
                {
                    marginWidth = UIElement.RoundLayoutValue(marginWidth, dpi.DpiScaleX);
                    marginHeight = UIElement.RoundLayoutValue(marginHeight, dpi.DpiScaleY);
                }
                arrangeSize.Width = Math.Max(0, arrangeSize.Width - marginWidth);
                arrangeSize.Height = Math.Max(0, arrangeSize.Height - marginHeight);

                // First, get clipped, transformed, unrounded size.
                if (useLayoutRounding)
                {
                    //// 'transformUnroundedDS' is a non-nullable value type and can never be null.
                    //if (ltd != null)
                    //{
                    //    transformedUnroundedDS = ltd.TransformedUnroundedDS;
                    //    transformedUnroundedDS.Width = Math.Max(0, transformedUnroundedDS.Width - marginWidth);
                    //    transformedUnroundedDS.Height = Math.Max(0, transformedUnroundedDS.Height - marginHeight);
                    //}
                }

                // Next, compare against unclipped, transformed size.
                SizeBox sb = UnclippedDesiredSizeField.GetValue(this);
                Size unclippedDesiredSize;
                if (sb == null)
                {
                    unclippedDesiredSize = new Size(Math.Max(0, this.DesiredSize.Width - marginWidth),
                                                    Math.Max(0, this.DesiredSize.Height - marginHeight));

                    //// There is no unclipped desired size, so check against clipped, but unrounded DS.
                    //if (transformedUnroundedDS != Size.Empty)
                    //{
                    //    unclippedDesiredSize.Width = Math.Max(transformedUnroundedDS.Width, unclippedDesiredSize.Width);
                    //    unclippedDesiredSize.Height = Math.Max(transformedUnroundedDS.Height, unclippedDesiredSize.Height);
                    //}
                }
                else
                {
                    unclippedDesiredSize = new Size(sb.Width, sb.Height);
                }

                if (DoubleUtil.LessThan(arrangeSize.Width, unclippedDesiredSize.Width))
                {
                    // NeedsClipBounds = true;
                    arrangeSize.Width = unclippedDesiredSize.Width;
                }

                if (DoubleUtil.LessThan(arrangeSize.Height, unclippedDesiredSize.Height))
                {
                    // NeedsClipBounds = true;
                    arrangeSize.Height = unclippedDesiredSize.Height;
                }

                // Alignment==Stretch --> arrange at the slot size minus margins
                // Alignment!=Stretch --> arrange at the unclippedDesiredSize
                if (HorizontalAlignment != HorizontalAlignment.Stretch)
                {
                    arrangeSize.Width = unclippedDesiredSize.Width;
                }

                if (VerticalAlignment != VerticalAlignment.Stretch)
                {
                    arrangeSize.Height = unclippedDesiredSize.Height;
                }

                ////if LayoutTransform is set, arrange at untransformed DS always
                ////alignments apply to the BoundingBox after transform
                //if (ltd != null)
                //{
                //    // Repeat the measure-time algorithm for finding a best fit local rect.
                //    // This essentially implements Stretch in case of LayoutTransform
                //    Size potentialArrangeSize = FindMaximalAreaLocalSpaceRect(ltd.Transform, arrangeSize);
                //    arrangeSize = potentialArrangeSize;

                //    // If using layout rounding, round untransformed desired size - in MeasureCore, this value is first transformed and clipped
                //    // before rounding, and hence saved unrounded.
                //    unclippedDesiredSize = ltd.UntransformedDS;

                //    //only use max area rect if both dimensions of it are larger then
                //    //desired size - replace with desired size otherwise
                //    if (!DoubleUtil.IsZero(potentialArrangeSize.Width)
                //        && !DoubleUtil.IsZero(potentialArrangeSize.Height))
                //    {
                //        //Use less precise comparision - otherwise FP jitter may cause drastic jumps here
                //        if (LayoutDoubleUtil.LessThan(potentialArrangeSize.Width, unclippedDesiredSize.Width)
                //           || LayoutDoubleUtil.LessThan(potentialArrangeSize.Height, unclippedDesiredSize.Height))
                //        {
                //            arrangeSize = unclippedDesiredSize;
                //        }
                //    }

                //    //if pre-transformed into local space arrangeSize is smaller in any dimension then
                //    //unclipped local DesiredSize of the element, extend the arrangeSize but
                //    //remember that we potentially need to clip the result of such arrange.
                //    if (DoubleUtil.LessThan(arrangeSize.Width, unclippedDesiredSize.Width))
                //    {
                //        NeedsClipBounds = true;
                //        arrangeSize.Width = unclippedDesiredSize.Width;
                //    }

                //    if (DoubleUtil.LessThan(arrangeSize.Height, unclippedDesiredSize.Height))
                //    {
                //        NeedsClipBounds = true;
                //        arrangeSize.Height = unclippedDesiredSize.Height;
                //    }

                //}

                MinMax mm = new MinMax(this);
                if (useLayoutRounding && !FrameworkAppContextSwitches.DoNotApplyLayoutRoundingToMarginsAndBorderThickness)
                {
                    mm.maxHeight = UIElement.RoundLayoutValue(mm.maxHeight, dpi.DpiScaleY);
                    mm.maxWidth = UIElement.RoundLayoutValue(mm.maxWidth, dpi.DpiScaleX);
                    mm.minHeight = UIElement.RoundLayoutValue(mm.minHeight, dpi.DpiScaleY);
                    mm.minWidth = UIElement.RoundLayoutValue(mm.minWidth, dpi.DpiScaleX);
                }

                //we have to choose max between UnclippedDesiredSize and Max here, because
                //otherwise setting of max property could cause arrange at less then unclippedDS.
                //Clipping by Max is needed to limit stretch here
                double effectiveMaxWidth = Math.Max(unclippedDesiredSize.Width, mm.maxWidth);
                if (DoubleUtil.LessThan(effectiveMaxWidth, arrangeSize.Width))
                {
                    // NeedsClipBounds = true;
                    arrangeSize.Width = effectiveMaxWidth;
                }

                double effectiveMaxHeight = Math.Max(unclippedDesiredSize.Height, mm.maxHeight);
                if (DoubleUtil.LessThan(effectiveMaxHeight, arrangeSize.Height))
                {
                    // NeedsClipBounds = true;
                    arrangeSize.Height = effectiveMaxHeight;
                }

                // If using layout rounding, round size passed to children.
                if (useLayoutRounding)
                {
                    arrangeSize = UIElement.RoundLayoutSize(arrangeSize, dpi.DpiScaleX, dpi.DpiScaleY);
                }


                Size oldRenderSize = RenderSize;
                Size innerInkSize = ArrangeOverride(arrangeSize);

                //Here we use un-clipped InkSize because element does not know that it is
                //clipped by layout system and it shoudl have as much space to render as
                //it returned from its own ArrangeOverride
                RenderSize = innerInkSize;
                if (useLayoutRounding)
                {
                    RenderSize = UIElement.RoundLayoutSize(RenderSize, dpi.DpiScaleX, dpi.DpiScaleY);
                }

                //clippedInkSize differs from InkSize only what MaxWidth/Height explicitly clip the
                //otherwise good arrangement. For ex, DS<clientSize but DS>MaxWidth - in this
                //case we should initiate clip at MaxWidth and only show Top-Left portion
                //of the element limited by Max properties. It is Top-left because in case when we
                //are clipped by container we also degrade to Top-Left, so we are consistent.
                Size clippedInkSize = new Size(Math.Min(innerInkSize.Width, mm.maxWidth),
                                               Math.Min(innerInkSize.Height, mm.maxHeight));

                if (useLayoutRounding)
                {
                    clippedInkSize = UIElement.RoundLayoutSize(clippedInkSize, dpi.DpiScaleX, dpi.DpiScaleY);
                }

                //remember we have to clip if Max properties limit the inkSize
                //NeedsClipBounds |=
                //        DoubleUtil.LessThan(clippedInkSize.Width, innerInkSize.Width)
                //    || DoubleUtil.LessThan(clippedInkSize.Height, innerInkSize.Height);

                //if LayoutTransform is set, get the "outer bounds" - the alignments etc work on them
                //if (ltd != null)
                //{
                //    Rect inkRectTransformed = Rect.Transform(new Rect(0, 0, clippedInkSize.Width, clippedInkSize.Height), ltd.Transform.Value);
                //    clippedInkSize.Width = inkRectTransformed.Width;
                //    clippedInkSize.Height = inkRectTransformed.Height;

                //    if (useLayoutRounding)
                //    {
                //        clippedInkSize = UIElement.RoundLayoutSize(clippedInkSize, dpi.DpiScaleX, dpi.DpiScaleY);
                //    }
                //}

                //Note that inkSize now can be bigger then layoutSlotSize-margin (because of layout
                //squeeze by the parent or LayoutConstrained=true, which clips desired size in Measure).

                // The client size is the size of layout slot decreased by margins.
                // This is the "window" through which we see the content of the child.
                // Alignments position ink of the child in this "window".
                // Max with 0 is neccessary because layout slot may be smaller then unclipped desired size.
                Size clientSize = new Size(Math.Max(0, finalRect.Width - marginWidth),
                                        Math.Max(0, finalRect.Height - marginHeight));

                if (useLayoutRounding)
                {
                    clientSize = UIElement.RoundLayoutSize(clientSize, dpi.DpiScaleX, dpi.DpiScaleY);
                }

                ////remember we have to clip if clientSize limits the inkSize
                //NeedsClipBounds |=
                //        DoubleUtil.LessThan(clientSize.Width, clippedInkSize.Width)
                //    || DoubleUtil.LessThan(clientSize.Height, clippedInkSize.Height);

                Vector offset = ComputeAlignmentOffset(clientSize, clippedInkSize);

                offset.X += finalRect.X + margin.Left;
                offset.Y += finalRect.Y + margin.Top;

                // If using layout rounding, round offset.
                if (useLayoutRounding)
                {
                    offset.X = UIElement.RoundLayoutValue(offset.X, dpi.DpiScaleX);
                    offset.Y = UIElement.RoundLayoutValue(offset.Y, dpi.DpiScaleY);
                }

                SetLayoutOffset(offset, oldRenderSize);
            }
        }

        private Vector ComputeAlignmentOffset(Size clientSize, Size inkSize)
        {
            Vector offset = new Vector();

            HorizontalAlignment ha = HorizontalAlignment;
            VerticalAlignment va = VerticalAlignment;

            //this is to degenerate Stretch to Top-Left in case when clipping is about to occur
            //if we need it to be Center instead, simply remove these 2 ifs
            if (ha == HorizontalAlignment.Stretch
                && inkSize.Width > clientSize.Width)
            {
                ha = HorizontalAlignment.Left;
            }

            if (va == VerticalAlignment.Stretch
                && inkSize.Height > clientSize.Height)
            {
                va = VerticalAlignment.Top;
            }
            //end of degeneration of Stretch to Top-Left

            if (ha == HorizontalAlignment.Center
                || ha == HorizontalAlignment.Stretch)
            {
                offset.X = (clientSize.Width - inkSize.Width) * 0.5;
            }
            else if (ha == HorizontalAlignment.Right)
            {
                offset.X = clientSize.Width - inkSize.Width;
            }
            else
            {
                offset.X = 0;
            }

            if (va == VerticalAlignment.Center
                || va == VerticalAlignment.Stretch)
            {
                offset.Y = (clientSize.Height - inkSize.Height) * 0.5;
            }
            else if (va == VerticalAlignment.Bottom)
            {
                offset.Y = clientSize.Height - inkSize.Height;
            }
            else
            {
                offset.Y = 0;
            }

            return offset;
        }

        /// <summary>
        /// Gets or sets the identifying name of the control.
        /// The name provides a reference so that code-behind, such as event handler code,
        /// can refer to a markup control after it is constructed during processing by a UIXML processor.
        /// </summary>
        /// <value>The name of the control. The default is <c>null</c>.</value>
        public string? Name { get; set; } // todo: maybe use Site.Name?
        
        /// <summary>
        ///     BindingGroup DependencyProperty
        /// </summary>
        public static readonly DependencyProperty BindingGroupProperty =
                    DependencyProperty.Register(
                                "BindingGroup",
                                typeof(BindingGroup),
                                typeof(FrameworkElement),
                                new FrameworkPropertyMetadata(null,
                                        FrameworkPropertyMetadataOptions.Inherits));

        public bool IsInitialized { get; internal set; }
        public FrameworkElement Parent { get; internal set; }
        public bool IsParentAnFE { get; internal set; }

        internal override DependencyObject GetUIParentCore()
        {
            return Parent;
        }

        internal bool IsLogicalChildrenIterationInProgress
        {
            get { return ReadInternalFlag(InternalFlags.IsLogicalChildrenIterationInProgress); }
            set { WriteInternalFlag(InternalFlags.IsLogicalChildrenIterationInProgress, value); }
        }


        /// <summary>
        /// Language can be specified in xaml at any point using the xml language attribute xml:lang.
        /// This will make the culture pertain to the scope of the element where it is applied.  The
        /// XmlLanguage names follow the RFC 3066 standard. For example, U.S. English is "en-US".
        /// </summary>
        static public readonly DependencyProperty LanguageProperty =
                    DependencyProperty.RegisterAttached(
                                "Language",
                                typeof(object),//typeof(XmlLanguage),
                                typeof(FrameworkElement),
                                new FrameworkPropertyMetadata(
                                        /*XmlLanguage.GetLanguage("en-US")*/"en-US",
                                        FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public event RoutedEventHandler LostFocus;

        private InternalFlags _flags = 0; // Stores Flags (see Flags enum)

        /// <summary>
        ///     Indicates the current mode of lookup for both inheritance and resources.
        /// </summary>
        /// <remarks>
        ///     Used in property inheritance and reverse
        ///     inheritance and resource lookup to stop at
        ///     logical tree boundaries
        ///
        ///     It is also used by designers such as Sparkle to
        ///     skip past the app resources directly to the theme.
        ///     They are expected to merge in the client's app
        ///     resources via the MergedDictionaries feature on
        ///     root element of the tree.
        ///
        ///     NOTE: Property can be set only when the
        ///     instance is not yet hooked to the tree. This
        ///     is to encourage setting it at construction time.
        ///     If we didn't restrict it to (roughly) construction
        ///     time, we would have to take the complexity of
        ///     firing property invalidations when the flag was
        ///     changed.
        /// </remarks>
        protected internal InheritanceBehavior InheritanceBehavior
        {
            get
            {
                Debug.Assert((uint)InternalFlags.InheritanceBehavior0 == 0x08);
                Debug.Assert((uint)InternalFlags.InheritanceBehavior1 == 0x10);
                Debug.Assert((uint)InternalFlags.InheritanceBehavior2 == 0x20);

                const uint inheritanceBehaviorMask =
                    (uint)InternalFlags.InheritanceBehavior0 +
                    (uint)InternalFlags.InheritanceBehavior1 +
                    (uint)InternalFlags.InheritanceBehavior2;

                uint inheritanceBehavior = ((uint)_flags & inheritanceBehaviorMask) >> 3;
                return (InheritanceBehavior)inheritanceBehavior;
            }

            set
            {
                Debug.Assert((uint)InternalFlags.InheritanceBehavior0 == 0x08);
                Debug.Assert((uint)InternalFlags.InheritanceBehavior1 == 0x10);
                Debug.Assert((uint)InternalFlags.InheritanceBehavior2 == 0x20);

                const uint inheritanceBehaviorMask =
                    (uint)InternalFlags.InheritanceBehavior0 +
                    (uint)InternalFlags.InheritanceBehavior1 +
                    (uint)InternalFlags.InheritanceBehavior2;

                if (!this.IsInitialized)
                {
                    if ((uint)value < 0 ||
                        (uint)value > (uint)InheritanceBehavior.SkipAllNext)
                    {
                        throw new InvalidEnumArgumentException("value", (int)value, typeof(InheritanceBehavior));
                    }

                    uint inheritanceBehavior = (uint)value << 3;

                    _flags = (InternalFlags)((inheritanceBehavior & inheritanceBehaviorMask) | (((uint)_flags) & ~inheritanceBehaviorMask));

                    if (Parent != null)
                    {
                        // This means that we are in the process of xaml parsing:
                        // an instance of FE has been created and added to a parent,
                        // but no children yet added to it (otherwise it would be initialized already
                        // and we would not be allowed to change InheritanceBehavior).
                        // So we need to re-calculate properties accounting for the new
                        // inheritance behavior.
                        // This must have no performance effect as the subtree of this
                        // element is empty (no children yet added).
                        TreeWalkHelper.InvalidateOnTreeChange(/*fe:*/this, /*fce:null,*/ Parent, true);
                    }
                }
                else
                {
                    throw new InvalidOperationException(SR.Get(SRID.Illegal_InheritanceBehaviorSettor));
                }
            }
        }

        /// <summary>
        ///     Invoked when logical parent is changed.  This just
        ///     sets the parent pointer.
        /// </summary>
        /// <remarks>
        ///     A parent change is considered catastrohpic and results in a large
        ///     amount of invalidations and tree traversals. <cref see="DependencyFastBuild"/>
        ///     is recommended to reduce the work necessary to build a tree
        /// </remarks>
        /// <param name="newParent">
        ///     New parent that was set
        /// </param>
        internal void ChangeLogicalParent(DependencyObject oldParent, DependencyObject newParent)
        {
            ///////////////////
            // OnNewParent:
            ///////////////////

            //
            // -- Approved By The Core Team --
            //
            // Do not allow foreign threads to change the tree.
            // (This is a noop if this object is not assigned to a Dispatcher.)
            //
            // We also need to ensure that the tree is homogenous with respect
            // to the dispatchers that the elements belong to.
            //
            this.VerifyAccess();
            if (newParent != null)
            {
                newParent.VerifyAccess();
            }

            //// Logical Parent must first be dropped before you are attached to a newParent
            //// This mitigates illegal tree state caused by logical child stealing as illustrated in bug 970706
            //if (_parent != null && newParent != null && _parent != newParent)
            //{
            //    throw new System.InvalidOperationException(SR.Get(SRID.HasLogicalParent));
            //}

            //// Trivial check to avoid loops
            //if (newParent == this)
            //{
            //    throw new System.InvalidOperationException(SR.Get(SRID.CannotBeSelfParent));
            //}

            //// invalid during a VisualTreeChanged event
            //VisualDiagnostics.VerifyVisualTreeChange(this);

            // Logical Parent implies no InheritanceContext
            if (newParent != null)
            {
                ClearInheritanceContext();
            }

            IsParentAnFE = newParent is FrameworkElement;

            OnNewParent(oldParent, newParent);

            // Update Has[Loaded/Unloaded]Handler Flags
            //BroadcastEventHelper.AddOrRemoveHasLoadedChangeHandlerFlag(this, oldParent, newParent);



            ///////////////////
            // OnParentChanged:
            ///////////////////

            // Invalidate relevant properties for this subtree
            DependencyObject parent = (newParent != null) ? newParent : oldParent;
            TreeWalkHelper.InvalidateOnTreeChange(/* fe = */ this, parent, (newParent != null));

            // If no one has called BeginInit then mark the element initialized and fire Initialized event
            // (non-parser programmatic tree building scenario)
            //TryFireInitialized();
        }

        private static readonly UncommonField<DependencyObject> InheritanceContextField = new UncommonField<DependencyObject>();

        // Clear the inheritance context (called when the element
        // gets a real parent
        private void ClearInheritanceContext()
        {
            if (InheritanceContext != null)
            {
                InheritanceContextField.ClearValue(this);
                OnInheritanceContextChanged(EventArgs.Empty);
            }
        }

        internal bool ReadInternalFlag(InternalFlags reqFlag)
        {
            return (_flags & reqFlag) != 0;
        }

        // Indicates if there are any implicit styles in the ancestry
        internal bool ShouldLookupImplicitStyles
        {
            get { return ReadInternalFlag(InternalFlags.ShouldLookupImplicitStyles); }
            set { WriteInternalFlag(InternalFlags.ShouldLookupImplicitStyles, value); }
        }

        /// <summary>
        ///     Allows FrameworkElement to augment the
        ///     <see cref="EventRoute"/>
        /// </summary>
        /// <remarks>
        ///     NOTE: If this instance does not have a
        ///     visualParent but has a model parent
        ///     then route is built through the model
        ///     parent
        /// </remarks>
        /// <param name="route">
        ///     The <see cref="EventRoute"/> to be
        ///     augmented
        /// </param>
        /// <param name="args">
        ///     <see cref="RoutedEventArgs"/> for the
        ///     RoutedEvent to be raised post building
        ///     the route
        /// </param>
        /// <returns>
        ///     Whether or not the route should continue past the visual tree.
        ///     If this is true, and there are no more visual parents, the route
        ///     building code will call the GetUIParentCore method to find the
        ///     next non-visual parent.
        /// </returns>
        internal override bool BuildRouteCore(EventRoute route, RoutedEventArgs args)
        {
            return BuildRouteCoreHelper(route, args, true);
        }

        // Allows adjustments to the branch source popped off the stack
        internal virtual void AdjustBranchSource(RoutedEventArgs args)
        {
        }

        internal static void AddIntermediateElementsToRoute(
            DependencyObject mergePoint,
            EventRoute route,
            RoutedEventArgs args,
            DependencyObject modelTreeNode)
        {
            while (modelTreeNode != null && modelTreeNode != mergePoint)
            {
                UIElement uiElement = modelTreeNode as UIElement;

                if (uiElement != null)
                {
                    uiElement.AddToEventRoute(route, args);

                    //FrameworkElement fe = uiElement as FrameworkElement;
                    //if (fe != null)
                    //{
                    //    AddStyleHandlersToEventRoute(fe, null, route, args);
                    //}
                }

                // Get model parent
                modelTreeNode = LogicalTreeHelper.GetParent(modelTreeNode);
            }
        }

        internal bool BuildRouteCoreHelper(EventRoute route, RoutedEventArgs args, bool shouldAddIntermediateElementsToRoute)
        {
            bool continuePastCoreTree = false;


            //DependencyObject visualParent = VisualTreeHelper.GetParent(this);
            DependencyObject modelParent = GetUIParentCore();

            // FrameworkElement extends the basic event routing strategy by
            // introducing the concept of a logical tree.  When an event
            // passes through an element in a logical tree, the source of
            // the event needs to change to the leaf-most node in the same
            // logical tree that is in the route.

            // Check the route to see if we are returning into a logical tree
            // that we left before.  If so, restore the source of the event to
            // be the source that it was when we left the logical tree.
            DependencyObject branchNode = route.PeekBranchNode() as DependencyObject;
            if (branchNode != null && IsLogicalDescendent(branchNode))
            {
                // We keep the most recent source in the event args.  Note that
                // this is only for our consumption.  Once the event is raised,
                // it will use the source information in the route.
                args.Source = route.PeekBranchSource();

                AdjustBranchSource(args);

                route.AddSource(args.Source);

                // By popping the branch node we will also be setting the source
                // in the event route.
                route.PopBranchNode();

                // Add intermediate ContentElements to the route
                if (shouldAddIntermediateElementsToRoute)
                {
                    FrameworkElement.AddIntermediateElementsToRoute(this, route, args, LogicalTreeHelper.GetParent(branchNode));
                }
            }


            // Check if the next element in the route is in a different
            // logical tree.

            if (!IgnoreModelParentBuildRoute(args))
            {
                // If there is no visual parent, route via the model tree.
                /* yezo
                if (visualParent == null)
                {
                    continuePastCoreTree = modelParent != null;
                }
                else */if (modelParent != null)
                {
                    //Visual visualParentAsVisual = visualParent as Visual;
                    //if (visualParentAsVisual != null)
                    //{
                    //    if (visualParentAsVisual.CheckFlagsAnd(VisualFlags.IsLayoutIslandRoot))
                    //    {
                    //        continuePastCoreTree = true;
                    //    }
                    //}
                    //else
                    //{
                    //    if (((Visual3D)visualParent).CheckFlagsAnd(VisualFlags.IsLayoutIslandRoot))
                    //    {
                    //        continuePastCoreTree = true;
                    //    }
                    //}

                    // If there is a model parent, record the branch node.
                    route.PushBranchNode(this, args.Source);

                    // The source is going to be the visual parent, which
                    // could live in a different logical tree.
                    //args.Source = visualParent;
                    args.Source = Parent;
                }
            }

            return continuePastCoreTree;
        }

        internal virtual bool IgnoreModelParentBuildRoute(RoutedEventArgs args)
        {
            return false;
        }

        /// <summary>
        ///     DataContextChanged event
        /// </summary>
        /// <remarks>
        ///     When an element's DataContext changes, all data-bound properties
        ///     (on this element or any other element) whose Bindings use this
        ///     DataContext will change to reflect the new value.  There is no
        ///     guarantee made about the order of these changes relative to the
        ///     raising of the DataContextChanged event.  The changes can happen
        ///     before the event, after the event, or in any mixture.
        /// </remarks>
        public event DependencyPropertyChangedEventHandler DataContextChanged
        {
            add { EventHandlersStoreAdd(DataContextChangedKey, value); }
            remove { EventHandlersStoreRemove(DataContextChangedKey, value); }
        }

        /// <summary>
        ///     Returns enumerator to logical children
        /// </summary>
        protected internal virtual IEnumerator LogicalChildren
        {
            get { return null; }
        }

        /// <summary>
        ///     Allows adjustment to the event source
        /// </summary>
        /// <remarks>
        ///     Subclasses must override this method
        ///     to be able to adjust the source during
        ///     route invocation <para/>
        ///
        ///     NOTE: Expected to return null when no
        ///     change is made to source
        /// </remarks>
        /// <param name="args">
        ///     Routed Event Args
        /// </param>
        /// <returns>
        ///     Returns new source
        /// </returns>
        internal override object AdjustEventSource(RoutedEventArgs args)
        {
            object source = null;

            // As part of routing events through logical trees, we have
            // to be careful about events that come to us from "foreign"
            // trees.  For example, the event could come from an element
            // in our "implementation" visual tree, or from an element
            // in a different logical tree all together.
            //
            // Note that we consider ourselves to be part of a logical tree
            // if we have either a logical parent, or any logical children.
            //
            // BUGBUG: this misses "trees" that have only one logical node.  No parents, no children.

            if (Parent != null || HasLogicalChildren)
            {
                DependencyObject logicalSource = args.Source as DependencyObject;
                if (logicalSource == null || !IsLogicalDescendent(logicalSource))
                {
                    args.Source = this;
                    source = this;
                }
            }

            return source;
        }

        // Returns if the given child instance is a logical descendent
        private bool IsLogicalDescendent(DependencyObject child)
        {
            while (child != null)
            {
                if (child == this)
                {
                    return true;
                }

                child = LogicalTreeHelper.GetParent(child);
            }

            return false;
        }

        internal void EventHandlersStoreAdd(EventPrivateKey key, Delegate handler)
        {
            EnsureEventHandlersStore();
            EventHandlersStore.Add(key, handler);
        }

        internal void EventHandlersStoreRemove(EventPrivateKey key, Delegate handler)
        {
            EventHandlersStore store = EventHandlersStore;
            if (store != null)
            {
                store.Remove(key, handler);
            }
        }

        /// <summary>
        ///     Notification that a specified property has been changed
        /// </summary>
        /// <param name="e">EventArgs that contains the property, metadata, old value, and new value for this change</param>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            DependencyProperty dp = e.Property;

            // invalid during a VisualTreeChanged event
            //VisualDiagnostics.VerifyVisualTreeChange(this);

            base.OnPropertyChanged(e);

            if (e.IsAValueChange || e.IsASubPropertyChange)
            {
                //
                // Try to fire the Loaded event on the root of the tree
                // because for this case the OnParentChanged will not
                // have a chance to fire the Loaded event.
                //
                //if (dp != null && dp.OwnerType == typeof(PresentationSource) && dp.Name == "RootSource")
                //{
                //    TryFireInitialized();
                //}

                //if (dp == FrameworkElement.NameProperty &&
                //    EventTrace.IsEnabled(EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Verbose))
                //{
                //    EventTrace.EventProvider.TraceEvent(EventTrace.Event.PerfElementIDName, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Verbose,
                //            PerfService.GetPerfElementID(this), GetType().Name, GetValue(dp));
                //}

                //
                // Invalidation propagation for Styles
                //

                // Regardless of metadata, the Style/Template/DefaultStyleKey properties are never a trigger drivers
                //if (dp != StyleProperty && dp != Control.TemplateProperty && dp != DefaultStyleKeyProperty)
                {
                    // Note even properties on non-container nodes within a template could be driving a trigger
                    //if (TemplatedParent != null)
                    //{
                    //    FrameworkElement feTemplatedParent = TemplatedParent as FrameworkElement;

                    //    FrameworkTemplate frameworkTemplate = feTemplatedParent.TemplateInternal;
                    //    if (frameworkTemplate != null)
                    //    {
                    //        StyleHelper.OnTriggerSourcePropertyInvalidated(null, frameworkTemplate, TemplatedParent, dp, e, false /*invalidateOnlyContainer*/,
                    //            ref frameworkTemplate.TriggerSourceRecordFromChildIndex, ref frameworkTemplate.PropertyTriggersWithActions, TemplateChildIndex /*sourceChildIndex*/);
                    //    }
                    //}

                    // Do not validate Style during an invalidation if the Style was
                    // never used before (dependents do not need invalidation)
                    //if (Style != null)
                    //{
                    //    StyleHelper.OnTriggerSourcePropertyInvalidated(Style, null, this, dp, e, true /*invalidateOnlyContainer*/,
                    //        ref Style.TriggerSourceRecordFromChildIndex, ref Style.PropertyTriggersWithActions, 0 /*sourceChildIndex*/); // Style can only have triggers that are driven by properties on the container
                    //}

                    // Do not validate Template during an invalidation if the Template was
                    // never used before (dependents do not need invalidation)
                    //if (TemplateInternal != null)
                    //{
                    //    StyleHelper.OnTriggerSourcePropertyInvalidated(null, TemplateInternal, this, dp, e, !HasTemplateGeneratedSubTree /*invalidateOnlyContainer*/,
                    //        ref TemplateInternal.TriggerSourceRecordFromChildIndex, ref TemplateInternal.PropertyTriggersWithActions, 0 /*sourceChildIndex*/); // These are driven by the container
                    //}

                    // There may be container dependents in the ThemeStyle. Invalidate them.
                    //if (ThemeStyle != null && Style != ThemeStyle)
                    //{
                    //    StyleHelper.OnTriggerSourcePropertyInvalidated(ThemeStyle, null, this, dp, e, true /*invalidateOnlyContainer*/,
                    //        ref ThemeStyle.TriggerSourceRecordFromChildIndex, ref ThemeStyle.PropertyTriggersWithActions, 0 /*sourceChildIndex*/); // ThemeStyle can only have triggers that are driven by properties on the container
                    //}
                }
            }

            FrameworkPropertyMetadata fmetadata = e.Metadata as FrameworkPropertyMetadata;

            //
            // Invalidation propagation for Groups and Inheritance
            //

            // Metadata must exist specifically stating propagate invalidation
            // due to group or inheritance
            if (fmetadata != null)
            {
                //
                // Inheritance
                //

                if (fmetadata.Inherits)
                {
                    // Invalidate Inheritable descendents only if instance is not a TreeSeparator
                    // or fmetadata.OverridesInheritanceBehavior is set to override separated tree behavior
                    if ((InheritanceBehavior == InheritanceBehavior.Default || fmetadata.OverridesInheritanceBehavior) &&
                        (!DependencyObject.IsTreeWalkOperation(e.OperationType) || PotentiallyHasMentees))
                    {
                        EffectiveValueEntry newEntry = e.NewEntry;
                        EffectiveValueEntry oldEntry = e.OldEntry;
                        if (oldEntry.BaseValueSourceInternal > newEntry.BaseValueSourceInternal)
                        {
                            // valuesource == Inherited && value == UnsetValue indicates that we are clearing the inherited value
                            newEntry = new EffectiveValueEntry(dp, BaseValueSourceInternal.Inherited);
                        }
                        else
                        {
                            newEntry = newEntry.GetFlattenedEntry(RequestFlags.FullyResolved);
                            newEntry.BaseValueSourceInternal = BaseValueSourceInternal.Inherited;
                        }

                        if (oldEntry.BaseValueSourceInternal != BaseValueSourceInternal.Default || oldEntry.HasModifiers)
                        {
                            oldEntry = oldEntry.GetFlattenedEntry(RequestFlags.FullyResolved);
                            oldEntry.BaseValueSourceInternal = BaseValueSourceInternal.Inherited;
                        }
                        else
                        {
                            // we use an empty EffectiveValueEntry as a signal that the old entry was the default value
                            oldEntry = new EffectiveValueEntry();
                        }

                        InheritablePropertyChangeInfo info =
                                new InheritablePropertyChangeInfo(
                                        this,
                                        dp,
                                        oldEntry,
                                        newEntry);

                        // Don't InvalidateTree if we're in the middle of doing it.
                        if (!DependencyObject.IsTreeWalkOperation(e.OperationType))
                        {
                            TreeWalkHelper.InvalidateOnInheritablePropertyChange(this, info, true);
                        }

                        // Notify mentees if they exist
                        if (PotentiallyHasMentees)
                        {
                            TreeWalkHelper.OnInheritedPropertyChanged(this, ref info, InheritanceBehavior);
                        }
                    }
                }

                if (e.IsAValueChange || e.IsASubPropertyChange)
                {
                    /*
                    //
                    // Layout invalidation
                    //

                    // Skip if we're traversing an Visibility=Collapsed subtree while
                    //  in the middle of an invalidation storm due to ancestor change
                    if (!(AncestorChangeInProgress && InVisibilityCollapsedTree))
                    {
                        UIElement layoutParent = null;

                        bool affectsParentMeasure = fmetadata.AffectsParentMeasure;
                        bool affectsParentArrange = fmetadata.AffectsParentArrange;
                        bool affectsMeasure = fmetadata.AffectsMeasure;
                        bool affectsArrange = fmetadata.AffectsArrange;
                        if (affectsMeasure || affectsArrange || affectsParentArrange || affectsParentMeasure)
                        {
                            // Locate nearest Layout parent
                            for (Visual v = VisualTreeHelper.GetParent(this) as Visual;
                                 v != null;
                                 v = VisualTreeHelper.GetParent(v) as Visual)
                            {
                                layoutParent = v as UIElement;
                                if (layoutParent != null)
                                {
                                    //let incrementally-updating FrameworkElements to mark the vicinity of the affected child
                                    //to perform partial update.
                                    if (FrameworkElement.DType.IsInstanceOfType(layoutParent))
                                        ((FrameworkElement)layoutParent).ParentLayoutInvalidated(this);

                                    if (affectsParentMeasure)
                                    {
                                        layoutParent.InvalidateMeasure();
                                    }

                                    if (affectsParentArrange)
                                    {
                                        layoutParent.InvalidateArrange();
                                    }

                                    break;
                                }
                            }
                        }

                        if (fmetadata.AffectsMeasure)
                        {
                            // Need to complete workaround ...
                            // this is a test to see if we understand the source of the duplicate renders -- WM_SIZE
                            // is handled by Window by setting Width & Height, even though the HwndSource will also
                            // handle WM_SIZE and perform a relayout
                            if (!BypassLayoutPolicies || !((dp == WidthProperty) || (dp == HeightProperty)))
                            {
                                InvalidateMeasure();
                            }
                        }

                        if (fmetadata.AffectsArrange)
                        {
                            InvalidateArrange();
                        }

                        if (fmetadata.AffectsRender &&
                            (e.IsAValueChange || !fmetadata.SubPropertiesDoNotAffectRender))
                        {
                            InvalidateVisual();
                        }
                    }*/
                }
            }
        }

        /// <summary>
        ///     DataContext DependencyProperty
        /// </summary>
        public static readonly DependencyProperty DataContextProperty =
                    DependencyProperty.Register(
                                "DataContext",
                                typeof(object),
                                typeof(FrameworkElement),
                                new FrameworkPropertyMetadata(null,
                                        FrameworkPropertyMetadataOptions.Inherits,
                                        new PropertyChangedCallback(OnDataContextChanged)));

        /// <summary>
        ///     DataContext Property
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Localizability(LocalizationCategory.NeverLocalize)]
        public object DataContext
        {
            get { return GetValue(DataContextProperty); }
            set { SetValue(DataContextProperty, value); }
        }

        internal virtual bool HasLogicalChildren => false;

        // Helper method to retrieve and fire Clr Event handlers for DependencyPropertyChanged event
        private void RaiseDependencyPropertyChanged(EventPrivateKey key, DependencyPropertyChangedEventArgs args)
        {
            EventHandlersStore store = EventHandlersStore;
            if (store != null)
            {
                Delegate handler = store.Get(key);
                if (handler != null)
                {
                    ((DependencyPropertyChangedEventHandler)handler)(this, args);
                }
            }
        }

        /// <summary>
        ///     DataContextChanged private key
        /// </summary>
        internal static readonly EventPrivateKey DataContextChangedKey = new EventPrivateKey();

        private static void OnDataContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == BindingExpressionBase.DisconnectedItem)
                return;

            ((FrameworkElement)d).RaiseDependencyPropertyChanged(DataContextChangedKey, e);
        }

        internal static void AddHandler(DependencyObject d, RoutedEvent routedEvent, Delegate handler)
        {
            throw new NotImplementedException();
        }

        internal static void RemoveHandler(DependencyObject d, RoutedEvent routedEvent, Delegate handler)
        {
            throw new NotImplementedException();
        }

        // Sets or Unsets the required flag based on
        // the bool argument
        internal void WriteInternalFlag(InternalFlags reqFlag, bool set)
        {
            if (set)
            {
                _flags |= reqFlag;
            }
            else
            {
                _flags &= (~reqFlag);
            }
        }

        internal static bool GetFrameworkParent(FrameworkElement current, out FrameworkElement feParent)
        {
            FrameworkObject fo = new FrameworkObject(current);

            fo = fo.FrameworkParent;

            feParent = fo.FE;

            return fo.IsValid;
        }

        internal static DependencyObject GetFrameworkParent(object current)
        {
            FrameworkObject fo = new FrameworkObject(current as DependencyObject);

            fo = fo.FrameworkParent;

            return fo.DO;
        }

        // Indicates that an ancestor change tree walk is progressing
        // through the given node
        internal bool AncestorChangeInProgress
        {
            get { return ReadInternalFlag(InternalFlags.AncestorChangeInProgress); }
            set { WriteInternalFlag(InternalFlags.AncestorChangeInProgress, value); }
        }

        internal bool InVisibilityCollapsedTree
        {
            get { return ReadInternalFlag(InternalFlags.InVisibilityCollapsedTree); }
            set { WriteInternalFlag(InternalFlags.InVisibilityCollapsedTree, value); }
        }

        /// <summary>
        ///     Called before the parent is chanded to the new value.
        /// </summary>
        internal virtual void OnNewParent(DependencyObject oldParent, DependencyObject newParent)
        {
            //
            // This API is only here for compatability with the old
            // behavior.  Note that FrameworkElement does not have
            // this virtual, so why do we need it here?
            //

            // Synchronize ForceInherit properties
            //if (_parent != null && _parent is ContentElement)
            //{
            //    UIElement.SynchronizeForceInheritProperties(this, null, null, _parent);
            //}
            //else if (oldParent is ContentElement)
            //{
            //    UIElement.SynchronizeForceInheritProperties(this, null, null, oldParent);
            //}


            // Synchronize ReverseInheritProperty Flags
            //
            // NOTE: do this AFTER synchronizing force-inherited flags, since
            // they often effect focusability and such.
            //this.SynchronizeReverseInheritPropertyFlags(oldParent, false);
        }

        // OnAncestorChangedInternal variant when we know what type (FE/FCE) the
        //  tree node is.
        internal void OnAncestorChangedInternal(TreeChangeInfo parentTreeState)
        {
            // Cache the IsSelfInheritanceParent flag
            bool wasSelfInheritanceParent = IsSelfInheritanceParent;

            if (parentTreeState.Root != this)
            {
                // Clear the HasStyleChanged flag
                //HasStyleChanged = false;
                //HasStyleInvalidated = false;
                //HasTemplateChanged = false;
            }

            // If this is a tree add operation update the ShouldLookupImplicitStyles
            // flag with respect to your parent.
            if (parentTreeState.IsAddOperation)
            {
                FrameworkObject fo =
                    new FrameworkObject(this);

                fo.SetShouldLookupImplicitStyles();
            }

            // Invalidate ResourceReference properties
            //if (HasResourceReference)
            //{
            //    // This operation may cause a style change and hence should be done before the call to
            //    // InvalidateTreeDependents as it relies on the HasStyleChanged flag
            //    TreeWalkHelper.OnResourcesChanged(this, ResourcesChangeInfo.TreeChangeInfo, false);
            //}

            // If parent is a FrameworkElement
            // This is also an operation that could change the style
            FrugalObjectList<DependencyProperty> currentInheritableProperties =
            InvalidateTreeDependentProperties(parentTreeState, IsSelfInheritanceParent, wasSelfInheritanceParent);

            // we have inherited properties that changes as a result of the above;
            // invalidation; push that list of inherited properties on the stack
            // for the children to use
            parentTreeState.InheritablePropertiesStack.Push(currentInheritableProperties);



            // Call OnAncestorChanged
            OnAncestorChanged();

            // Notify mentees if they exist
            if (PotentiallyHasMentees)
            {
                // Raise the ResourcesChanged Event so that ResourceReferenceExpressions
                // on non-[FE/FCE] listening for this can then update their values
                RaiseClrEvent(FrameworkElement.ResourcesChangedKey, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     ResourcesChanged private key
        /// </summary>
        internal static readonly EventPrivateKey ResourcesChangedKey = new EventPrivateKey();

        // Helper method to retrieve and fire Clr Event handlers
        internal void RaiseClrEvent(EventPrivateKey key, EventArgs args)
        {
            EventHandlersStore store = EventHandlersStore;
            if (store != null)
            {
                Delegate handler = store.Get(key);
                if (handler != null)
                {
                    ((EventHandler)handler)(this, args);
                }
            }
        }

        // Indicates if the current element has or had mentees at some point.
        internal bool PotentiallyHasMentees
        {
            get { return ReadInternalFlag(InternalFlags.PotentiallyHasMentees); }
            set
            {
                Debug.Assert(value == true,
                    "This flag is set to true when a mentee attaches a listeners to either the " +
                    "InheritedPropertyChanged event or the ResourcesChanged event. It never goes " +
                    "back to being false because this would involve counting the remaining listeners " +
                    "for either of the aforementioned events. This seems like an overkill for the perf " +
                    "optimization we are trying to achieve here.");

                WriteInternalFlag(InternalFlags.PotentiallyHasMentees, value);
            }
        }

        /// <summary>
        ///     Invoked when ancestor is changed.  This is invoked after
        ///     the ancestor has changed, and the purpose is to allow elements to
        ///     perform actions based on the changed ancestor.
        /// </summary>
        internal virtual void OnAncestorChanged()
        {
        }

        /// <summary>
        ///     InheritedPropertyChanged private key
        /// </summary>
        internal static readonly EventPrivateKey InheritedPropertyChangedKey = new EventPrivateKey();

        // Helper method to retrieve and fire the InheritedPropertyChanged event
        internal void RaiseInheritedPropertyChangedEvent(ref InheritablePropertyChangeInfo info)
        {
            EventHandlersStore store = EventHandlersStore;
            if (store != null)
            {
                Delegate handler = store.Get(FrameworkElement.InheritedPropertyChangedKey);
                if (handler != null)
                {
                    InheritedPropertyChangedEventArgs args = new InheritedPropertyChangedEventArgs(ref info);
                    ((InheritedPropertyChangedEventHandler)handler)(this, args);
                }
            }
        }

        // Invalidate all the properties that may have changed as a result of
        //  changing this element's parent in the logical (and sometimes visual tree.)
        internal FrugalObjectList<DependencyProperty> InvalidateTreeDependentProperties(TreeChangeInfo parentTreeState, bool isSelfInheritanceParent, bool wasSelfInheritanceParent)
        {
            AncestorChangeInProgress = true;

            InVisibilityCollapsedTree = false;  // False == we don't know whether we're in a visibility collapsed tree.

            if (parentTreeState.TopmostCollapsedParentNode == null)
            {
                //// There is no ancestor node with Visibility=Collapsed.
                ////  See if "fe" is the root of a collapsed subtree.
                //if (Visibility == Visibility.Collapsed)
                //{
                //    // This is indeed the root of a collapsed subtree.
                //    //  remember this information as we proceed on the tree walk.
                //    parentTreeState.TopmostCollapsedParentNode = this;

                //    // Yes, this FE node is in a visibility collapsed subtree.
                //    InVisibilityCollapsedTree = true;
                //}
            }
            else
            {
                // There is an ancestor node somewhere above us with
                //  Visibility=Collapsed.  We're in a visibility collapsed subtree.
                InVisibilityCollapsedTree = true;
            }


            try
            {
                // Style property is a special case of a non-inherited property that needs
                // invalidation for parent changes. Invalidate StyleProperty if it hasn't been
                // locally set because local value takes precedence over implicit references
                //if (IsInitialized && !HasLocalStyle && (this != parentTreeState.Root))
                //{
                //    UpdateStyleProperty();
                //}

                //Style selfStyle = null;
                //Style selfThemeStyle = null;
                //DependencyObject templatedParent = null;

                //int childIndex = -1;
                //ChildRecord childRecord = new ChildRecord();
                //bool isChildRecordValid = false;

                //selfStyle = Style;
                //selfThemeStyle = ThemeStyle;
                //templatedParent = TemplatedParent;
                //childIndex = TemplateChildIndex;

                // StyleProperty could have changed during invalidation of ResourceReferenceExpressions if it
                // were locally set or during the invalidation of unresolved implicitly referenced style
                //bool hasStyleChanged = HasStyleChanged;

                // Fetch selfStyle, hasStyleChanged and childIndex for the current node
                //FrameworkElement.GetTemplatedParentChildRecord(templatedParent, childIndex, out childRecord, out isChildRecordValid);

                FrameworkElement parentFE;
                //FrameworkContentElement parentFCE;
                bool hasParent = FrameworkElement.GetFrameworkParent(this, out parentFE/*, out parentFCE*/);

                DependencyObject parent = null;
                InheritanceBehavior parentInheritanceBehavior = InheritanceBehavior.Default;
                if (hasParent)
                {
                    if (parentFE != null)
                    {
                        parent = parentFE;
                        parentInheritanceBehavior = parentFE.InheritanceBehavior;
                    }
                    //else
                    //{
                    //    parent = parentFCE;
                    //    parentInheritanceBehavior = parentFCE.InheritanceBehavior;
                    //}
                }

                if (!TreeWalkHelper.SkipNext(InheritanceBehavior) && !TreeWalkHelper.SkipNow(parentInheritanceBehavior))
                {
                    // Synchronize InheritanceParent
                    this.SynchronizeInheritanceParent(parent);
                }
                else if (!IsSelfInheritanceParent)
                {
                    // Set IsSelfInheritanceParet on the root node at a tree boundary
                    // so that all inheritable properties are cached on it.
                    SetIsSelfInheritanceParent();
                }

                // Loop through all cached inheritable properties for the parent to see if they should be invalidated.
                return TreeWalkHelper.InvalidateTreeDependentProperties(parentTreeState, /* fe = */ this, /* fce = *//* null*//*, selfStyle, selfThemeStyle,
                    ref childRecord, isChildRecordValid, hasStyleChanged,*/ isSelfInheritanceParent, wasSelfInheritanceParent);
            }
            finally
            {
                AncestorChangeInProgress = false;
                InVisibilityCollapsedTree = false;  // 'false' just means 'we don't know' - see comment at definition of the flag.
            }
        }
    }

    [Flags]
    internal enum InternalFlags2 : uint
    {
        // RESERVED: Bits 0-15  (0x0000FFFF): TemplateChildIndex
        R0 = 0x00000001,
        R1 = 0x00000002,
        R2 = 0x00000004,
        R3 = 0x00000008,
        R4 = 0x00000010,
        R5 = 0x00000020,
        R6 = 0x00000040,
        R7 = 0x00000080,
        R8 = 0x00000100,
        R9 = 0x00000200,
        RA = 0x00000400,
        RB = 0x00000800,
        RC = 0x00001000,
        RD = 0x00002000,
        RE = 0x00004000,
        RF = 0x00008000,

        // free bit                 = 0x00010000,
        // free bit                 = 0x00020000,
        // free bit                 = 0x00040000,
        // free bit                 = 0x00080000,

        TreeHasLoadedChangeHandler = 0x00100000,
        IsLoadedCache = 0x00200000,
        IsStyleSetFromGenerator = 0x00400000,
        IsParentAnFE = 0x00800000,
        IsTemplatedParentAnFE = 0x01000000,
        HasStyleChanged = 0x02000000,
        HasTemplateChanged = 0x04000000,
        HasStyleInvalidated = 0x08000000,
        IsRequestingExpression = 0x10000000,
        HasMultipleInheritanceContexts = 0x20000000,

        // free bit                 = 0x40000000,
        BypassLayoutPolicies = 0x80000000,

        // Default is so that the default value of TemplateChildIndex
        // (which is stored in the low 16 bits) can be 0xFFFF (interpreted to be -1).
        Default = 0x0000FFFF,

    }
}