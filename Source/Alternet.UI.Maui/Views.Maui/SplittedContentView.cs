using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Alternet.Maui
{
    /// <summary>
    /// Represents a view that is split into two panes with a movable splitter between them.
    /// </summary>
    public partial class SplittedContentView : Grid
    {
        /// <summary>
        /// Gets or sets the default thickness of the splitter.
        /// </summary>
        public static double DefaultSplitterThickness = 6;

        /// <summary>
        /// Gets or sets the default color of the splitter.
        /// </summary>
        public static Color DefaultSplitterColor = Colors.LightGray;

        readonly ContentView firstPane;
        readonly ContentView secondPane;
        readonly BoxView splitter;

        double initialSize;
        private double fixedPaneSize = 200;
        private double splitterThickness = DefaultSplitterThickness;
        private bool isVertical = true;
        private bool isFixedVisible = true;
        private bool isFirstPaneFixed = true;
        private double minFixedPaneSize = 50;

        /// <summary>
        /// Initializes a new instance of the <see cref="SplittedContentView"/> class.
        /// </summary>
        public SplittedContentView()
        {
            splitter = CreateSplitter();
            firstPane = CreateFirstPane();
            secondPane = CreateSecondPane();
            splitter.Color = DefaultSplitterColor;

            var pan = new PanGestureRecognizer();
            pan.PanUpdated += OnPanUpdated;
            splitter.GestureRecognizers.Add(pan);

            RebuildLayout();
        }

        /// <summary>
        /// Creates the splitter control that separates the two panes.
        /// </summary>
        /// <returns> The splitter control. </returns>
        protected virtual BoxView CreateSplitter()
        {
            return new BoxView();
        }

        /// <summary>
        /// Creates the second pane of the control.
        /// This method can be overridden in derived classes to provide a custom implementation of the second pane.
        /// </summary>
        /// <returns> The second pane control. </returns>
        protected virtual ContentView CreateSecondPane()
        {
            return new ContentView();
        }

        /// <summary>
        /// Creates the first pane of the control.
        /// This method can be overridden in derived classes to provide a custom implementation of the first pane.
        /// </summary>
        /// <returns> The first pane control. </returns>
        protected virtual ContentView CreateFirstPane()
        {
            return new ContentView();
        }

        /// <summary>
        /// Gets the splitter control that separates the two panes.
        /// </summary>
        public BoxView Splitter => splitter;

        /// <summary>
        /// Gets the first pane of the control.
        /// </summary>
        public ContentView? FirstPane => firstPane;

        /// <summary>
        /// Gets the second pane of the control.
        /// </summary>
        public ContentView? SecondPane => secondPane;

        /// <summary>
        /// Gets or sets the content of the first pane.
        /// </summary>
        public View? FirstContent
        {
            get => firstPane.Content;
            set => firstPane.Content = value;
        }

        /// <summary>
        /// Gets or sets the content of the second pane.
        /// </summary>
        public View? SecondContent
        {
            get => secondPane.Content;
            set => secondPane.Content = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the fixed pane is visible.
        /// </summary>
        public virtual bool IsFixedPaneVisible
        {
            get => isFixedVisible;
            set
            {
                if (isFixedVisible == value)
                    return;
                isFixedVisible = value;
                RebuildLayout();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the splitter is vertical or horizontal.
        /// </summary>
        public virtual bool IsVertical
        {
            get => isVertical;
            set
            {
                if (isVertical == value)
                    return;
                isVertical = value;
                RebuildLayout();
            }
        }

        /// <summary>
        /// Gets or sets the thickness of the splitter.
        /// </summary>
        public virtual double SplitterThickness
        {
            get => splitterThickness;
            set
            {
                if (splitterThickness == value)
                    return;
                splitterThickness = value;

                if (IsFixedPaneVisible)
                {
                    if (IsVertical)
                        ColumnDefinitions[1].Width = value;
                    else
                        RowDefinitions[1].Height = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the first pane has a fixed size.
        /// </summary>
        public virtual bool IsFirstPaneFixed
        {
            get
            {
                return isFirstPaneFixed;
            }

            set
            {
                if (isFirstPaneFixed == value)
                    return;
                isFirstPaneFixed = value;
                RebuildLayout();
            }
        }

        /// <summary>
        /// Gets or sets the size of the side bar.
        /// Side bar is the pane of the control which has a fixed size.
        /// By default the first pane is the side bar, but you can change it by
        /// setting <see cref="IsFirstPaneFixed"/> property.
        /// </summary>
        public virtual double FixedPaneSize
        {
            get => fixedPaneSize;
            set
            {
                if (value < MinFixedPaneSize)
                    value = MinFixedPaneSize;
                if (fixedPaneSize == value)
                    return;
                fixedPaneSize = value;

                if (IsFixedPaneVisible)
                {
                    if (IsFirstPaneFixed)
                    {
                        if (IsVertical)
                            ColumnDefinitions[0].Width = value;
                        else
                            RowDefinitions[0].Height = value;
                    }
                    else
                    {
                        if (IsVertical)
                            ColumnDefinitions[2].Width = value;
                        else
                            RowDefinitions[2].Height = value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the minimum size of the fixed pane.
        /// </summary>
        public virtual double MinFixedPaneSize
        {
            get => minFixedPaneSize;
            set
            {
                if (minFixedPaneSize == value)
                    return;
                minFixedPaneSize = value;
                if (FixedPaneSize < minFixedPaneSize)
                    FixedPaneSize = minFixedPaneSize;
            }
        }

        /// <summary>
        /// Rebuilds the layout of the control based on the current orientation and sizes of the panes.
        /// </summary>
        protected virtual void RebuildLayout()
        {
            Children.Clear();
            RowDefinitions.Clear();
            ColumnDefinitions.Clear();

            if (IsVertical)
            {
                ColumnDefinition CreateFixedPaneColumn()
                {
                    return new ColumnDefinition(FixedPaneSize);
                }

                ColumnDefinition CreateFirstPaneColumn()
                {
                    if (IsFirstPaneFixed)
                        return CreateFixedPaneColumn();
                    else
                        return CreateFillPaneColumn();
                }

                ColumnDefinition CreateFillPaneColumn()
                {
                    return new ColumnDefinition(GridLength.Star);
                }

                ColumnDefinition CreateSecondPaneColumn()
                {
                    if (IsFirstPaneFixed)
                        return CreateFillPaneColumn();
                    else
                        return CreateFixedPaneColumn();
                }

                if (IsFixedPaneVisible)
                {
                    ColumnDefinitions.Add(CreateFirstPaneColumn());
                    ColumnDefinitions.Add(new ColumnDefinition(SplitterThickness));
                    SetColumn(firstPane as IView, 0);
                    SetColumn(splitter as IView, 1);
                    ColumnDefinitions.Add(CreateSecondPaneColumn());
                    SetColumn(secondPane as IView, 2);
                }
                else
                {
                    ColumnDefinitions.Add(CreateFillPaneColumn());

                    if (IsFirstPaneFixed)
                        SetColumn(secondPane as IView, 0);
                    else
                        SetColumn(firstPane as IView, 0);
                }
            }
            else
            {
                RowDefinition CreateFixedPaneRow()
                {
                    return new RowDefinition(FixedPaneSize);
                }

                RowDefinition CreateFillPaneRow()
                {
                    return new RowDefinition(GridLength.Star);
                }

                RowDefinition CreateFirstPaneRow()
                {
                    if (IsFirstPaneFixed)
                        return CreateFixedPaneRow();
                    else
                        return CreateFillPaneRow();
                }

                RowDefinition CreateSecondPaneRow()
                {
                    if(IsFirstPaneFixed)
                        return CreateFillPaneRow();
                    else
                        return CreateFixedPaneRow();
                }

                if (IsFixedPaneVisible)
                {
                    RowDefinitions.Add(CreateFirstPaneRow());
                    RowDefinitions.Add(new RowDefinition(SplitterThickness));
                    SetRow(firstPane as IView, 0);
                    SetRow(splitter as IView, 1);
                    RowDefinitions.Add(CreateSecondPaneRow());
                    SetRow(secondPane as IView, 2);
                }
                else
                {
                    RowDefinitions.Add(CreateFillPaneRow());

                    if (IsFirstPaneFixed)
                        SetRow(secondPane as IView, 0);
                    else
                        SetRow(firstPane as IView, 0);
                }
            }

            if (IsFixedPaneVisible)
            {
                Children.Add(firstPane);
                Children.Add(splitter);
                Children.Add(secondPane);
            }
            else
            {
                if (IsFirstPaneFixed)
                    Children.Add(secondPane);
                else
                    Children.Add(firstPane);
            }
        }

        /// <summary>
        /// Handles the PanUpdated event of the splitter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PanUpdatedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnPanUpdated(object? sender, PanUpdatedEventArgs e)
        {
            if (!IsFixedPaneVisible)
                return;

            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    initialSize = FixedPaneSize;
                    break;

                case GestureStatus.Running:
                    if (IsVertical)
                    {
                        double delta;
                        if(IsFirstPaneFixed)
                            delta = e.TotalX;
                        else
                            delta = -e.TotalX;
                        var width = Math.Max(MinFixedPaneSize, initialSize + delta);
                        FixedPaneSize = width;
                    }
                    else
                    {
                        double delta;
                        if (IsFirstPaneFixed)
                            delta = e.TotalY;
                        else
                            delta = -e.TotalY;
                        var height = Math.Max(MinFixedPaneSize, initialSize + delta);
                        FixedPaneSize = height;
                    }
                    break;
            }
        }
    }
}
