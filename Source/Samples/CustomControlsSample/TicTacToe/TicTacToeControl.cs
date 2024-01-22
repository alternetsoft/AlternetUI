﻿using System;
using System.Collections.Generic;
using Alternet.Drawing;
using Alternet.UI;
using static CustomControlsSample.TicTacToeGame;

namespace CustomControlsSample
{
    internal partial class TicTacToeControl : Control
    {
        private readonly List<TicTacToeCell> cells = [];

        private static Grid grid = new()
        {
            Margin = 5,
            Padding = 5,
        };

        public TicTacToeControl()
        {
            grid.Parent = this;
            CreateCells();
            InitializeGame();
            SuggestedSize = new SizeD(50 * 3, 50 * 3);
        }

        private void CreateCells()
        {
            if (grid == null)
                return;

            var cellSize = 45;

            for (int i = 0; i < 3; i++)
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(cellSize) });

            for (int i = 0; i < 3; i++)
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(cellSize) });


            for (int columnIndex = 0; columnIndex < grid.ColumnDefinitions.Count; columnIndex++)
            {
                for (int rowIndex = 0; rowIndex < grid.RowDefinitions.Count; rowIndex++)
                {
                    var cell = new TicTacToeCell
                    {
                        SuggestedWidth = cellSize - 2,
                        SuggestedHeight = cellSize - 2
                    };

                    cell.Click += Cell_Click;
                    cells.Add(cell);

                    grid.Children.Add(cell);

                    Grid.SetColumn(cell, columnIndex);
                    Grid.SetRow(cell, rowIndex);
                }
            }
        }

        bool gameOver;
        PlayerMark currentPlayerMark;

        void InitializeGame()
        {
            currentPlayerMark = PlayerMark.X;
            gameOver = false;

            foreach (var cell in cells)
            {
                cell.Mark = null;
                cell.IsWinningCell = false;
            }
        }

        private void Cell_Click(object? sender, EventArgs e)
        {
            if (gameOver)
            {
                if (MessageBox.Show(
                    "The game is over. Do you want to start a new game?",
                    "Game Over",
                    MessageBoxButtons.YesNo,
                    MessageBoxDefaultButton.Yes) == DialogResult.Yes)
                {
                    InitializeGame();
                }

                return;
            }

            var cell = (TicTacToeCell)sender!;
            if (cell.Mark != null)
                return;

            cell.Mark = currentPlayerMark;

            currentPlayerMark = currentPlayerMark == PlayerMark.X ? PlayerMark.O : PlayerMark.X;

            gameOver = CheckAndProcessWinner(cells);
        }

    }
}