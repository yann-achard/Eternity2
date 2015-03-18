using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2_CS
{
	struct Piece
	{
		public int top, right, bottom, left;

		public Piece(int t, int r, int b, int l)
		//public Piece(int t=0, int r=0, int b=0, int l=0)
		{
			top = t;
			right = r;
			bottom = b;
			left = l;
		}

		public override string ToString()
		{
			return (top==-1?0:top) + " " + (right==-1?0:right) + " " + (bottom==-1?0:bottom) + " " + (left==-1?0:left);
		}
	}
}
