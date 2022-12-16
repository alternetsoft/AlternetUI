using System.Collections.Generic;
using System.Linq;

namespace CustomControlsSample
{
    public static class TicTacToeGame
    {
        public enum PlayerMark
        {
            X,
            O
        }

        /// <summary>
        /// Winners contains all the array locations of the winning combination -- if they are all either X or O
        /// (and not blank)
        /// </summary>
        static private int[,] Winners = new int[,]
        {
                {0,1,2},
                {3,4,5},
                {6,7,8},
                {0,3,6},
                {1,4,7},
                {2,5,8},
                {0,4,8},
                {2,4,6}
        };

        /// <summary>
        /// CheckAndProcessWinner determines if either X or O has won. Once a winner has been determined, play
        /// stops.
        /// </summary>
        public static bool CheckAndProcessWinner(IReadOnlyList<TicTacToeCell> cells)
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

    }
}