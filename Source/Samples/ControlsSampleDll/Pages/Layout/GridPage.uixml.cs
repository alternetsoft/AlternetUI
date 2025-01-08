﻿using System;
using System.Linq;
using Alternet.UI;
using Alternet.Drawing;
using System.Collections.Generic;

namespace ControlsSample
{
    internal partial class GridPage : Control
    {
        private readonly ControlSet controls;

        public GridPage()
        {
            InitializeComponent();

            growButton.Click += GrowButton_Click;
            shrinkButton.Click += ShrinkButton_Click;

            List<AbstractControl> gridControls = new();

            void CreateControls(int minRow, int maxRow, int minColumn, int maxColumn)
            {
                for (int column = minColumn; column <= maxColumn; column++)
                {
                    for (int row = minRow; row <= maxRow; row++)
                    {
                        Border border = new();
                        Label label = new($"({column},{row})")
                        {
                            Parent = border,
                        };
                        Grid.SetRowColumn(border, row, column);
                        gridControls.Add(border);
                    }
                }
            }

            CreateControls(0, 5, 3, 4);
            CreateControls(3, 5, 0, 4);

            controls = new(gridControls);
        }

        private void GrowButton_Click(object? sender, EventArgs e)
        {
            mainGrid.ColumnCount = 4;
            mainGrid.RowCount = 5;
            controls.Parent(mainGrid);
            PerformLayoutAndInvalidate();
            growButton.Enabled = false;
        }

        private void ShrinkButton_Click(object? sender, EventArgs e)
        {
        }

        private void BackgroundButton_Click(object? sender, EventArgs e)
        {
            if (mainGrid.Background == null)
                mainGrid.Background = Color.Olive.AsBrush;
            else
                mainGrid.Background = null;
            mainGrid.Invalidate();
        }
    }
}