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
            Board board = new Board(p.wd, p.ht);
			CleftSnailOrder ord = new CleftSnailOrder(board);
			PieceFinder finder = new PieceFinder(p.pieces, p.nbPat);
            HashSet<SpunPiece>[] slots = new HashSet<SpunPiece>[board.size];
            slots.FillWith(() => new HashSet<SpunPiece>());

            int nbClefts = board.cleftCount;
            int nbPieces = board.size;
            int cleftIndex = 0;

            Stack<StackElem> stack = new Stack<StackElem>(nbClefts);
            StackElem se = new StackElem();
            var allNeeds = new Dictionary<int,HashSet<int>>();

            double iterCount = 0;
            do {
                ++iterCount;

                int cleftIdx = ord.Idx(cleftIndex);
                Board.PieceSide slot1 = board.cleftToPieceMap[cleftIndex][0];
                Board.PieceSide slot2 = board.cleftToPieceMap[cleftIndex][1];
                var spunSet1 = slots[slot1.index];
                var spunSet2 = slots[slot2.index];
                spunSet1.Clear();
                spunSet2.Clear();
                
                if (stack.Count > cleftIndex)
                {
                    // Retrieve the old color
                    se = stack.Pop();

                    // Remove the needs for the two slots that the cleft coressponds to
                    allNeeds.Remove(slot1.index);
                    allNeeds.Remove(slot2.index);
                }
                else
                {
                    se.color = 0;
                    se.piece1 = 0;
                    se.piece2 = 0;
                }

                // We want to try the next color
                ++se.color;

                // Find first color that's available and valid
                while (se.color != p.nbPat)
                {
                    // Set the color we're going to try to use
                    board.SetCleft(cleftIndex, se.color);

                    // Find all the pieces that could go into slot one
				    int t = board.GetSide(Side.Top,     slot1.index);
				    int r = board.GetSide(Side.Right,   slot1.index);
				    int b = board.GetSide(Side.Bottom,  slot1.index);
				    int l = board.GetSide(Side.Left,    slot1.index);
                    finder.ListAll(t, r, b, l, spunSet1);

                    if (spunSet1.Count > 0)
                    {
                        // Find all the pieces that could go into slot two
				        t = board.GetSide(Side.Top,     slot2.index);
				        r = board.GetSide(Side.Right,   slot2.index);
				        b = board.GetSide(Side.Bottom,  slot2.index);
				        l = board.GetSide(Side.Left,    slot2.index);
                        finder.ListAll(t, r, b, l, spunSet2);

						if (spunSet2.Count > 0)
                        {
                            // Add the needs of each slot to the list of all the needs
                            allNeeds.Remove(slot1.index);
                            allNeeds.Remove(slot2.index);
                            allNeeds.Add(slot1.index, new HashSet<int>(spunSet1.Select(sp => sp.pieceIndex)));
                            allNeeds.Add(slot2.index, new HashSet<int>(spunSet2.Select(sp => sp.pieceIndex)));

                            // Check if there's any way all the needs can be met
                            if (ContentionSolver.IsSatisfiable(allNeeds, p.nbPat))
                            {
                                break; // We've found a suitable color for the cleft
                            }

                            allNeeds.Remove(slot1.index);
                            allNeeds.Remove(slot2.index); 
                            spunSet2.Clear();
                        }
                        spunSet1.Clear();
                    }

					// Try the next color
					++se.color;
                }

				// If we found a suitable color
                if (se.color <= p.nbPat)
                {
                    stack.Push(se);
                    
                    if (update != null) update(iterCount, cleftIndex, board, cleftIndex+1 == nbClefts);

                    ++cleftIndex;
                    if (cleftIndex == nbClefts) {
						if (findAll) --cleftIndex; else break;
					}
                } else {
                    // Restore the fact that there is now no requried color on this cleft
                    board.SetCleft(cleftIndex, -1);

					// Next we're focusing on the previous cleft
                    --cleftIndex;
                }

            }
            while (stack.Count > 0);

            board.CopyToClipboard();

            return iterCount;
        }
    }
}
