//#define LOG_PROGRESS

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

#if LOG_PROGRESS
		static int PROGRESS_DEPTH = 50;
		static int[] PROGRESS = new int[PROGRESS_DEPTH];
#endif

		public double Solve(Problem p, bool findAll = false, Action<double, int, Board, bool> update = null, Board guide = null)
		{
#if LOG_PROGRESS
			PROGRESS.FillWith(0);
#endif
			Board board = new Board(p.wd, p.ht, p.nbPat);
			board.SetBordersTo(0);
			PieceSnailOrder ord = new CleftSnailOrder(board);
			ContentionSolver contentionSolver = new ContentionSolver(board.size, board.size);
			PieceFinder finder = new PieceFinder(p.pieces, p.nbPat);

			List<SpunPiece> spunSet1 = null;
			List<SpunPiece> spunSet2 = null;
			
			var hs1 = new HashSet<int>();
			var hs2 = new HashSet<int>();

			int nbClefts = board.cleftCount;
			int nbPieces = board.size;
			int cleftNumber = 0;

			Stack<StackElem> stack = new Stack<StackElem>(nbClefts);
			StackElem se = new StackElem();
			var allNeeds = new Dictionary<int,HashSet<int>>();

			double iterCount = 0;
			do {
				++iterCount;

				int cleftIdx = ord.Idx(cleftNumber); //cleftNumber;
				Board.PieceSide slot1 = board.cleftToPieceMap[cleftIdx][0];
				Board.PieceSide slot2 = board.cleftToPieceMap[cleftIdx][1];

				if (stack.Count > cleftNumber)
				{
					// Retrieve the old color
					se = stack.Pop();

					// Remove the needs for the two slots that the cleft coressponds to
					//allNeeds.Remove(slot1.index);
					//allNeeds.Remove(slot2.index);
					contentionSolver.PopInterest(slot2.index);
					contentionSolver.PopInterest(slot1.index);
				}
				else
				{
					se.color = 0;
					se.piece1 = 0;
					se.piece2 = 0;
				}

				// We want to try the next color
				++se.color;
				if (guide != null)
					se.color = guide.GetCleft(cleftIdx);

				// Find first color that's available and valid
				while (se.color != p.nbPat)
				{
#if LOG_PROGRESS
					if (cleftNumber < PROGRESS_DEPTH)
					{
						for (int z=0; z<cleftNumber; ++z) Console.Out.Write("  ");
						Console.Out.WriteLine("{0}:{1}", cleftNumber, se.color);
					}
#endif
					// Set the color we're going to try to use
					board.SetCleft(cleftIdx, se.color);
					//board.CopyToClipboard();

					// Find all the pieces that could go into slot one
					int t = board.GetSide(Side.Top,     slot1.index);
					int r = board.GetSide(Side.Right,   slot1.index);
					int b = board.GetSide(Side.Bottom,  slot1.index);
					int l = board.GetSide(Side.Left,    slot1.index);
					spunSet1 = finder.ListAll(t, r, b, l);

					if (spunSet1.Count > 0)
					{
						// Find all the pieces that could go into slot two
						t = board.GetSide(Side.Top,     slot2.index);
						r = board.GetSide(Side.Right,   slot2.index);
						b = board.GetSide(Side.Bottom,  slot2.index);
						l = board.GetSide(Side.Left,    slot2.index);
						spunSet2 = finder.ListAll(t, r, b, l);

						if (spunSet2.Count > 0)
						{
							// Add the needs of each slot to the list of all the needs
							//allNeeds.Remove(slot1.index);
							//allNeeds.Remove(slot2.index);

							hs1 = new HashSet<int>(spunSet1.Select(sp => sp.pieceIndex));
							hs2 = new HashSet<int>(spunSet2.Select(sp => sp.pieceIndex));
							contentionSolver.PushInterest(slot1.index, hs1);
							contentionSolver.PushInterest(slot2.index, hs2);
							//allNeeds.Add(slot1.index, new HashSet<int>(spunSet1.Select(sp => sp.pieceIndex)));
							//allNeeds.Add(slot2.index, new HashSet<int>(spunSet2.Select(sp => sp.pieceIndex)));

							Dictionary<int,int> solution = null; //new Dictionary<int,int>();
							// Check if there's any way all the needs can be met
							if (contentionSolver.TrySolve())//contentionSolver.Solve(allNeeds, nbPieces, solution))
							{
								break; // We've found a suitable color for the cleft
							}

							contentionSolver.PopInterest(slot2.index);
							contentionSolver.PopInterest(slot1.index);
							//allNeeds.Remove(slot1.index);
							//allNeeds.Remove(slot2.index); 
						}
					}

					// Try the next color
					++se.color;
				}

				// If we found a suitable color
				if (se.color < p.nbPat)
				{
					stack.Push(se);

					if (update != null) update(iterCount, cleftIdx, board, cleftNumber+1 == nbClefts);

					++cleftNumber;
					if (cleftNumber == nbClefts) {
						if (findAll) --cleftNumber; else break;
					}
				} else {
					// Restore the fact that there is now no requried color on this cleft
					board.SetCleft(cleftIdx, -1);

					// Next we're focusing on the previous cleft
					--cleftNumber;
				}

			}
			while (stack.Count > 0);

			board.CopyToClipboard();

			return iterCount;
		}
	}
}
