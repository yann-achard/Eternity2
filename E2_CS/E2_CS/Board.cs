using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2_CS
{
	class Board
	{
		public Board(int w, int h)
		{
			wd = w;
			ht = h;
			size = wd * ht;
			pieces = new Piece[size];
			lookup = new Point2D[size];
			int idx = 0;
			for (int y = 0; y < ht; ++y)
			{
				for (int x = 0; x < wd; ++x)
				{
					lookup[idx].x = x;
					lookup[idx].y = y;
					++idx;
				}
			}
			ResetAllTo(-1, -1, -1, -1);
		}

		public Board(Board b)
		{
			wd = b.wd;
			ht = b.ht;
			size = b.size;
			pieces = new Piece[size];
			for (int i = wd * ht - 1; i >= 0; --i)
			{
				pieces[i] = b.Get(i);
			}
		}

		public void	ResetAllTo(int t, int r, int b, int l)
		{
			for (int i = 0; i < size; ++i)
			{
				pieces[i].top = t;
				pieces[i].right = r;
				pieces[i].bottom = b;
				pieces[i].left = l;
			}
		}

		public void Shuffle(Random rng)
		{
			pieces.Shuffle(rng);
		}

		public void Set(int x, int y, Piece p)
		{
			pieces[y*wd+x] = p;
		}

		public void Set(int idx, Piece p)
		{
			pieces[idx] = p;
		}

		public void Set(int idx, int t, int r, int b, int l)
		{
			pieces[idx].Set(t, r, b, l);
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
				case Side.Left  : return p.x == 0    ? 0 : pieces[idx-1 ].right;
				case Side.Bottom: return p.y == 0    ? 0 : pieces[idx-wd].top;
				case Side.Right : return p.x+1 == wd ? 0 : pieces[idx+1 ].left;
				case Side.Top   : return p.y+1 == ht ? 0 : pieces[idx+wd].bottom;
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

		public int wd, ht, size;
		private Piece[] pieces;
		private Point2D[] lookup;
	}

	struct BoardSolution
	{
		public BoardSolution(Board b, float iteration)
		{
			board = b;
			foundOnIteration = iteration;
		}

		public Board board;
		public float foundOnIteration;
	}
}
