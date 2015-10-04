using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2_CS
{
	class Board
	{
        public struct PieceSide
        {
            public PieceSide(int idx, Side s)
            {
                index = idx;
                side = s;
            }

            public int index;
            public Side side;
        }
		public Board(int w, int h, int cPat)
		{
			wd = w;
			ht = h;
			nbPat = cPat;
			size = wd * ht;
			horCleftCount = wd * (ht-1);
			verCleftCount = ht * (wd-1);
            cleftCount = horCleftCount + verCleftCount;
			pieces = new Piece[size];

			lookup = new Point2D[size];
            cleftToPieceMap = new List<PieceSide>[cleftCount];
            cleftToPieceMap.FillWith(()=>new List<PieceSide>(2));
            pieceToCleftMap = new Dictionary<PieceSide, int>();
			int idx = 0;
			for (int y = 0; y < ht; ++y)
			{
				for (int x = 0; x < wd; ++x)
				{
					lookup[idx].x = x;
					lookup[idx].y = y;

                    int t = idx;
                    int r = horCleftCount+x*ht+y;
                    int b = idx-w;
                    int l = r-ht;

                    if (y+1 < ht) pieceToCleftMap.Add(new PieceSide(idx, Side.Top),     t);
                    if (x+1 < wd) pieceToCleftMap.Add(new PieceSide(idx, Side.Right),   r);
                    if (y   >  0) pieceToCleftMap.Add(new PieceSide(idx, Side.Bottom),  b);
                    if (x   >  0) pieceToCleftMap.Add(new PieceSide(idx, Side.Left),    l);

                    if (y+1 < ht) cleftToPieceMap[t].Add(new PieceSide(idx, Side.Top));
                    if (x+1 < wd) cleftToPieceMap[r].Add(new PieceSide(idx, Side.Right));
                    if (y   >  0) cleftToPieceMap[b].Add(new PieceSide(idx, Side.Bottom));
                    if (x   >  0) cleftToPieceMap[l].Add(new PieceSide(idx, Side.Left));
					++idx;
				}
			}
			clefts = new int[cleftCount];
			ResetAllTo(-1);
		}

		public Board(Board b)
		{
			wd = b.wd;
			ht = b.ht;
			nbPat = b.nbPat;
			size = b.size;
            cleftCount = b.cleftCount;
            horCleftCount = b.horCleftCount;
            verCleftCount = b.verCleftCount;

			pieces = new Piece[size];
            Array.Copy(b.pieces, pieces, pieces.Length);

			clefts = new int[horCleftCount+verCleftCount];
            Array.Copy(b.clefts, clefts, clefts.Length);

            // Shallow copy these
            lookup = b.lookup;
            cleftToPieceMap = b.cleftToPieceMap;
            pieceToCleftMap = b.pieceToCleftMap;
		}

		public void	ResetAllTo(int val)
		{
			for (int i = 0; i < size; ++i)
			{
				pieces[i].t = val;
				pieces[i].r = val;
				pieces[i].b = val;
				pieces[i].l = val;
			}

			clefts.FillWith(val);
		}

        public void SetBordersTo(int val)
        {
            for (int y=ht-1; y>=0; --y)
			{
                pieces[wd*y].l = val;
                pieces[wd*y+wd-1].r = val;
            }
			for (int x = 0; x < wd; ++x)
            {
                pieces[x].b = val;
                pieces[x+wd*(ht-1)].t = val;
            }
        }

        public int GetCleft(int i)
        {
            return clefts[i];
        }

        public void SetCleft(int i, int col)
        {
            clefts[i] = col;
            foreach (PieceSide ps in cleftToPieceMap[i]) pieces[ps.index].Set(ps.side, col);
        }

        public void PiecesToClefts()
		{
			int i = 0;
            for (int y=0; y<ht; ++y) {
                for (int x=0; x<wd; ++x) {
                    if (y+1<ht) clefts[i] = pieces[x+y*wd].t;
                    if (y  > 0) clefts[i-wd] = pieces[x+y*wd].b;
                    if (x+1<wd) clefts[horCleftCount+x*ht+y] = pieces[x+y*wd].r;
                    if (x  > 0) clefts[horCleftCount+x*ht+y-ht] = pieces[x+y*wd].l;
					++i;
                }
            }
		}

        public void CleftsToPieces()
        {
			SetBordersTo(0);
			int i = 0;
            for (int y=0; y<ht; ++y) {
                for (int x=0; x<wd; ++x) {
                    if (y+1<ht) pieces[x+y*wd].t = clefts[i];
                    if (y  > 0) pieces[x+y*wd].b = clefts[i-wd];
                    if (x+1<wd) pieces[x+y*wd].r = clefts[horCleftCount+x*ht+y];
                    if (x  > 0) pieces[x+y*wd].l = clefts[horCleftCount+x*ht+y-ht];
					++i;
                }
            }
        }

		public void ShufflePieces(Random rng)
		{
			pieces.Shuffle(rng);
		}

		public double Diff(Board b)
		{
			int d = 0;
			for (int i = 0; i < size; ++i)
			{
				d += pieces[i].Diff(ref b.pieces[i]);
			}
			return (double)(d) / (double)(size*4);
		}

		public void Set(int x, int y, Piece p)
		{
			Set(y*wd+x, p);
		}

		public void Set(int idx, Piece p)
		{
			pieces[idx] = p;
            pieceToCleftMap[new PieceSide(idx, Side.Top)] = p.t; 
            pieceToCleftMap[new PieceSide(idx, Side.Right)] = p.r; 
            pieceToCleftMap[new PieceSide(idx, Side.Bottom)] = p.b; 
            pieceToCleftMap[new PieceSide(idx, Side.Left)] = p.l; 
		}

		public void Set(int idx, int t, int r, int b, int l)
		{
			pieces[idx].Set(t, r, b, l);
            pieceToCleftMap[new PieceSide(idx, Side.Top)] = t; 
            pieceToCleftMap[new PieceSide(idx, Side.Right)] = r; 
            pieceToCleftMap[new PieceSide(idx, Side.Bottom)] = b; 
            pieceToCleftMap[new PieceSide(idx, Side.Left)] = l; 
		}

		public Piece Get(int x, int y)
		{
			return pieces[y*wd+x];
		}

		public Piece Get(int idx)
		{
			return pieces[idx];
		}

		public int GetSide(Side side, int idx)
		{
			Point2D p = lookup[idx];
			switch (side)
			{
				case Side.Left  : return p.x == 0    ? 0 : pieces[idx-1 ].r;
				case Side.Bottom: return p.y == 0    ? 0 : pieces[idx-wd].t;
				case Side.Right : return p.x+1 == wd ? 0 : pieces[idx+1 ].l;
				case Side.Top   : return p.y+1 == ht ? 0 : pieces[idx+wd].b;
				default: throw new Exception("Asking for an unknown side");
			}
		}

		public void CopyToClipboard()
		{
			StringBuilder sb = new StringBuilder(pieces.Length * 3);
			for (int y=ht-1; y>=0; --y)
			{
				sb.Append(' ');
				sb.Append(Get(0,y).ToString());
				sb.Append(' ');
				for (int x = 1; x < wd; ++x)
				{
					sb.Append("    ");
					sb.Append(Get(x,y).ToString());
					sb.Append(' ');
				}
				sb.Append('\n');
			}
			System.Windows.Forms.Clipboard.SetText(sb.ToString());
		}

		public int wd, ht, nbPat, size, cleftCount, horCleftCount, verCleftCount;
		private int[] clefts;
		private Piece[] pieces;
		public Point2D[] lookup;
		public List<PieceSide>[] cleftToPieceMap;
		public Dictionary<PieceSide, int> pieceToCleftMap;
	}

	struct BoardSolution
	{
		public BoardSolution(Board b, double iteration)
		{
			board = new Board(b);
			foundOnIteration = iteration;
		}

		public Board board;
		public double foundOnIteration;
	}
}
