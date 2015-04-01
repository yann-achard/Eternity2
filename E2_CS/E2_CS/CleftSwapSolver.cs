using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2_CS
{
    class CleftBacktrackSolver
    {
        public string Name
		{
			get { return "CleftBacktrackSolver"; }
		}

        struct StackElem
        {
            public int color;
            public int piece1;
            public int piece2;
        }

	    public double Solve(Problem p, bool findAll = false, Action<double, int, Board, bool> update = null)
        {
            p.ComputeColourDistribution();
            double iterCount = 0;

            Board board = new Board(p.wd, p.ht);
			PieceFinder finder = new PieceFinder(p.pieces, p.nbPat);

            int[] colors = p.colors.Select( c => c / 2 ).ToArray();

            int nbClefts = board.cleftCount;
            int index = 0;

            Stack<StackElem> stack = new Stack<StackElem>(nbClefts);
            StackElem se = new StackElem();

            do {
                ++iterCount;

                if (stack.Count > index)
                {
                    // Retrieve the old color
                    se = stack.Pop();
                    // Place the old color back in the pile
                    ++colors[se.color];

                    // Restore the fact that there is now no requried color on this cleft
                    board.SetCleft(index, -1); // May be able to do that in the else branch below
                }
                else
                {
                    se.color = 1;
                    se.piece1 = 0;
                    se.piece2 = 0;
                }

                // Find the next color that's available and valid
                while (se.color != p.nbPat)
                {
                    if (colors[se.color] > 0)
                    {
                        // Are there pieces that satisfy this cleft being of that color?
                        while (true)
                        {
                            se.piece1 = finder.Acquire();
                            if (se.piece1 != -1) {
                                se.piece2 = finder.Acquire();
                                if (se.piece2 != -1) {
                                }
                                else
                                {
                                    finder.Restore(se.piece1);
                                }
                            }
                        }
                    }
                    ++se.color;
                }

                if (se.color <= p.nbPat)
                {
                    board.SetCleft(index, se.color);
                    stack.Push(se);
                    
                    if (update!=null) update(iterCount, index, board, index+1 == nbClefts);

                    ++index;
                    if (index == nbClefts) {
						if (findAll) --index; else break;
					}
                } else {
                    --index;
                }

            }
            while (stack.Count > 0);

            board.CleftsToPieces();
            board.CopyToClipboard();


            return iterCount;
        }
    }
}
