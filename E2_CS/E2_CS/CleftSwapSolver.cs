using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2_CS
{
    class CleftSwapSolver
    {
        public string Name
		{
			get { return "CleftSwapSolver"; }
		}

	    public double Solve(Problem p, bool findAll = false, Action<double, int, Board, bool> update = null)
        {
            p.ComputeColourDistribution();
            double iterCount = 0;

            Board board = new Board(p.wd, p.ht);
            GenerateFullBoard(p, board);

            board.CleftsToPieces();
            board.CopyToClipboard();



            return iterCount;
        }

        private void GenerateFullBoard(Problem prob, Board board)
        {
            board.SetBordersTo(0);
            
			Stack<int> colors = new Stack<int>(prob.colors.SelectMany( (nb,col) => Enumerable.Repeat( col, col==0 ? 0 : nb/2)));

            for (int x = 0; x < board.cleftCount; ++x) {
				board.SetCleft(x, colors.Pop());
			}
        }
    }
}
