﻿using System;
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
            m_nbPat = nbPat;
            m_pieces = pieces.ToArray();
            m_count = new int[pieces.Count+1]; // One extra so we can use it as a sentinel
            Array.Clear(m_count, 0, m_count.Length);
            m_piecesIndex = new int[nbPat, nbPat, nbPat, nbPat];
            m_piecesIndex.FillWith(pieces.Count); // Piece signatures that don't have a piece will be directed toward the sentinel
            for (int idx = 0; idx < m_pieces.Length; ++idx)
            {
                Piece p = m_pieces[idx];
                int countIdx = m_piecesIndex[p.t, p.r, p.b, p.l];
                if (countIdx >= pieces.Count)
                {
                    countIdx = idx;
                    m_piecesIndex[p.t, p.r, p.b, p.l] = idx;
                    m_piecesIndex[p.r, p.b, p.l, p.t] = idx;
                    m_piecesIndex[p.b, p.l, p.t, p.r] = idx;
                    m_piecesIndex[p.l, p.t, p.r, p.b] = idx;
                }
                ++m_count[countIdx];
            }
        }

		public int Acquire(ref int t, ref int r, ref int b, ref int l, int firstIdx=0)
		{
            int rmin = r != -1 ? r : 0;
            int bmin = b != -1 ? b : 0;
            int lmin = l != -1 ? l : 0;
            int tmax = t != -1 ? t : m_nbPat - 1; // optim
            int rmax = r != -1 ? r : m_nbPat - 1; // optim
            int bmax = b != -1 ? b : m_nbPat - 1; // optim
            int lmax = l != -1 ? l : m_nbPat - 1; // optim
            t = t != -1 ? t : (firstIdx >> 24)       ;
            r = r != -1 ? r : (firstIdx >> 16) & 0xff;
            b = b != -1 ? b : (firstIdx >>  8) & 0xff;
            l = l != -1 ? l : (firstIdx      ) & 0xff;
            if (firstIdx != 0) ++l;
            for (; t <= tmax; ++t) {
                for (; r <= rmax; ++r) {
                    for (; b <= bmax; ++b) {
                        for (; l <= lmax; ++l) {
                            if (m_count[m_piecesIndex[t, r, b, l]] > 0)
                            {
                                --m_count[m_piecesIndex[t, r, b, l]];
                                return (t<<24) | (r<<16) | (b<<8) | l;
                            }
                        }
                        l = lmin;
                    }
                    b = bmin;
                }
                r = rmin;
            }
			return -1;
		}

        public void Restore(int idx)
        {
            ++m_count[m_piecesIndex[(idx >> 24), (idx >> 16) & 0xff, (idx >> 8) & 0xff, idx & 0xff]];
        }

        public void Restore(int t, int r, int b, int l)
        {
            ++m_count[m_piecesIndex[t, r, b, l]];
        }

        int m_nbPat;
        Piece[] m_pieces;
        int[, , ,] m_piecesIndex;
        int[] m_count;
	}

	class PieceFinderOld
	{
		public PieceFinderOld(IReadOnlyList<Piece> pieces, int nbPat)
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
					if (((t == -1 && p.t!=0) || p.t == t) && ((r == -1 && p.r!=0) || p.r == r) && ((b == -1 && p.b!=0) || p.b == b) && ((l == -1 && p.l!=0) || p.l == l))
					{
						t = p.t;
						r = p.r;
						b = p.b;
						l = p.l;
						m_used[i] = true;
						return i;
					}
					if (((r == -1 && p.t!=0) || p.t == r) && ((b == -1 && p.r!=0) || p.r == b) && ((l == -1 && p.b!=0) || p.b == l) && ((t == -1 && p.l!=0) || p.l == t))
					{
						r = p.t;
						b = p.r;
						l = p.b;
						t = p.l;
						m_used[i] = true;
						return i;
					}
					if (((b == -1 && p.t!=0) || p.t == b) && ((l == -1 && p.r!=0) || p.r == l) && ((t == -1 && p.b!=0) || p.b == t) && ((r == -1 && p.l!=0) || p.l == r))
					{
						b = p.t;
						l = p.r;
						t = p.b;
						r = p.l;
						m_used[i] = true;
						return i;
					}
					if (((l == -1 && p.t!=0) || p.t == l) && ((t == -1 && p.r!=0) || p.r == t) && ((r == -1 && p.b!=0) || p.b == r) && ((b == -1 && p.l!=0) || p.l == b))
					{
						l = p.t;
						t = p.r;
						r = p.b;
						b = p.l;
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
