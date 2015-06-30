using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2_CS
{
	class Problem
	{
		public int wd, ht;
		public int nbPat;
		public List<Piece> pieces = new List<Piece>();
		public int[] colors;

		public Problem(Board board)
		{
			wd = board.wd;
			ht = board.ht;
			nbPat = board.nbPat;
			for (int i=0; i<board.size; ++i) {
				pieces.Add(board.Get(i));
			}
		}

		public Problem(int width, int height, int nbPatterns)
		{
		   wd = width;
		   ht = height;
		   nbPat = nbPatterns;
		   colors = new int[nbPat];
		}

		public bool IsEquivalentTo(Board b)
		{
			return IsEquivalentTo(new Problem(b));
		}

		public bool IsEquivalentTo(Problem p)
		{
			if (wd != p.wd || ht != p.ht || nbPat != p.nbPat || pieces.Count != p.pieces.Count)
				return false;

			List<int> thisList = pieces.Select(piece => piece.ToMinInt()).ToList();
			List<int> otherList = p.pieces.Select(piece => piece.ToMinInt()).ToList();
			thisList.Sort();
			otherList.Sort();
			return thisList.SequenceEqual(otherList); 
		}

		public void ComputeColourDistribution()
		{
		   colors.FillWith(0);
		   foreach (Piece p in pieces) {
		       colors[p.t] += 1;
		       colors[p.r] += 1;
		       colors[p.b] += 1;
		       colors[p.l] += 1;
		   }
		}
	}
}
