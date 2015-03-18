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

		public uint Solve(Problem p, List<Board> solutions, bool findAll = false)
		{
			uint iterCount = 0;
			PieceFinder finder = new PieceFinder(p.pieces, p.nbPat);
			Board board = new Board(p.wd, p.ht);
			SnailOrder ord = new SnailOrder(board);
			Piece resetPiece = new Piece(-1, -1, -1, -1);
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
					++pieceIndex;
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
					ord.Set(index, new Piece(t, r, b, l));
					++index;
					if (index == board.size) {
						solutions.Add(board);
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
