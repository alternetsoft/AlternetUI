using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.Drawing;
using Alternet.UI;

namespace Alternet.UI
{
    /// <summary>
    /// Implements tic-tac-toe game.
    /// </summary>
    public partial class TicTacToeControl : Control
    {
        /// <summary>
        /// Winners contains all the array locations of the winning combination
        /// -- if they are all either X or O
        /// (and not blank)
        /// </summary>
        private static readonly int[,] Winners = new int[,]
        {
                { 0, 1, 2 },
                { 3, 4, 5 },
                { 6, 7, 8 },
                { 0, 3, 6 },
                { 1, 4, 7 },
                { 2, 5, 8 },
                { 0, 4, 8 },
                { 2, 4, 6 },
        };

        private readonly Grid grid = new()
        {
            Margin = 5,
            Padding = 5,
        };

        private readonly List<TicTacToeCell> cells = [];
        private bool gameOver;
        private PlayerMark currentPlayerMark;

        /// <summary>
        /// Initializes a new instance of the <see cref="TicTacToeControl"/> class.
        /// </summary>
        public TicTacToeControl()
        {
            grid.Parent = this;
            CreateCells();
            InitializeGame();
            SuggestedSize = new SizeD(50 * 3, 50 * 3);
        }

        internal enum PlayerMark
        {
            X,
            O,
        }

        /// <summary>
        /// CheckAndProcessWinner determines if either X or O has won. Once a winner
        /// has been determined, play
        /// stops.
        /// </summary>
        internal static bool CheckAndProcessWinner(IReadOnlyList<TicTacToeCell> cells)
        {
            bool gameOver = false;
            for (int i = 0; i < 8; i++)
            {
                int a = Winners[i, 0], b = Winners[i, 1], c = Winners[i, 2];

                TicTacToeCell c1 = cells[a], c2 = cells[b], c3 = cells[c];

                if (c1.Mark == null || c2.Mark == null || c3.Mark == null)
                    continue;

                if (c1.Mark == c2.Mark && c2.Mark == c3.Mark)
                {
                    c1.IsWinningCell = c2.IsWinningCell = c3.IsWinningCell = true;
                    gameOver = true;
                    break;
                }
            }

            if (!gameOver)
                gameOver = cells.All(x => x.Mark != null);

            return gameOver;
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
                        SuggestedHeight = cellSize - 2,
                    };

                    cell.Click += Cell_Click;
                    cells.Add(cell);

                    grid.Children.Add(cell);

                    Grid.SetColumn(cell, columnIndex);
                    Grid.SetRow(cell, rowIndex);
                }
            }
        }

        private void InitializeGame()
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