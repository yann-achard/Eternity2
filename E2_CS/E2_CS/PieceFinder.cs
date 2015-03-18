using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2_CS
{
	class PieceFinder
	{
		public PieceFinder(IReadOnlyList<Piece> pieces, int nbPat)
		{
			m_pieces = pieces.ToArray();
			m_used = new bool[pieces.Count];
			Array.Clear(m_used, 0, m_used.Length);
		}

		public int Acquire(int t, int r, int b, int l, int firstIdx=0)
		{
			Piece p;
			for (int i=firstIdx; i<m_pieces.Length; ++i)
			{
				p = m_pieces[i];
				if (m_used[i] == false && ((t == -1 && p.top!=0) || p.top == t) && ((r == -1 && p.right!=0) || p.right == r) && ((b == -1 && p.bottom!=0) || p.bottom == b) && ((l == -1 && p.left!=0) || p.left == l))
				{
					m_used[i] = true;
					return i;
				}
			}
			return -1;
		}

		public void Restore(int idx)
		{
			m_used[idx] = false;
		}

		Piece[] m_pieces;
		bool[] m_used;
	}
}
