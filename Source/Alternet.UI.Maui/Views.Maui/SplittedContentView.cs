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

        readonly ContentView firstPane = new();
        readonly ContentView secondPane = new();
        readonly Border splitter = new();

        double initialSize;
        private double firstPaneSize = 200;
        private double splitterThickness = DefaultSplitterThickness;
        private bool isVertical = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="SplittedContentView"/> class.
        /// </summary>
        public SplittedContentView()
        {
            splitter.Background = DefaultSplitterColor;

            var pan = new PanGestureRecognizer();
            pan.PanUpdated += OnPanUpdated;
            splitter.GestureRecognizers.Add(pan);

            RebuildLayout();
        }

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
            set => splitterThickness = value;
        }

        /// <summary>
        /// Gets or sets the size of the first pane.
        /// </summary>
        public virtual double FirstPaneSize
        {
            get => firstPaneSize;
            set
            {
                if (firstPaneSize == value)
                    return;
                firstPaneSize = value;
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
                ColumnDefinitions.Add(new ColumnDefinition(FirstPaneSize));
                ColumnDefinitions.Add(new ColumnDefinition(SplitterThickness));
                ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));

                SetColumn(firstPane as IView, 0);
                SetColumn(splitter as IView, 1);
                SetColumn(secondPane as IView, 2);
            }
            else
            {
                RowDefinitions.Add(new RowDefinition(FirstPaneSize));
                RowDefinitions.Add(new RowDefinition(SplitterThickness));
                RowDefinitions.Add(new RowDefinition(GridLength.Star));

                SetRow(firstPane as IView, 0);
                SetRow(splitter as IView, 1);
                SetRow(secondPane as IView, 2);
            }

            Children.Add(firstPane);
            Children.Add(splitter);
            Children.Add(secondPane);
        }

        /// <summary>
        /// Handles the PanUpdated event of the splitter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PanUpdatedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnPanUpdated(object? sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    initialSize = FirstPaneSize;
                    break;

                case GestureStatus.Running:
                    if (IsVertical)
                    {
                        var width = Math.Max(50, initialSize + e.TotalX);

                        FirstPaneSize = width;
                        ColumnDefinitions[0].Width = width;
                    }
                    else
                    {
                        var height = Math.Max(50, initialSize + e.TotalY);

                        FirstPaneSize = height;
                        RowDefinitions[0].Height = height;
                    }
                    break;
            }
        }
    }
}
