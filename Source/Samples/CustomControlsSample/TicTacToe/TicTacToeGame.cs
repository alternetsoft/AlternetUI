using System.Collections.Generic;

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

                TicTacToeCell b1 = cells[a], b2 = cells[b], b3 = cells[c];

                if (b1.Mark == null || b2.Mark == null || b3.Mark == null)
                    continue;

                if (b1.Mark == b2.Mark && b2.Mark == b3.Mark)
                {
                    //b1.BackColor = b2.BackColor = b3.BackColor = Color.LightCoral;
                    //b1.Font = b2.Font = b3.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Italic & System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
                    gameOver = true;
                    break;
                }
            }

            return gameOver;
        }

    }
}