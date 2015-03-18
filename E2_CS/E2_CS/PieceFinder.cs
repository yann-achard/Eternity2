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

		public int Acquire(ref int t, ref int r, ref int b, ref int l, int firstIdx=0)
		{
			Piece p;
			for (int i=firstIdx; i<m_pieces.Length; ++i)
			{
				if (m_used[i] == false) {
					p = m_pieces[i];
					if (((t == -1 && p.top!=0) || p.top == t) && ((r == -1 && p.right!=0) || p.right == r) && ((b == -1 && p.bottom!=0) || p.bottom == b) && ((l == -1 && p.left!=0) || p.left == l))
					{
						t = p.top;
						r = p.right;
						b = p.bottom;
						l = p.left;
						m_used[i] = true;
						return i;
					}
					if (((r == -1 && p.top!=0) || p.top == r) && ((b == -1 && p.right!=0) || p.right == b) && ((l == -1 && p.bottom!=0) || p.bottom == l) && ((t == -1 && p.left!=0) || p.left == t))
					{
						r = p.top;
						b = p.right;
						l = p.bottom;
						t = p.left;
						m_used[i] = true;
						return i;
					}
					if (((b == -1 && p.top!=0) || p.top == b) && ((l == -1 && p.right!=0) || p.right == l) && ((t == -1 && p.bottom!=0) || p.bottom == t) && ((r == -1 && p.left!=0) || p.left == r))
					{
						b = p.top;
						l = p.right;
						t = p.bottom;
						r = p.left;
						m_used[i] = true;
						return i;
					}
					if (((l == -1 && p.top!=0) || p.top == l) && ((t == -1 && p.right!=0) || p.right == t) && ((r == -1 && p.bottom!=0) || p.bottom == r) && ((b == -1 && p.left!=0) || p.left == b))
					{
						l = p.top;
						t = p.right;
						r = p.bottom;
						b = p.left;
						m_used[i] = true;
						return i;
					}
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
