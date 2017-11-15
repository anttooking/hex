using Hex.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hex
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary> 
        static void Main()
        {

            GameOptions options = new GameOptions() { BoardSize = 5 };

            GameBoard board = new GameBoard(options);


            var spans = board.GetSpansUntilBoardEdge(new HexCoordinate(2, -2, 0));
        }
    }
}
