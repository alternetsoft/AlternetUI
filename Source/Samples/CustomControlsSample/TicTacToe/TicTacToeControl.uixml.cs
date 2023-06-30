using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static CustomControlsSample.TicTacToeGame;

namespace CustomControlsSample
{
    internal partial class TicTacToeControl : Control
    {
        public TicTacToeControl()
        {
            InitializeComponent();
            CreateCells();
            InitializeGame();
            Size = new Size(50*3,50*3);
        }

        List<TicTacToeCell> cells = new List<TicTacToeCell>();

        private void CreateCells()
        {
            if (grid == null)
                return;

            var cellSize = 45;

            for (int i = 0; i < 3; i++)
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(cellSize)});

            for (int i = 0; i < 3; i++)
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(cellSize) });


            for (int columnIndex = 0; columnIndex < grid.ColumnDefinitions.Count; columnIndex++)
            {
                for (int rowIndex = 0; rowIndex < grid.RowDefinitions.Count; rowIndex++)
                {
                    var cell = new TicTacToeCell
                    {
                        Width= cellSize-2,
                        Height = cellSize-2
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
                    defaultButton: MessageBoxDefaultButton.Yes) == MessageBoxResult.Yes)
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