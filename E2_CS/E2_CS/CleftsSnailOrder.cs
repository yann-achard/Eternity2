using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace E2_CS
{
	class CleftSnailOrder : PieceSnailOrder
	{
		public CleftSnailOrder(Board board) : base(board)
		{
            int wd = board.wd;
            int ht = board.ht;
            HashSet<int> usedClefts = new HashSet<int>();
            int horCleftCount = board.horCleftCount;
            cat = new int[board.cleftCount];
			//pieceAtCleft = new int[board.cleftCount];
            int cleftIdx = 0;
            foreach (var p in at2d)
            {
                int i = p.x + p.y * wd;
                int t = i;
                int r = horCleftCount+p.x*ht+p.y;
                int b = i-wd;
                int l = r-ht;
				//pieceAtCleft[cleftIdx] = i;
                if (p.y + 1 < ht && usedClefts.Add(t)) cat[cleftIdx++] = t;
				//pieceAtCleft[cleftIdx] = i;
                if (p.x + 1 < wd && usedClefts.Add(r)) cat[cleftIdx++] = r;
				//pieceAtCleft[cleftIdx] = i;
                if (p.y     >  0 && usedClefts.Add(b)) cat[cleftIdx++] = b;
				//pieceAtCleft[cleftIdx] = i;
                if (p.x     >  0 && usedClefts.Add(l)) cat[cleftIdx++] = l;
            }
            Debug.Assert(cleftIdx == board.cleftCount);
		}

		//public int PieceIdx(int idx)
		//{
		//	return pieceAtCleft[idx];
		//}

		public override int Idx(int idx)
		{
			return cat[idx];
		}

		public int GetCleft(int idx)
		{
			return board.GetCleft(cat[idx]);
		}

		public void SetCleft(int idx, int col)
		{
			board.SetCleft(cat[idx], col);
		}
		
		private int[] cat;
		//private int[] pieceAtCleft;
	}
}
