using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2_CS
{
	class SnailOrder : BoardOrder
	{
		public SnailOrder(Board b)
		{
			board = b;
			int w = b.wd;
			int h = b.ht;
			int nb = w*h;
			at = new int[nb];
			at2d = new Point2D[nb];
			int x = 0;
			int y = h-1;
			int dx = 1;
			int dy = 0;
			int endx = w-1;
			int endy = h-1;
			for (int i=0; i<nb; ++i)
			{
				at[i] = x + y * b.wd;
				at2d[i].x = x;
				at2d[i].y = y;

				if (dx != 0 && x == endx)
				{
					dy = -dx;
					if (dx == 1)
					{
						endy = h-endy-1;
					}
					else
					{
						endy = h-endy-2;
					}
					dx = 0;
				}
				else if (dy!=0 && y == endy)
				{
					dx = dy;
					if (dy == 1)
					{
						endx = w-endx-2;
					}
					else
					{
						endx = w-endx-1;
					}
					dy = 0;
				}
				x += dx;
				y += dy;
			}
		}

		public Piece Get(int idx)
		{
			return board.Get(at[idx]);
		}

		public void Set(int idx, Piece p)
		{
			board.Set(at[idx], p);
		}
		
		public int GetSide(Side side, int idx)
		{
			return board.GetSide(side, at[idx]);
		}

		private Point2D[] at2d;
		private int[] at;
		private Board board;
	}
}
