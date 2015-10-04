using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2_CS
{
	class BacktrackSolver
	{
		public string Name
		{
			get { return "BacktrackSolver"; }
		}

		public double Solve(Problem p, bool findAll = false, Action<double, int, Board, bool> update = null)
		{
			double iterCount = 0;
			PieceFinderOneAtATime finder = new PieceFinderOneAtATime(p.pieces, p.nbPat);
			//PieceFinderOld finder = new PieceFinderOld(p.pieces, p.nbPat);
			Board board = new Board(p.wd, p.ht, p.nbPat);
			PieceSnailOrder ord = new PieceSnailOrder(board);
			Piece resetPiece = new Piece(-1, -1, -1, -1);
			Piece   setPiece = new Piece();
			Stack<int> pieceIndices = new Stack<int>();
			int index = 0;
			do {
				++iterCount;

				int pieceIndex = 0;
				if (pieceIndices.Count > index)
				{
					pieceIndex = pieceIndices.Pop();
					ord.Set(index, resetPiece);
					finder.Restore(pieceIndex);
                    //++pieceIndex;
				}
				int l = ord.GetSide(Side.Left, index);
				int r = ord.GetSide(Side.Right, index);
				int b = ord.GetSide(Side.Bottom, index);
				int t = ord.GetSide(Side.Top, index);
				pieceIndex = finder.Acquire(ref t, ref r, ref b, ref l, pieceIndex);
				if (pieceIndex == -1)
				{
					--index;
					continue;
				}
				else
				{
					pieceIndices.Push(pieceIndex);
					setPiece.Set(t, r, b, l);
					ord.Set(index, setPiece);

					if (update!=null) {
						update(iterCount, ord.Idx(index), board, index+1 == board.size);
					}

					++index;
					if (index == board.size) {
						if (findAll) {
							--index;
						}
						else
						{
							break;
						}
					}
				}
			} while (pieceIndices.Count > 0);


			return iterCount;
		}
	}
}
